FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/OpenTTD/OpenTTD.API/OpenTTD.API.fsproj", "OpenTTD.API/"]
COPY ["Services/OpenTTD/OpenTTD.AdminClient/OpenTTD.AdminClient.fsproj", "OpenTTD.AdminClient/"]
COPY ["Services/OpenTTD/OpenTTD.Networking/OpenTTD.Networking.fsproj", "OpenTTD.Networking/"]
RUN dotnet restore "Services/OpenTTD/OpenTTD.API/OpenTTD.API.fsproj"
COPY . .
WORKDIR "/src/OpenTTD.API"
RUN dotnet build "OpenTTD.API.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenTTD.API.fsproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenTTD.API.dll"]
