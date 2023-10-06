namespace database_synchronizer.Services;

public static class DynamicList
{
    public static List<dynamic> Create(FileModel fileModel)
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
            
        var listDynamic = new List<dynamic>();

        for (int l = 2; l <= rowns; l++)
        {
            Console.WriteLine("Montando lista de dados...");
            var objDynamic = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;
            
            for (int i = 0; i < fileModel.Columns.Length; i++)
                objDynamic[fileModel.Columns[i].Trim()] = spreadsheet.Cell($"{_columns.ElementAt(i)}{l}").Value.ToString().Trim();

            listDynamic.Add(objDynamic);
        }

        return listDynamic;
    }
}
