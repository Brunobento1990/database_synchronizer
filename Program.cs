try
{
    //var menu = new Menu();
    //await menu.Init();
    //var service = new SincronizarEquipamentosService();
    //await service.Sincronizar();
    var service = new SincronizarOsService();
    await service.Sincronizar();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
	throw;
}