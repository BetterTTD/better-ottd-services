#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/OpenTTD/OpenTTD.SignalrHub/OpenTTD.SignalrHub.csproj", "Services/OpenTTD/OpenTTD.SignalrHub/"]
COPY ["Services/OpenTTD/OpenTTD.SignalrHub.Messages/OpenTTD.SignalrHub.Messages.csproj", "Services/OpenTTD/OpenTTD.SignalrHub.Messages/"]
RUN dotnet restore "Services/OpenTTD/OpenTTD.SignalrHub/OpenTTD.SignalrHub.csproj"
COPY . .
WORKDIR "/src/Services/OpenTTD/OpenTTD.SignalrHub"
RUN dotnet build "OpenTTD.SignalrHub.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenTTD.SignalrHub.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenTTD.SignalrHub.dll"]
