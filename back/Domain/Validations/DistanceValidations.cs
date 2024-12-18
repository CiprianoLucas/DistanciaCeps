using System.Text.RegularExpressions;
namespace Back.Domain.Validations;
public static class CepValidator
{
    public static string Formate(string? cep)
    {
        if (cep == null) return "";
        return Regex.Replace(cep, @"\D", "");
    }
}