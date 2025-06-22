#!/bin/bash
set -e

echo "➡️ Building and starting containers..."
docker-compose up -d --build

echo "➡️ Applying EF migrations..."
docker exec videohub-backend dotnet ef database update

echo "➡️ Starting frontend dev server..."
cd frontend
npm install
npm run dev