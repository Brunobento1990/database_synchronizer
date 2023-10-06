namespace database_synchronizer;

public static class Validations
{
    public static void ValidateString(string? value)
    {
        if(string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
    }
}
