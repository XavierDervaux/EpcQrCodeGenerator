using System;

namespace EpcQrCodeGenerator;
/// <summary>
/// A basic QR-Code generator for EPC Data Payloads
/// </summary>
public class EpcQrCodeGenerator
{
    public EpcQrCodeData _epcQrCodeData { get; set; }

    public EpcQrCodeGenerator(EpcQrCodeData epcQrCodeData)
    {
        _epcQrCodeData = epcQrCodeData;
    }

    public void SaveCodeToDisk(string path)
    {
        throw new NotImplementedException();
    }
}
