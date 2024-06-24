using Npgsql;
using System.Globalization;

namespace database_synchronizer.Services;

public class SincronizarOsService
{
    public async Task Sincronizar()
    {
        var conn = "";
        Console.WriteLine("Lendo a planilha...");
        var xls = new XLWorkbook("D:\\UPDATEOS.xlsx");
        var spreadsheet = xls.Worksheets.First(w => w.Name == "Planilha1");
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
            using var connection3 = new NpgsqlConnection(conn);
            await connection.OpenAsync();
            await connection2.OpenAsync();
            await connection3.OpenAsync();

            var status = spreadsheet.Cell($"A{i}").Value.ToString().Trim();
            var patrimonio = spreadsheet.Cell($"B{i}").Value.ToString().Trim();
            var falha = spreadsheet.Cell($"C{i}").Value.ToString().Trim();
            //var numeroDeSerie = spreadsheet.Cell($"D{i}").Value.ToString().Trim();
            var chamado = spreadsheet.Cell($"E{i}").Value.ToString().Trim();
            var nf = spreadsheet.Cell($"F{i}").Value.ToString().Trim();
            var data = spreadsheet.Cell($"G{i}").Value.ToString().Trim();
            var valor = spreadsheet.Cell($"H{i}").Value.ToString().Trim();
            var os = spreadsheet.Cell($"I{i}").Value.ToString().Trim();
            var sdcv = spreadsheet.Cell($"J{i}").Value.ToString().Trim();

            DateTime dateValue = DateTime.MinValue;
            string format = "dd/MM/yyyy HH:mm:ss";

            bool success = DateTime.TryParseExact(data, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);

            if (success)
            {
                data = dateValue.ToString("yyyy-MM-dd");
            }
            else
            {
                data = DateTime.Now.ToString("yyyy-MM-dd");
            }

            var querySelect = $"select id from \"statusOs\" where description = '{status}' LIMIT 1";
            var querySelectEquipamento = $"select id from \"equipments\" where patrimony = '{patrimonio}' LIMIT 1";

            using var commandSelect = new NpgsqlCommand(querySelect, connection);
            var setorSelect = await commandSelect.ExecuteReaderAsync();
            Guid? statusId = null;
            if (setorSelect.HasRows)
            {
                await setorSelect.ReadAsync();
                statusId = setorSelect.GetGuid(0);
            }
            else
            {
                using var connection4 = new NpgsqlConnection(conn);
                var querySelect4 = $"select id from \"statusOs\" where id = 'efb5fb7a-0255-47d7-82a6-14ede93b58d7' LIMIT 1";
                await connection4.OpenAsync();
                using var commandSelect4 = new NpgsqlCommand(querySelect4, connection4);
                var setorSelect4 = await commandSelect4.ExecuteReaderAsync();

                if (setorSelect4.HasRows)
                {
                    await setorSelect4.ReadAsync();
                    statusId = setorSelect4.GetGuid(0);
                }

                await connection4.CloseAsync();
            }

            using var commandSelectPatrimonio = new NpgsqlCommand(querySelectEquipamento, connection3);
            var quipamentoSelect = await commandSelectPatrimonio.ExecuteReaderAsync();
            Guid? equipamentoId = null;
            if (quipamentoSelect.HasRows)
            {
                await quipamentoSelect.ReadAsync();
                equipamentoId = quipamentoSelect.GetGuid(0);
            }
            else
            {
                using var connection5 = new NpgsqlConnection(conn);
                var querySelect5 = $"select id from \"equipments\" where id = '6775e3a1-9c94-4a27-a527-0e18a13566d4' order by dtcreation desc LIMIT 1";
                await connection5.OpenAsync();
                using var commandSelect5 = new NpgsqlCommand(querySelect5, connection5);
                var setorSelect5 = await commandSelect5.ExecuteReaderAsync();

                if (setorSelect5.HasRows)
                {
                    await setorSelect5.ReadAsync();
                    equipamentoId = setorSelect5.GetGuid(0);
                }

                await connection5.CloseAsync();
            }

            var query = $"INSERT INTO public.os (id, \"statusOsId\", \"equipmentId\", failure, \"internalCall\", \"isAntenna\", \"isBattery\", \"isCover\", \"isCapa\", \"numberOs\", \"numberNF\", \"data\", sdcv, dtcreation, \"cost\") ";
            query += $"VALUES(uuid_generate_v4(), {(statusId == null ? "null" : $"'{statusId}'")}, {(equipamentoId == null ? "null" : $"'{equipamentoId}'")}, '{falha}', '{chamado}', false, false, false, false, '{os}', '{nf}', '{data}', '{sdcv}', CURRENT_TIMESTAMP(6), {valor.Replace(",",".")});";

            using var command = new NpgsqlCommand(query, connection2);
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            await connection2.CloseAsync();
            await connection3.CloseAsync();
        }

    }
}
