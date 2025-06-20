# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Build args za NuGet autentikaciju
ARG NUGET_USERNAME
ARG NUGET_PASSWORD

# Kopiraj csproj i nuget.config za cache
COPY ["VideoHubApp.Backend.csproj", "./"]
COPY ["nuget.config", "./"]

# Zamijeni placeholder-e u nuget.config sa stvarnim vrijednostima argumenata
RUN sed -i "s|__NUGET_USERNAME__|$NUGET_USERNAME|g" nuget.config && \
    sed -i "s|__NUGET_PASSWORD__|$NUGET_PASSWORD|g" nuget.config

# Restore koristeći ažurirani nuget.config
RUN dotnet restore VideoHubApp.Backend.csproj --configfile nuget.config

# Kopiraj ostatak koda
COPY . .

# Build i publish aplikaciju
RUN dotnet publish VideoHubApp.Backend.csproj -c Release -o /app --no-restore

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app .

ENTRYPOINT ["dotnet", "VideoHubApp.Backend.dll"]
