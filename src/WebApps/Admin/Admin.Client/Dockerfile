#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebApps/Admin/Admin.Client/Admin.Client.csproj", "WebApps/Admin/Admin.Client/"]
COPY ["WebApps/Admin/Admin.Shared/Admin.Shared.csproj", "WebApps/Admin/Admin.Shared/"]
RUN dotnet restore "WebApps/Admin/Admin.Client/Admin.Client.csproj"
COPY . .
WORKDIR "/src/WebApps/Admin/Admin.Client"
RUN dotnet build "Admin.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Admin.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Admin.Client.dll"]
