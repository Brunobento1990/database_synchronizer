namespace database_synchronizer.Databases;

public interface IDatabase
{
    Task ExecuteQueryAsync(string query);
}
