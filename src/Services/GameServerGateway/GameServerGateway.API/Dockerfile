FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/GameServerGateway/GameServerGateway.API/GameServerGateway.API/GameServerGateway.API.csproj", "Services/GameServerGateway/GameServerGateway.API/GameServerGateway.API/"]
RUN dotnet restore "Services/GameServerGateway/GameServerGateway.API/GameServerGateway.API/GameServerGateway.API.csproj"
COPY . .
WORKDIR "/src/Services/GameServerGateway/GameServerGateway.API/GameServerGateway.API"
RUN dotnet build "GameServerGateway.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameServerGateway.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameServerGateway.API.dll"]
