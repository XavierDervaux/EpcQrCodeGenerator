namespace EpcQrCodeGenerator.Models;

public class EpcVersion
{
    private readonly string _value;

    private EpcVersion(string value)
    {
        _value = value;
    }

    public static readonly EpcVersion V1 = new("001");
    public static readonly EpcVersion V2 = new("002");

    public override string ToString()
    {
        return _value;
    }
}
