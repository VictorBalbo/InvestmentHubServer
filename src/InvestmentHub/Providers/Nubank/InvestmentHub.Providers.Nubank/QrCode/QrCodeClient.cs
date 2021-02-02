using System;
using QRCoder;

namespace InvestmentHub.Providers.Nubank.QrCode
{
    public class QrCodeClient : IQRCodeClient
    {
        public readonly string Code;
        private readonly QRCodeData _qrCodeData;

        public QrCodeClient(string code = null)
        {
            Code = code ?? Guid.NewGuid().ToString();

            var qrGenerator = new QRCodeGenerator();
            _qrCodeData = qrGenerator.CreateQrCode(Code, QRCodeGenerator.ECCLevel.Q);
        }

        public string GetQrCodeAsAscii()
        {
            var qrCode = new AsciiQRCode(_qrCodeData);
            return qrCode.GetGraphic(1, "  ", "██");
        }
    }
}
