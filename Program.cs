try
{
    var menu = new Menu();
    await menu.Init();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
	throw;
}