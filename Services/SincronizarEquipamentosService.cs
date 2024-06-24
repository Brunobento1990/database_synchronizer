using Npgsql;
using System.Globalization;
using System.Text;

namespace database_synchronizer.Services;

public class SincronizarEquipamentosService
{
    public async Task Sincronizar()
    {
        var conn = "";
        Console.WriteLine("Lendo a planilha...");
        var xls = new XLWorkbook("D:\\EQUIPAMENTOS.xlsx");
        var spreadsheet = xls.Worksheets.First(w => w.Name == "Contabilidade - Consulta detalh");
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

        for (int i = 2; i <= rowns; i++)
        {
            using var connection = new NpgsqlConnection(conn);
            using var connection2 = new NpgsqlConnection(conn);
            await connection.OpenAsync();
            await connection2.OpenAsync();

            var equipamento = spreadsheet.Cell($"A{i}").Value.ToString().Trim();
            var patrimonio = spreadsheet.Cell($"B{i}").Value.ToString().Trim();
            var modelo = spreadsheet.Cell($"C{i}").Value.ToString().Trim();
            var numeroDeSerie = spreadsheet.Cell($"D{i}").Value.ToString().Trim();
            var cost = spreadsheet.Cell($"E{i}").Value.ToString().Trim();
            var dataAquisicao = spreadsheet.Cell($"F{i}").Value.ToString().Trim();
            var codigoLoca = spreadsheet.Cell($"G{i}").Value.ToString().Trim();
            var setor = spreadsheet.Cell($"H{i}").Value.ToString().Trim();
            var preco = spreadsheet.Cell($"I{i}").Value.ToString().Trim();

            DateTime dateValue = DateTime.MinValue;
            string format = "dd/MM/yyyy HH:mm:ss";

            bool success = DateTime.TryParseExact(dataAquisicao, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);

            if (success)
            {
                dataAquisicao = dateValue.ToString("yyyy-MM-dd");
            }

            var querySelect = $"select id from sectors where description = '{setor}' LIMIT 1";

            using var commandSelect = new NpgsqlCommand(querySelect, connection);
            var setorSelect = await commandSelect.ExecuteReaderAsync();
            Guid? setorId = null;
            if (setorSelect.HasRows)
            {
                await setorSelect.ReadAsync();
                setorId = setorSelect.GetGuid(0);
            }
            else
            {
                var querySelect3 = $"select id from sectors where id = 'f8a2ca2c-ab66-4c12-b8c7-3a14fe0e5b50' order by dtcreation LIMIT 1";
                using var connection3 = new NpgsqlConnection(conn);
                await connection3.OpenAsync();
                using var commandSelect3 = new NpgsqlCommand(querySelect3, connection3);
                var setorSelect3 = await commandSelect3.ExecuteReaderAsync();

                if (setorSelect3.HasRows) 
                {
                    await setorSelect3.ReadAsync();
                    setorId = setorSelect3.GetGuid(0);
                }

                await connection3.CloseAsync();
            }

            var query = "insert into equipments (" +
                "patrimony, " +
                "model, " +
                "\"serialNumber\", " +
                "\"cost\", " +
                "\"data\", " +
                "\"localCode\", " +
                "dtcreation, " +
                "equipament, " +
                "\"sectorId\", " +
                "price, " +
                "status) values";
            query += $"('{patrimonio}','{modelo}', '{numeroDeSerie}','{cost}', '{dataAquisicao}', '{codigoLoca}', now(), '{equipamento}',{(setorId == null ? "NULL" : $"'{setorId}'")}, {preco.Replace(",", ".")}, true)";

            using var command = new NpgsqlCommand(query, connection2);
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            await connection2.CloseAsync();
        }

    }
}
