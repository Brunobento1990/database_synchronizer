using Npgsql;

namespace database_synchronizer.Databases;

public class PgDatabase : IDatabase
{
    private readonly string _stringConnection;

    public PgDatabase(string stringConnection)
    {
        _stringConnection = stringConnection;
    }

    public async Task ExecuteQueryAsync(string query)
    {

        using var connection = new NpgsqlConnection(_stringConnection);
        try
        {
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(query, connection);
            int rowsAffected = await command.ExecuteNonQueryAsync();

            Console.WriteLine($"{rowsAffected} registros inseridos.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
        finally
        {
            await connection.CloseAsync();
        }

    }
}
