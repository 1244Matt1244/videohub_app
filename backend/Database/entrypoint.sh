#!/bin/bash
set -e

# Hardkodirana lozinka – mora odgovarati MSSQL_SA_PASSWORD u docker-compose.yml
SA_PASSWORD="YourStrong@Passw0rd"

# Pokreni SQL Server u pozadini
/opt/mssql/bin/sqlservr &
SQL_PID=$!

echo "Waiting for SQL Server to start (max 10 minutes)..."
READY=0
for i in $(seq 1 200); do
    if /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -C -Q "SELECT 1" &>/dev/null; then
        echo "SQL Server ready after $i attempts!"
        READY=1
        break
    fi
    if [ $((i % 10)) -eq 0 ]; then
        echo "Attempt $i/200..."
    fi
    sleep 3
done

if [ "$READY" -ne 1 ]; then
    echo "ERROR: SQL Server did not become ready in time."
    exit 1
fi

# Kreiraj bazu ako ne postoji
if ! /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -C -Q "SELECT name FROM sys.databases WHERE name='VideoApp'" -h -1 | grep -q VideoApp; then
    echo "Creating VideoApp database..."
    /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -C -i /docker-entrypoint-initdb.d/init.sql
    echo "Database initialized!"
else
    echo "VideoApp database already exists."
fi

wait $SQL_PID
