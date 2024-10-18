namespace Syncer.APIs.Models.Domain;

public class Emoji
{
    public required string Code { get; set; }
    public required string SortName { get; set; }

    public static Emoji Create(string code, string name)
    {
        var unidoce = char.ConvertToUtf32(code, 0);
        var hexadecimal = $"U+{unidoce:X4}";
        return new Emoji { Code = hexadecimal, SortName = name };
    }
}
