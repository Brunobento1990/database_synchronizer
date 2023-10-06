namespace database_synchronizer.Databases;

public static class FactoryDatabase
{
    public static IDatabase GetDatabase(int type, string stringConnection)
    {
        switch (type)
        {
            case 1:
                return new MysqlDatabase(stringConnection);
            default:
                return new PgDatabase(stringConnection);
        }
    }
}
