using System.Globalization;
using System.Text;

namespace database_synchronizer.Services;

public static class DynamicList
{
    public static string Create(FileModel fileModel)
    {
        Console.WriteLine("Lendo a planilha...");
        var xls = new XLWorkbook(fileModel.Path);
        var spreadsheet = xls.Worksheets.First(w => w.Name == fileModel.Spreadsheet);
        var rowns = spreadsheet.Rows().Count();
        List<string> _columns = new()
        {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I"
        };
        StringBuilder sqlBuilder = new StringBuilder($"insert into {fileModel.Tabela} ({fileModel.ColumnsSql}) values ");

        for (int l = 2; l <= rowns; l++)
        {
            Console.WriteLine("Montando lista de dados...");
            var objDynamic = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;
            sqlBuilder.Append('(');
            for (int i = 0; i < fileModel.Columns.Length; i++)
            {
                var value = spreadsheet.Cell($"{_columns.ElementAt(i)}{l}").Value.ToString().Trim();
                DateTime dateValue;
                string format = "dd/MM/yyyy HH:mm:ss";

                bool success = DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);

                if (success)
                {
                    value = dateValue.ToString("yyyy-MM-dd");
                }
                sqlBuilder.Append($"'{value}',");
            }

            if (sqlBuilder.Length > 0)
            {
                sqlBuilder.Length--;
            }
            sqlBuilder.Append("),");
        }
        if (sqlBuilder.Length > 0)
        {
            sqlBuilder.Length--;
        }

        return sqlBuilder.ToString();
    }
}
