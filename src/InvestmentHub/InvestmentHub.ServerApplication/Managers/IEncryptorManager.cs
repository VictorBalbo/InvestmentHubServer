namespace InvestmentHub.ServerApplication.Managers
{
    public interface IEncryptorManager
    {
        public string Encrypt(string toEncrypt, string password);

        public string Decrypt(string toDecrypt, string password);
    }
}
