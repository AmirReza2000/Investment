using FluentValidation;
using Hangfire;
using Investment.Api.Utilities;
using Investment.Application.Users.Commands;
using Investment.Application.Utilities;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Configs;
using Investment.Application.Utilities.FluentValidations;
using Investment.Application.Utilities.QueryRepositories;
using Investment.Domain.Common;
using Investment.Domain.Contracts.Repository;
using Investment.Domain.Transactions.Repositories;
using Investment.Domain.Users;
using Investment.Domain.Users.Repositories;
using Investment.Infrastructure;
using Investment.Infrastructure.Domain.Contracts.Repository;
using Investment.Infrastructure.Domain.Transactions.Repositories;
using Investment.Infrastructure.Domain.Users.Repositories;
using Investment.Infrastructure.Services;
using Investment.Infrastructure.Services.Contract;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Npgsql;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection.Metadata;
using System.Text;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

// -------------------- Serilog --------------------
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);

// -------------------- Authenticate --------------------
var authenticateConfig = builder.Configuration.GetSection(AuthenticateConfig.SectionNameInAppsettings).Get<AuthenticateConfig>();
var IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticateConfig!.SecretKey));

var tokenValidationParameters = new TokenValidationParameters()
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = IssuerSigningKey,
    ValidateAudience = true,
    ValidateIssuer = true,
    ValidateLifetime = true,
    ValidIssuer = authenticateConfig.Issuer,
    ValidAudience = authenticateConfig.Audience,
    RequireExpirationTime = true,
    ClockSkew = TimeSpan.Zero
};

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    jwt.TokenValidationParameters = tokenValidationParameters;
}).AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
       ApiKeyAuthenticationHandler.SchemeName, null); ;

// -------------------- Controllers & Swagger --------------------
builder.Services.AddControllers(options =>
{
    options.AllowEmptyInputInBodyModelBinding = false;
    options.Filters.Add<ModelSateFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "nova_backend", Version = "v1" });

    // IMPORTANT: id = "bearer" (lowercase), Scheme = "bearer" (lowercase)
    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key authentication",
        Name = "X-API-Key",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKey"
    });
    // NEW in v10: AddSecurityRequirement takes a factory with the OpenAPI document
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        // NEW in v10: OpenApiSecuritySchemeReference
        [new OpenApiSecuritySchemeReference("bearer", document)] = [],
        [new OpenApiSecuritySchemeReference("ApiKey", document)] = []

    });
});

// -------------------- DbContext (PostgreSQL + EF Core) --------------------
var dataSourceBuilder = new NpgsqlDataSourceBuilder(
    builder.Configuration.GetConnectionString("Postgres"));

dataSourceBuilder.EnableDynamicJson();

var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<InvestmentDbContext>(options =>
{
    options.UseNpgsql(dataSource, npgsql =>
    {
        npgsql.MigrationsAssembly(typeof(InvestmentDbContext).Assembly.FullName);
        //npgsql.EnableRetryOnFailure(3);

    });

    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
    options.EnableDetailedErrors(builder.Environment.IsDevelopment());
});

// -------------------- DI--------------------

builder.Services.AddHttpClient(CryptoPaymentsClientFactory.ClientFactoryName, client =>
{
    var cryptoPaymentConfig = builder.Configuration
        .GetSection(CryptoPaymentConfig.SectionNameInAppsettings)
        .Get<CryptoPaymentConfig>() ?? throw new Exception("can't load CryptoPaymentConfig;");

    var baseAddress = cryptoPaymentConfig.BaseAddress.Trim();

    var scheme = cryptoPaymentConfig.IsSecure ? "https://" : "http://";

    client.BaseAddress = new Uri(scheme + baseAddress);

    client.Timeout = TimeSpan.FromSeconds(30);

}).SetHandlerLifetime(TimeSpan.FromMinutes(10));

// -------------------- Add Custom Exception --------------------

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

#region Configs
builder.Services.Configure<AuthenticateConfig>(
    builder.Configuration.GetSection(AuthenticateConfig.SectionNameInAppsettings));

builder.Services.Configure<EmailServerSettings>(
    builder.Configuration.GetSection(EmailServerSettings.SectionNameInAppsettings));

builder.Services.Configure<RegistrationVerifyEmailSettings>(
    builder.Configuration.GetSection(RegistrationVerifyEmailSettings.SectionNameInAppsettings));

builder.Services.Configure<ResetPasswordVerifyEmailSettings>(
    builder.Configuration.GetSection(ResetPasswordVerifyEmailSettings.SectionNameInAppsettings));

builder.Services.Configure<ObjectStoreConfig>(
    builder.Configuration.GetSection(ObjectStoreConfig.SectionNameInAppsettings));

builder.Services.Configure<CryptoPaymentConfig>(
    builder.Configuration.GetSection(CryptoPaymentConfig.SectionNameInAppsettings));

#endregion /Configs

builder.Services.AddServices(builder.Configuration);

builder.Services.AddHostedService<ContractJob>();

builder.Host.UseDefaultServiceProvider(opt =>
{
    opt.ValidateOnBuild = true;
    opt.ValidateScopes = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        config.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
        config.DocExpansion(DocExpansion.None);
    });
}

app.UseExceptionHandler();

app.UseSerilogRequestLogging(opts =>
{
    opts.GetLevel = (ctx, elapsed, ex) =>
    {
        if (ex != null) return Serilog.Events.LogEventLevel.Debug;

        if (ctx.Response.StatusCode >= 500) return Serilog.Events.LogEventLevel.Error;
        if (ctx.Response.StatusCode >= 400) return Serilog.Events.LogEventLevel.Warning;

        return Serilog.Events.LogEventLevel.Information;
    };
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard(builder.Configuration.GetValue<string>("HangfireSettings:DashboardPath"), new DashboardOptions
{
    Authorization = new[]
    {
        new HangfireBasicAuthAuthorizationFilter(
            builder.Configuration.GetValue<string>("HangfireSettings:Username")!,
            builder.Configuration.GetValue<string>("HangfireSettings:Password")!)
    }
});

app.Run();



