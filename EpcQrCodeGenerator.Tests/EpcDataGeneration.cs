namespace EpcQrCodeGenerator.Tests;

public class EpcDataGeneration
{
    [Fact]
    public void TheEpcDataIsGeneratedAndAFileIsCreated()
    {
        const string filepath = "code.svg";

        if (File.Exists(filepath)) { File.Delete(filepath); }

        var epcData = EpcQrCodeDatasetProvider.GetValidTestDataForV2();

        var epcGenerator = new EpcQrCodeGenerator(epcData);
        var isSaved = epcGenerator.SaveAsSvg(filepath);

        Assert.True(isSaved);
        Assert.True(File.Exists(filepath));
    }

    [Fact]
    public void InvalidInputIsGivenAndTheGenerationFails()
    {
        var epcData = EpcQrCodeDatasetProvider.GetInvalidTestData();
        var epcGenerator = new EpcQrCodeGenerator(epcData);

        Assert.Throws<InvalidDataException>(() => epcGenerator.SaveAsSvg("dummy"));
    }
}
