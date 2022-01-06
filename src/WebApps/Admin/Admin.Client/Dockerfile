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

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY WebApps/Admin/Admin.Client/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80