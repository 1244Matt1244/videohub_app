# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set radni direktorij
WORKDIR /src

# Kopiraj samo csproj i nuget.config za efikasniji cache
COPY ["VideoHubApp.Backend.csproj", "./"]
COPY ["nuget.config", "./"]

# Restore uz nuget.config (za Mux.Video)
RUN dotnet restore VideoHubApp.Backend.csproj --configfile nuget.config

# Kopiraj ostatak projekta
COPY . .

# Build aplikaciju (publish u Release režimu)
RUN dotnet publish VideoHubApp.Backend.csproj -c Release -o /app --no-restore

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Postavi radni direktorij
WORKDIR /app

# Kopiraj iz build stage-a
COPY --from=build /app .

# Startaj aplikaciju
ENTRYPOINT ["dotnet", "VideoHubApp.Backend.dll"]
