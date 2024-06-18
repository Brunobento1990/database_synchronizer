namespace database_synchronizer.Models;

public class FileModel
{
    public FileModel(string? path, string? spreadsheet, string? columns, string tabela, string columnsSql)
    {
        Validations.ValidateString(path);
        Validations.ValidateString(spreadsheet);
        Validations.ValidateString(columns);

        Path = path;
        Spreadsheet = spreadsheet;
        Columns = columns.Split(",");
        Tabela = tabela;
        ColumnsSql = columnsSql;
    }

    public string Path { get; set; }
    public string Tabela { get; set; }
    public string Spreadsheet { get; set; }
    public string[] Columns { get; set; }
    public string ColumnsSql { get; set; }
}
