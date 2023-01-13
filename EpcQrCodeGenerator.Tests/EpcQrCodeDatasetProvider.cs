namespace EpcQrCodeGenerator.Tests;

public static class EpcQrCodeDatasetProvider
{
    public static EpcQrCodeData GetValidTestDataForV1()
    {
        return new EpcQrCodeData
        {
            ServiceTag = "BCD",
            Version = EpcVersion.V1,
            CharacterSet = EpcEncoding.Iso88591,
            IdentificationCode = "SCT",
            BeneficiaryBic = "BHBLDEHHXXX",
            BeneficiaryName = "Franz Mustermänn",
            BeneficiaryIban = "DE71110220330123456789",
            CreditAmount = 12.3m,
            PurposeOfCreditTransfer = "GDDS",
            RemittanceInformationStructured = "RF18539007547034",
            RemittanceInformationUnstructured = "",
            BeneficiaryToOriginatorInformation = ""
        };
    }

    public static EpcQrCodeData GetValidTestDataForV2()
    {
        return new EpcQrCodeData
        {
            ServiceTag = "BCD",
            Version = EpcVersion.V2,
            CharacterSet = EpcEncoding.Iso88591,
            IdentificationCode = "SCT",
            BeneficiaryBic = "",
            BeneficiaryName = "François D'Alsace S.A.",
            BeneficiaryIban = "FR1420041010050500013M02606",
            CreditAmount = 12.3m,
            PurposeOfCreditTransfer = "",
            RemittanceInformationStructured = "",
            RemittanceInformationUnstructured = "Client:Marie Louise La Lune",
            BeneficiaryToOriginatorInformation = ""
        };
    }

    public static EpcQrCodeData GetInvalidTestData()
    {
        //BIC is mandatory in V1, returning a V1 data set without one makes it invalid.
        return new EpcQrCodeData
        {
            ServiceTag = "BCD",
            Version = EpcVersion.V1,
            CharacterSet = EpcEncoding.Iso88591,
            IdentificationCode = "SCT",
            BeneficiaryBic = "",
            BeneficiaryName = "François D'Alsace S.A.",
            BeneficiaryIban = "FR1420041010050500013M02606",
            CreditAmount = 12.3m,
            PurposeOfCreditTransfer = "",
            RemittanceInformationStructured = "",
            RemittanceInformationUnstructured = "Client:Marie Louise La Lune",
            BeneficiaryToOriginatorInformation = ""
        };
    }
}
