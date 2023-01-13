var folderName = Path.Combine(Directory.GetCurrentDirectory(), "Generated");
var filePath = Path.Combine(folderName, "QuickRun.svg");
if (!Directory.Exists(folderName)) { Directory.CreateDirectory(folderName); }
if (File.Exists(filePath)) { File.Delete(filePath); }




var epcData = new EpcQrCodeData
{
    //Set your custom values here :
    BeneficiaryName = "WIKIMEDIA FOUNDATION INC",
    BeneficiaryIban = "GB12CITI18500818796270",
    BeneficiaryBic = "CITIGB2L", //Optional
    CreditAmount = 1.00m,

    // You can only use one RemitanceInformation property, leave the other empty.
    RemittanceInformationStructured = "",
    RemittanceInformationUnstructured = "Thank you for your hard work."
};




var epcGenerator = new EpcQrCodeGenerator.EpcQrCodeGenerator(epcData);
epcGenerator.SaveAsSvg(filePath);
Process.Start("explorer.exe", filePath);