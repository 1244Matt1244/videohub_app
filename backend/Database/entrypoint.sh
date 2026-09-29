#!/bin/bash
set -e

/opt/mssql/bin/sqlservr &
SQL_PID=$!

echo "Waiting for SQL Server to start..."
for i in {1..50}; do
    if /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -Q "SELECT 1" &>/dev/null; then
        echo "SQL Server ready!"
        break
    fi
    echo "Attempt $i/50..."
    sleep 2
done

if ! /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -Q "SELECT name FROM sys.databases WHERE name='VideoApp'" | grep -q VideoApp; then
    echo "Creating VideoApp database..."
    /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -i /docker-entrypoint-initdb.d/init.sql
    echo "Database initialized!"
fi

wait $SQL_PID
