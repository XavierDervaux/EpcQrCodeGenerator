using System;

namespace EpcQrCodeGenerator
{
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
}