namespace EpcQrCodeGenerator.Tests;

public class EpcQrCodeDataUnit
{
    //Used to test validation rules checking max amount of characters in a string.
    private const string _random140PlusCharsString = "sKKoxxxLmBLDSowocGOhTLPCZzmMKdjUPwxjkAALKcNXomXWweRXeZjsi VWL IxDlYTIfBVpgcmC gojay RPp iwpaZQ UQQHvUrBeurCmvXCkLOXZvegvjXzjlgFTgSbSNeVkobUln";
    private const string _random70PlusCharsString = "eEPERdzQzIh pALYDbDouayOunoNwxonhhCzsfvOXtoMctXiOYONVoOYHQruRExWt ouhJl";
    private const string _random34PlusCharsString = "rVshbdYLHHijurxAFxFaoYBmANYApqEVySf";
    private const string _random35PlusCharsString = "rVshbdYLHHiju rxAFxFaoYBmANYApqEVySf";
    private const string _random4PlusCharsString = "jFk4Y";

    [Theory]
    [InlineData("GEBABEBB", true)] //Valid.
    [InlineData("GEBABEBBXXX", true)] //Also valid, the last three chars are free and can be present or not.
    [InlineData("GEBABEBB321", true)] //Also valid, the last three chars can also be number.
    [InlineData("BBRU BE BB", false)] //No spaces allowed.
    [InlineData("Hello", false)] //Not a BIC.
    [InlineData("", false)] //Empty string.
    [InlineData(null, false)] //Null.
    public void BicValidation(string bic, bool expectedResult)
    {
        var epcDataObject = new EpcQrCodeData();
        var parameters = new object[] { bic };
        var methodInfo = typeof(EpcQrCodeData).GetMethod("IsValidBic", BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo is null) throw new ApplicationException("Could not find the private method by reflection. Did the method name change ?");

        var result = methodInfo.Invoke(epcDataObject, parameters);

        Assert.True((bool)result == expectedResult);
    }

    [Theory]
    [InlineData("BE68539007547034", true)] //16 chars valid IBAN.
    [InlineData("JO94CBJO0010000000000131000302", true)] //30 chars valid IBAN.
    [InlineData("BE68 5390 0754 7034", false)] //No spaces allowed.
    [InlineData("How are you today?", false)] //Not an IBAN.
    [InlineData("", false)] //Empty string.
    [InlineData(null, false)] //Null.
    public void IbanValidation(string iban, bool expectedResult)
    {
        var epcDataObject = new EpcQrCodeData();
        var parameters = new object[] { iban };
        var methodInfo = typeof(EpcQrCodeData).GetMethod("IsValidIban", BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo is null) throw new ApplicationException("Could not find the private method by reflection. Did the method name change ?");

        var result = methodInfo.Invoke(epcDataObject, parameters);

        Assert.True((bool)result == expectedResult);
    }

    [Theory]
    [InlineData(null, null, true, true)] //V1 - No field alteration, this covers most success cases.
    [InlineData(null, null, false, true)] //V2 - No field alteration, this covers most success cases.

    [InlineData("ServiceTag", "Anything that is not BCD", false, false)] //Only BCD is allowed.

    [InlineData("Version", null, false, false)] //Version can either be "001" or "002", success covered in first two cases.

    [InlineData("CharacterSet", default(EpcEncoding), false, false)] //Default value is 'None' and not allowed.

    [InlineData("IdentificationCode", "Anything that is not SCT", false, false)]//Only SCT is allowed.

    [InlineData("BeneficiaryBic", "GEBABEBB", false, true)] //BIC is optional in V2 but should work if used.
    [InlineData("BeneficiaryBic", null, true, false)] //BIC cannot be null in V1.
    [InlineData("BeneficiaryBic", "invalid", false, false)] //BIC cannot be less than 8 characters.
    [InlineData("BeneficiaryBic", "evenMoreInvalid", false, false)] //BIC cannot be greater than 11 characters.
    [InlineData("BeneficiaryBic", "correctLen", false, false)] //BIC must be in the correct format.

    [InlineData("BeneficiaryName", null, false, false)] //Name is mandatory.
    [InlineData("BeneficiaryName", _random70PlusCharsString, false, false)] //Name must be max 70 chars.

    [InlineData("BeneficiaryIban", null, false, false)] //IBAN is mandatory.
    [InlineData("BeneficiaryIban", _random34PlusCharsString, false, false)] //IBAN must be max 34 chars.
    [InlineData("BeneficiaryIban", "Visibly Invalid IBAN", false, false)] //IBAN must be in the correct format.

    [InlineData("CreditAmount", 0, false, false)] //CreditAmount cannot be 0.
    [InlineData("CreditAmount", -1, false, false)] //CreditAmount cannot be negative.
    [InlineData("CreditAmount", 1000000000, false, false)] //CreditAmount cannot be superior to 999999999,99.

    [InlineData("PurposeOfCreditTransfer", _random4PlusCharsString, false, false)] //PurposeOfCreditTransfer must be max 4 chars.

    [InlineData("RemittanceInformationStructured", _random35PlusCharsString, true, false)] //RemittanceInformationStructured must be max 35 chars.
    [InlineData("RemittanceInformationUnstructured", _random140PlusCharsString, false, false)] //RemittanceInformationUnstructured must be max 140 chars.
    [InlineData("RemittanceInformationUnstructured", "Populating both remittance type at the same time.", true, false)] //Only one type of remittance information is allowed. V1 dataset uses Structured by default. By filling Unstructured as well, both of them are populated and validation should fail..

    [InlineData("BeneficiaryToOriginatorInformation", _random70PlusCharsString, false, false)] //PurposeOfCreditTransfer must be max 70 chars.
    public void DataValidation(string fieldName, object fieldValue, bool UseV1InsteadofV2, bool expectedToSucceed)
    {
        var dataset = UseV1InsteadofV2 ? EpcQrCodeDatasetProvider.GetValidTestDataForV1() : EpcQrCodeDatasetProvider.GetValidTestDataForV2();

        if (fieldName is not null)
        {
            var propertydInfo = typeof(EpcQrCodeData).GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
            if (propertydInfo is null) throw new ApplicationException("Could not find the selected field by reflection. Did the property name change ?");

            propertydInfo.SetValue(dataset, Convert.ChangeType(fieldValue, propertydInfo.PropertyType));
        }

        var methodInfo = typeof(EpcQrCodeData).GetMethod("TryValidateData", BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo is null) throw new ApplicationException("Could not find the private method by reflection. Did the method name change ?");

        var exception = Record.Exception(() => methodInfo.Invoke(dataset, new object[0]));
        Assert.True(expectedToSucceed ? exception is null : exception is not null);
    }
}
