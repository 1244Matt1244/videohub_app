using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace VideoApp.Repositories;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}

public class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _cs;
    public SqlConnectionFactory(IConfiguration config) => _cs = config.GetConnectionString("Default")!;
    public IDbConnection CreateConnection() => new SqlConnection(_cs);
}
