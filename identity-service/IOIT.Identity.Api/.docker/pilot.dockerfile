#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["identity-service/IOIT.Identity.Api/IOIT.Identity.Api.csproj", "identity-service/IOIT.Identity.Api/"]
COPY ["shared/IOIT.Shared.Logging/IOIT.Shared.Logging.csproj", "shared/IOIT.Shared.Logging/"]
COPY ["shared/IOIT.Shared.Queues/IOIT.Shared.Queues.csproj", "shared/IOIT.Shared.Queues/"]
COPY ["shared/IOIT.Shared.ViewModels/IOIT.Shared.ViewModels.csproj", "shared/IOIT.Shared.ViewModels/"]
COPY ["shared/IOIT.Shared.Common/IOIT.Shared.Commons.csproj", "shared/IOIT.Shared.Common/"]
COPY ["utilities-service/IOIT.Utilities.Application/IOIT.Utilities.Application.csproj", "utilities-service/IOIT.Utilities.Application/"]
COPY ["shared/IOIT.Shared.Helpers/IOIT.Shared.Helpers.csproj", "shared/IOIT.Shared.Helpers/"]
COPY ["utilities-service/IOIT.Utilities.Domain/IOIT.Utilities.Domain.csproj", "utilities-service/IOIT.Utilities.Domain/"]
COPY ["identity-service/IOIT.Identity.Application/IOIT.Identity.Application.csproj", "identity-service/IOIT.Identity.Application/"]
COPY ["identity-service/IOIT.Identity.Domain/IOIT.Identity.Domain.csproj", "identity-service/IOIT.Identity.Domain/"]
COPY ["identity-service/IOIT.Identity.Infrastructure/IOIT.Identity.Infrastructure.csproj", "identity-service/IOIT.Identity.Infrastructure/"]
RUN dotnet restore "identity-service/IOIT.Identity.Api/IOIT.Identity.Api.csproj"
COPY . .
WORKDIR "/src/identity-service/IOIT.Identity.Api"
RUN dotnet build "IOIT.Identity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IOIT.Identity.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IOIT.Identity.Api.dll", "--environment=Pilot"]