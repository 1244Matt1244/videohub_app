FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["VideoApp.csproj", "./"]
RUN dotnet restore "VideoApp.csproj"
COPY . .
RUN dotnet publish "VideoApp.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "VideoApp.dll"]
