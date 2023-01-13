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

    /// <summary>
    /// Creates an SVG file containing the QR-Code generated from the data payload.
    /// </summary>
    /// <param name="filePath">The path of the file created. Assumed valid.</param>
    /// <returns>true in case of success.</returns>
    /// <exception cref="InvalidDataException">If the provided EPC QR-Code data was invalid and could not be parsed.</exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    public bool SaveAsSvg(string filePath)
    {
        var qrCode = QrCode.EncodeText(_epcQrCodeData.GeneratePayload(), QrCode.Ecc.Medium);
        var svg = qrCode.ToSvgString(1);
        File.WriteAllText(filePath, svg, Encoding.UTF8);

        return true;
    }
}

