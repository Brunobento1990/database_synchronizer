using database_synchronizer.Databases;

namespace database_synchronizer.Services;

public class Menu
{
    public async Task<bool> Init()
    {
        Console.WriteLine("Bem vindo ao sincronizador!");
        Console.WriteLine("Para iniciar, informe o caminho do arquivo!");
        var path = Console.ReadLine();

        if (!File.Exists(path))
        {
            Console.WriteLine("Não foi possível encontrar o arquivo!");
            return false;
        }

        Console.WriteLine("Informe o nome da planilha!");
        var spreadsheet = Console.ReadLine();

        Console.WriteLine("Informe o nome das colunas, separado por virgula!");
        var columns = Console.ReadLine();

        var fileModel = new FileModel(path, spreadsheet, columns);

        var data = DynamicList.Create(fileModel);

        Console.WriteLine("Informe o tipo de banco de dados: \\n 1 - Postgres \\n 2 - Mysql");
        var typeDb = int.Parse(Console.ReadLine() ?? "0");

        Console.WriteLine("Informe a string de conexão com o banco de dados!");
        var stringConnection = Console.ReadLine();

        if (string.IsNullOrEmpty(stringConnection)) throw new Exception("String de conexão inválida!");

        var db = FactoryDatabase.GetDatabase(typeDb, stringConnection);

        await db.ExecuteQueryAsync("", data);

        return true;
    }
}
