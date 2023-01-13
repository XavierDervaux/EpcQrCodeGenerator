# EPC QR-Code Generator

This project is a simple, light solution to generate EPC QR-Codes.
These are used in order to initiate a SEPA money transfer while pre-filling 
most input fields, effectively reducing the risk of manual input error while 
being highly convenient to use.

## How to use the library

If you just want to try the library or just generate one single EPC QR-Code, the 
repository contains a console app that can be used to experiment with the generator.

To use the library in your own application, first install the latest version of the 
package [from nuget.org here.](https://www.nuget.org/packages/XD.EpcQrCodeGenerator/)
Then the library can be used with these few lines of code :
```
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
```
Using the `SaveAsSvg()` method will trigger a validation of the values encoded 
in the data object. If the values are invalid, a detailed exception will be triggered.

The `SaveAsSvg()` method uses an external QR-Code generator to render the actual image.
If you want to use your own generator you can get the payload used to generate 
the QR-Code by using the following method :
``` 
string encodedPayload = epcData.GeneratePayload();
```
Calling `epcData.GeneratePayload()` will also trigger a validation of the provided
data, raising an exception similarly if an error is detected. You can then use the
`encodedPayload` string with your chosen QR-Code generator.

## Notes
 * EPC QR-Codes are also sometimes refered to as GiroCodes, the German name. 
 These are the same thing.
 * I don't recommend using this project for professional use, as I provide no 
 support over it. If you are looking for a reliable solution for business use, 
 I recommend you look at [QRCoder](https://github.com/codebude/QRCoder).
 * This project relies on [QrCodeGenerator](https://github.com/manuelbl/QrCodeGenerator) for QR-Codes generation. 
However, any QR-Code generator can be used in it's place with the generated payload.


## See also
 * [The official EPC instructions for EPC QR-Codes generation](https://www.europeanpaymentscouncil.eu/sites/default/files/KB/files/EPC069-12%20v2.1%20Quick%20Response%20Code%20-%20Guidelines%20to%20Enable%20the%20Data%20Capture%20for%20the%20Initiation%20of%20a%20SCT.pdf)
