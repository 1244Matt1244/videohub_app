#!/bin/sh
set -e

# Učitaj NuGet kredencijale iz Docker secrets
export NUGET_TOKEN="$(cat /run/secrets/nuget_token)"
export NUGET_USERNAME="$(cat /run/secrets/nuget_username)"

# Izvrši glavnu komandu (npr. dotnet ...)
exec "$@"
