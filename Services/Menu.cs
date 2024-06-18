using database_synchronizer.Databases;
using System.Text.RegularExpressions;

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

        Console.WriteLine("Informe o nome da tabela do banco de dados!");
        var tabela = Console.ReadLine();

        var fileModel = new FileModel(path, spreadsheet, columns, tabela, columns);

        var sql = DynamicList.Create(fileModel);

        Console.WriteLine("Informe o tipo de banco de dados: \\n 1 - Mysql \\n 2 - Postgres");
        var typeDb = int.Parse(Console.ReadLine() ?? "0");

        Console.WriteLine("Informe a string de conexão com o banco de dados!");
        var stringConnection = Console.ReadLine();

        if (string.IsNullOrEmpty(stringConnection)) throw new Exception("String de conexão inválida!");

        var db = FactoryDatabase.GetDatabase(typeDb, stringConnection);

        await db.ExecuteQueryAsync(sql);

        return true;
    }
}
