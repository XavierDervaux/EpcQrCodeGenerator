using EpcQrCodeGenerator.Models;

namespace EpcQrCodeGenerator;

/// <summary>
/// https://www.europeanpaymentscouncil.eu/sites/default/files/KB/files/EPC069-12%20v2.1%20Quick%20Response%20Code%20-%20Guidelines%20to%20Enable%20the%20Data%20Capture%20for%20the%20Initiation%20of%20a%20SCT.pdf
/// </summary>
public class EpcQrCodeData
{
    public string ServiceTag { get; set; }
    public EpcVersion Version { get; set; }
    public EpcEncoding CharacterSet { get; set; } 
    public string IdentificationCode { get; set; }
    public string BeneficiaryBic { get; set; }
    public string BeneficiaryName { get; set; }
    public string BeneficiaryIban { get; set; }
    public decimal CreditAmount { get; set; }
    public string PurposeOfCreditTransfer { get; set; }
    public string RemittanceInformationStructured { get; set; }
    public string RemittanceInformationUnstructured { get; set; }
    public string BeneficiaryToOriginatorInformation { get; set; }
} 
