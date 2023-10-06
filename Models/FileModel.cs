namespace database_synchronizer.Models;

public class FileModel
{
    public FileModel(string? path, string? spreadsheet, string? columns)
    {
        Validations.ValidateString(path);
        Validations.ValidateString(spreadsheet);
        Validations.ValidateString(columns);

        Path = path;
        Spreadsheet = spreadsheet;
        Columns = columns.Split(",");
    }

    public string Path { get; set; }
    public string Spreadsheet { get; set; }
    public string[] Columns { get; set; }
}
