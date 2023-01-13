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


    /// <summary>
    /// Converts the current EPC QR-Code Data object into a valid string payload that can be used with any generator to create a valid EPC QR-Code.
    /// </summary>
    /// <returns>A valid EPC QR-Code payload.</returns>
    /// <exception cref="InvalidDataException">If the provided EPC QR-Code data was invalid and could not be parsed.</exception>
    public string GeneratePayload()
    {
        try
        {
            TryValidateData();
        }
        catch (Exception ex)
        {
            throw new InvalidDataException("The provided EPC QR-Code data was invalid and could not be parsed.", ex);
        }

        var res = string.Empty;
        const string lf = "\n";

        res += ServiceTag + lf;
        res += Version + lf;
        res += (int)CharacterSet + lf;
        res += IdentificationCode + lf;
        res += BeneficiaryBic + lf;
        res += BeneficiaryName + lf;
        res += BeneficiaryIban + lf;
        res += "EUR" + CreditAmount.ToString("0.00", CultureInfo.InvariantCulture) + lf;
        res += PurposeOfCreditTransfer + lf;
        res += RemittanceInformationStructured + lf;
        res += RemittanceInformationUnstructured + lf;
        res += BeneficiaryToOriginatorInformation + lf;

        return EncodeString(res, CharacterSet);
    }


    /// <summary>
    /// Validates the EPC QR-Code data.
    /// </summary>
    /// <exception cref="InvalidDataException">Throws a detailed exception if validation was unsuccessful.</exception>
    /// <returns>True if validation was successful.</returns>
    private void TryValidateData()
    {
        if (ServiceTag != "BCD") throw new InvalidDataException("ServiceTag cannot have a value different than \"BCD\".");

        if (Version is null) throw new InvalidDataException("Version needs to be defined.");

        if (CharacterSet is default(EpcEncoding)) throw new InvalidDataException("CharacterSet is mandatory.");

        if (IdentificationCode != "SCT") throw new InvalidDataException("IdentificationCode cannot have a value different than \"SCT\".");

        var beneficiaryBicIsNullOrEmpty = string.IsNullOrWhiteSpace(BeneficiaryBic);
        if (beneficiaryBicIsNullOrEmpty && Version == EpcVersion.V1) throw new InvalidDataException("BeneficiaryBic is mandatory when using Version 1.");
        if (!beneficiaryBicIsNullOrEmpty && BeneficiaryBic.Length < 8) throw new InvalidDataException("BeneficiaryBic cannot be shorter than 8 characters when defined.");
        if (!beneficiaryBicIsNullOrEmpty && BeneficiaryBic.Length > 11) throw new InvalidDataException("BeneficiaryBic cannot be longer than 11 characters.");
        if (!beneficiaryBicIsNullOrEmpty && !IsValidBic(BeneficiaryBic)) throw new InvalidDataException("BeneficiaryBic does not have a valid BIC format.");

        if (string.IsNullOrWhiteSpace(BeneficiaryName)) throw new InvalidDataException("BeneficiaryName is mandatory.");
        if (BeneficiaryName.Length > 70) throw new InvalidDataException("BeneficiaryName cannot be longer than 70 characters.");

        if (string.IsNullOrWhiteSpace(BeneficiaryIban)) throw new InvalidDataException("BeneficiaryIban is mandatory.");
        if (BeneficiaryIban.Length > 34) throw new InvalidDataException("BeneficiaryIban cannot be longer than 34 characters.");
        if (!IsValidIban(BeneficiaryIban)) throw new InvalidDataException("BeneficiaryIban does not have a valid IBAN format.");

        if (CreditAmount is default(decimal)) throw new InvalidDataException("CreditAmount is mandatory.");
        if (CreditAmount < 0.01m) throw new InvalidDataException("CreditAmount cannot be inferior to 0,01.");
        if (CreditAmount > 999999999.99m) throw new InvalidDataException("CreditAmount cannot be superior to 999999999,99.");

        if (PurposeOfCreditTransfer?.Length > 4) throw new InvalidDataException("PurposeOfCreditTransfer cannot be longer than 4 characters.");

        if (!string.IsNullOrWhiteSpace(RemittanceInformationStructured) && !string.IsNullOrWhiteSpace(RemittanceInformationUnstructured)) throw new InvalidDataException("Only one type of remittance information can be used per data set.");
        if (RemittanceInformationStructured?.Length > 35) throw new InvalidDataException("RemittanceInformationStructured cannot be longer than 35 characters.");
        if (RemittanceInformationUnstructured?.Length > 140) throw new InvalidDataException("RemittanceInformationUnstructured cannot be longer than 140 characters.");

        if (BeneficiaryToOriginatorInformation?.Length > 70) throw new InvalidDataException("BeneficiaryToOriginatorInformation cannot be longer than 70 characters.");
    }

    /// <summary>
    /// Validates the structure of a BIC code.
    /// Does not check if the BIN code actually exists.
    /// </summary>
    /// <param name="bic">BIC string to validate.</param>
    /// <returns>true if valid, false if invalid.</returns>
    private bool IsValidBic(string bic)
    {
        var res = false;

        if (bic is not null)
        {
            res = Regex.IsMatch(bic, @"^([a-zA-Z]{4}[a-zA-Z]{2}[a-zA-Z0-9]{2}([a-zA-Z0-9]{3})?)$");
        }

        return res;
    }


    /// <summary>
    /// Validates the structure of an IBAN code.
    /// Does not check if the IBAN code actually exists.
    /// </summary>
    /// <param name="iban">IBAN string to validate.</param>
    /// <returns>true if valid, false if invalid.</returns>
    private bool IsValidIban(string iban)
    {
        var res = false;

        if (iban is not null)
        {
            res = Regex.IsMatch(iban, @"^[a-zA-Z]{2}[0-9]{2}([a-zA-Z0-9]?){16,30}$");
        }

        return res;
    }

    /// <summary>
    /// Encodes a string in the provided character set.
    /// Only character sets compatible with EPC specifications are allowed.
    /// </summary>
    /// <param name="toEncode">String to encode.</param>
    /// <param name="characterSet">EPC allowed character set.</param>
    /// <returns>String encoded with the provided character set.</returns>
    /// <exception cref="NotImplementedException">Throws when providing an unsupported character set.</exception>
    private string EncodeString(string toEncode, EpcEncoding characterSet)
    {
        var requestedCharSet = characterSet switch
        {
            EpcEncoding.Utf8 => "UTF-8",
            EpcEncoding.Iso88591 => "ISO-8859-1",
            EpcEncoding.Iso88592 => "ISO-8859-2",
            EpcEncoding.Iso88594 => "ISO-8859-4",
            EpcEncoding.Iso88595 => "ISO-8859-5",
            EpcEncoding.Iso88597 => "ISO-8859-7",
            EpcEncoding.Iso885910 => "ISO-8859-10",
            EpcEncoding.Iso885915 => "ISO-8859-15",
            _ => throw new NotImplementedException("The requested encoding is not implemented."),
        };
        var charset = Encoding.GetEncoding(requestedCharSet);
        var encodedBytes = charset.GetBytes(toEncode);

        return charset.GetString(encodedBytes);
    }
}