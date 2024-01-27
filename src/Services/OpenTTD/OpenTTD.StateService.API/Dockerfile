FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/OpenTTD/OpenTTD.StateService/OpenTTD.StateService.csproj", "Services/OpenTTD/OpenTTD.StateService/"]
RUN dotnet restore "Services/OpenTTD/OpenTTD.StateService/OpenTTD.StateService.csproj"
COPY . .
WORKDIR "/src/Services/OpenTTD/OpenTTD.StateService"
RUN dotnet build "OpenTTD.StateService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenTTD.StateService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenTTD.StateService.dll"]
