using System.Dynamic;

namespace database_synchronizer.Databases;

public class PgDatabase : IDatabase
{
    private readonly string _stringConnection;

    public PgDatabase(string stringConnection)
    {
        _stringConnection = stringConnection;
    }

    public Task ExecuteQueryAsync(string query, List<dynamic> data)
    {

        //private static dynamic? GetPropertyValue(ExpandoObject expando, string propertyName)
        //{
        //    var expandoDict = expando as IDictionary<string, dynamic>;
        //    if (expandoDict != null && expandoDict.ContainsKey(propertyName))
        //    {
        //        return expandoDict[propertyName];
        //    }
        //    return null;
        //}

        throw new NotImplementedException();
    }
}
