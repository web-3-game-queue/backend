#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GameQueue.Host/GameQueue.Host.csproj", "GameQueue.Host/"]
COPY ["GameQueue.AppServices/GameQueue.AppServices.csproj", "GameQueue.AppServices/"]
COPY ["GameQueue.Core/GameQueue.Core.csproj", "GameQueue.Core/"]
COPY ["GameQueue.Api.Contracts/GameQueue.Api.Contracts.csproj", "GameQueue.Api.Contracts/"]
COPY ["GameQueue.AuthTokensCache/GameQueue.AuthTokensCache.csproj", "GameQueue.AuthTokensCache/"]
COPY ["GameQueue.DataAccess/GameQueue.DataAccess.csproj", "GameQueue.DataAccess/"]
COPY ["GameQueue.S3Access/GameQueue.S3Access.csproj", "GameQueue.S3Access/"]
RUN dotnet restore "GameQueue.Host/GameQueue.Host.csproj"
COPY . .
WORKDIR "/src/GameQueue.Host"
RUN dotnet build "GameQueue.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameQueue.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameQueue.Host.dll"]