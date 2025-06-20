#!/bin/sh
export NUGET_TOKEN=$(cat /run/secrets/nuget_token)
export NUGET_USERNAME=$(cat /run/secrets/nuget_username)

exec "$@"
