using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
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
using Investment.Infrastructure.Domain.Contracts.Repository;
using Investment.Infrastructure.Domain.Transactions.Repositories;
using Investment.Infrastructure.Domain.Users.Repositories;
using Investment.Infrastructure.Services;
using Investment.Infrastructure.Services.Contract;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Investment.Infrastructure;

public static class ServicesRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection Services, ConfigurationManager configuration)
    {
        Services.AddScoped<ITokenAuthenticateService, TokenAuthenticateService>();
        Services.AddScoped<IUserInfoService, UserInfoService>();
        Services.AddScoped<IJwtTokenService, JwtTokenService>();
        Services.AddSingleton<IHashHelperService, HashHelperService>();

        Services.AddHttpContextAccessor();

        Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(UserCreateCommand).Assembly);
        });

        Services.AddValidatorsFromAssembly(typeof(UserCreateCommand).Assembly);

        Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        Services.AddScoped<IUnitOfWork, UnitOfWork>();

        Services.AddScoped<IUserCommandRepository, UserCommandRepository>();
        Services.AddScoped<IUserQueryRepository, UserQueryRepository>();
        Services.AddScoped<IUserBalanceCommandRepository, UserBalanceCommandRepository>();
        Services.AddScoped<IUserBalanceQueryRepository, UserBalanceQueryRepository>();
        Services.AddScoped<IContractCommandRepository, ContractCommandRepository>();
        Services.AddScoped<IContractQueryRepository, ContractQueryRepository>();

        Services.AddScoped<IEmailSenderService, EmailSenderService>();
        Services.AddScoped<IObjectStoreService, ObjectStoreService>();

        Services.AddScoped<ICryptoPaymentsService, CryptoPaymentsService>();
        Services.AddSingleton<HmacSha512WebhookVerifier>();
        Services.AddScoped<CryptoPaymentsClientFactory>();

        Services.AddHangfire(config =>
        {
            config.UsePostgreSqlStorage(options => options.UseNpgsqlConnection(configuration.GetConnectionString("Postgres")));
        });

        Services.AddHangfireServer();

        Services.AddScoped<ContractProfitAllocator>();

        return Services;
    }
}
