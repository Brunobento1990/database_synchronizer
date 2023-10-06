namespace database_synchronizer.Databases;

public class MysqlDatabase : IDatabase
{
    private readonly string _stringConnection;

    public MysqlDatabase(string stringConnection)
    {
        _stringConnection = stringConnection;
    }
    public Task ExecuteQueryAsync(string query, List<dynamic> data)
    {
        throw new NotImplementedException();
    }
}
