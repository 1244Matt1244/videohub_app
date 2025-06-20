# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Kopiraj samo csproj i nuget.config da iskoristimo cache kad nije bilo promjena
COPY ["VideoHubApp.Backend.csproj", "./"]
COPY ["nuget.config", "./"]

# Ako koristiš docker secrets za autentikaciju nugeta, ovdje trebaš ubaciti RUN s --mount=type=secret (vidi napomenu dolje)
RUN dotnet restore VideoHubApp.Backend.csproj --configfile nuget.config

# Kopiraj ostatak koda
COPY . .

# Build i publish aplikaciju u Release mode, bez restore jer smo ga već napravili
RUN dotnet publish VideoHubApp.Backend.csproj -c Release -o /app --no-restore

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app .

ENTRYPOINT ["dotnet", "VideoHubApp.Backend.dll"]
