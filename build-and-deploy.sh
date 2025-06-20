#!/bin/sh

echo "🛠️  Building images..."
docker build -t videohub-backend ./backend
docker build -t videohub-frontend ./frontend

echo "🚀 Deploying stack..."
docker stack deploy -c docker-compose.yml videohub
