FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files (adjusted to include src/)
COPY ./src/1.Core/Investment.Application/Investment.Application.csproj 1.Core/Investment.Application/
COPY ./src/1.Core/Investment.Domain/Investment.Domain.csproj 1.Core/Investment.Domain/
COPY ./src/2.Infrastructure/Investment.Infrastructure/Investment.Infrastructure.csproj 2.Infrastructure/Investment.Infrastructure/
COPY ./src/3.Endpoints/Investment.Api/Investment.Api.csproj 3.Endpoints/Investment.Api/

RUN dotnet restore 3.Endpoints/Investment.Api/Investment.Api.csproj

# Copy the rest of the source
COPY ./src/ .

RUN dotnet publish 3.Endpoints/Investment.Api/Investment.Api.csproj \
    -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Investment.Api.dll"]
