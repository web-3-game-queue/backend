#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GameQueue.Backend/GameQueue.Host.csproj", "GameQueue.Backend/"]
COPY ["GameQueue.AppServices/GameQueue.AppServices.csproj", "GameQueue.AppServices/"]
COPY ["GameQueue.Core/GameQueue.Core.csproj", "GameQueue.Core/"]
COPY ["GameQueue.Backend.Api.Contracts/GameQueue.Api.Contracts.csproj", "GameQueue.Backend.Api.Contracts/"]
COPY ["GameQueue.DataAccess/GameQueue.DataAccess.csproj", "GameQueue.DataAccess/"]
RUN dotnet restore "GameQueue.Backend/GameQueue.Host.csproj"
COPY . .
WORKDIR "/src/GameQueue.Backend"
RUN dotnet build "GameQueue.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameQueue.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameQueue.Host.dll"]