namespace BLL.Helpers;

public static class ValidationHelper
{
    public static void ThrowIfNull(params (object? value, string paramName)[] parameters)
    {
        foreach (var (value, paramName) in parameters)
        {
            if (value is null) throw new ArgumentNullException(paramName);
        }
    }
}
