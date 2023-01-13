using EpcQrCodeGenerator.Models;

namespace EpcQrCodeGenerator;

/// <summary>
/// Stores the required data to generate an EPC QR-Code.
/// The total payload is limited to 331 bytes. Please note that the number of characters may be less than the numbers of bytes with UTF-8.
/// Based on the official definition found here : https://www.europeanpaymentscouncil.eu/sites/default/files/KB/files/EPC069-12%20v2.1%20Quick%20Response%20Code%20-%20Guidelines%20to%20Enable%20the%20Data%20Capture%20for%20the%20Initiation%20of%20a%20SCT.pdf
/// </summary>
public class EpcQrCodeData
{
    /// <summary>
    /// Service Tag. The value "BCD" is the only allowed value and must not be modified.
    /// Mandatory.
    /// </summary>
    public string ServiceTag { get; set; } = "BCD";

    /// <summary>
    /// The version of the EPC QR-Code. Latest by default.
    /// Mandatory.
    /// </summary>
    public EpcVersion Version { get; set; } = EpcVersion.V2;

    /// <summary>
    /// The encoding format of the QR-Code. Iso-8859-1 by default.
    /// Mandatory.
    /// </summary>
    public EpcEncoding CharacterSet { get; set; } = EpcEncoding.Iso88591;

    /// <summary>
    /// Identification Code. The value "SCT" is the only allowed value and must not be modified.
    /// Mandatory.
    /// </summary>
    public string IdentificationCode { get; set; } = "SCT";

    /// <summary>
    /// BIC of the Beneficiary.
    /// Minimum 8 characters when defined.
    /// Maximum 11 characters.
    /// Mandatory if using V1, Optional in V2.
    /// </summary>
    public string BeneficiaryBic { get; set; } = null;

    /// <summary>
    /// Name of the beneficiary.
    /// Maximum 70 characters.
    /// Mandatory.
    /// </summary>
    public string BeneficiaryName { get; set; }

    /// <summary>
    /// IBAN of the beneficiary.
    /// Maximum 34 characters.
    /// Mandatory.
    /// </summary>
    public string BeneficiaryIban { get; set; }

    /// <summary>
    /// Amount of the credit transfer. Only EUR is supported.
    /// Amount must be between 0,01 and 999999999,99.
    /// Value will be rounded to the second decimal.
    /// Maximum 12 characters.
    /// Mandatory.
    /// </summary>
    public decimal CreditAmount { get; set; }

    /// <summary>
    /// Purpose of the credit transfer.
    /// Maximum 4 characters.
    /// Optional.
    /// </summary>
    public string PurposeOfCreditTransfer { get; set; } = null;

    /// <summary>
    /// Remittance information (Structured).
    /// See ISO 11649 RF Creditor Reference.
    /// Maximum 35 characters.
    /// Optional. Only one remittance information field may be populated, either Structured or Unstructured but not both.
    /// </summary>
    public string RemittanceInformationStructured { get; set; } = null;

    /// <summary>
    /// Remittance information (Unstructured).
    /// Maximum 140 characters.
    /// Optional. Only one remittance information field may be populated, either Structured or Unstructured but not both.
    /// </summary>
    public string RemittanceInformationUnstructured { get; set; } = null;

    /// <summary>
    /// Beneficiary to originator information.
    /// Maximum 70 characters.
    /// Optional.
    /// </summary>
    public string BeneficiaryToOriginatorInformation { get; set; } = null;
}
