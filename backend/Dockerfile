# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy csproj and nuget.config to use cache
COPY ["VideoHubApp.Backend.csproj", "./"]
COPY ["nuget.config", "./"]

# Mount secrets za nuget autentikaciju i izvrši restore
RUN --mount=type=secret,id=nuget_username \
    --mount=type=secret,id=nuget_password \
    NUGET_USERNAME=$(cat /run/secrets/nuget_username) && \
    NUGET_PASSWORD=$(cat /run/secrets/nuget_password) && \
    sed -i "s|__NUGET_USERNAME__|$NUGET_USERNAME|g" nuget.config && \
    sed -i "s|__NUGET_PASSWORD__|$NUGET_PASSWORD|g" nuget.config && \
    dotnet restore VideoHubApp.Backend.csproj --configfile nuget.config

COPY . .

RUN dotnet publish VideoHubApp.Backend.csproj -c Release -o /app --no-restore

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app .

ENTRYPOINT ["dotnet", "VideoHubApp.Backend.dll"]
