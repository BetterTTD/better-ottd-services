FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/OpenTTD/OpenTTD.AdminClient/OpenTTD.AdminClient.csproj", "Services/OpenTTD/OpenTTD.AdminClient/"]
COPY ["Shared/Domain/Domain.csproj", "Shared/Domain/"]
COPY ["Shared/Common/Common.csproj", "Shared/Common/"]
COPY ["Shared/OpenTTD.Networking/OpenTTD.Networking.csproj", "Shared/OpenTTD.Networking/"]
COPY ["Services/OpenTTD/OpenTTD.Actors/OpenTTD.Actors.csproj", "Services/OpenTTD/OpenTTD.Actors/"]
RUN dotnet restore "Services/OpenTTD/OpenTTD.AdminClient/OpenTTD.AdminClient.csproj"
COPY . .
WORKDIR "/src/Services/OpenTTD/OpenTTD.AdminClient"
RUN dotnet build "OpenTTD.AdminClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenTTD.AdminClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenTTD.AdminClient.dll"]
