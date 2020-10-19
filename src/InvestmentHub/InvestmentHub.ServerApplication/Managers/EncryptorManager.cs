using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace InvestmentHub.ServerApplication.Managers
{
    internal class EncryptorManager : IEncryptorManager
    {
        private const int AesBlockByteSize = 128 / 8;
        private const int PasswordSaltByteSize = 128 / 8;
        private const int PasswordByteSize = 256 / 8;
        private const int PasswordIterationCount = 100_000;

        private readonly RandomNumberGenerator _randomNumberGenerator;
        private readonly Encoding StringEncoding = Encoding.Unicode;

        public EncryptorManager()
        {
            _randomNumberGenerator = RandomNumberGenerator.Create();
        }

        public string Encrypt(string toEncrypt, string password)
        {
            var keySalt = GenerateRandomBytes(PasswordSaltByteSize);
            var key = GetKey(password, keySalt);
            var iv = GenerateRandomBytes(AesBlockByteSize);

            using var aes = CreateAes();
            using var encryptor = aes.CreateEncryptor(key, iv);

            var plainText = StringEncoding.GetBytes(toEncrypt);
            var cipherText = encryptor
                .TransformFinalBlock(plainText, 0, plainText.Length);

            var result = MergeArrays(keySalt, iv, cipherText);
            return Convert.ToBase64String(result);
        }

        public string Decrypt(string toDecrypt, string password)
        {
            var encryptedData = Convert.FromBase64String(toDecrypt);

            var keySalt = encryptedData
                .Take(PasswordSaltByteSize)
                .ToArray();
            var key = GetKey(password, keySalt);
            var iv = encryptedData
                .Skip(PasswordSaltByteSize)
                .Take(AesBlockByteSize)
                .ToArray();
            var cipherText = encryptedData
                .Skip(PasswordSaltByteSize + AesBlockByteSize)
                .ToArray();

            using var aes = CreateAes();
            using var encryptor = aes.CreateDecryptor(key, iv);

            var decryptedBytes = encryptor
                .TransformFinalBlock(cipherText, 0, cipherText.Length);
            return StringEncoding.GetString(decryptedBytes);
        }

        private static Aes CreateAes()
        {
            var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            return aes;
        }

        private byte[] GetKey(string password, byte[] passwordSalt)
        {
            var keyBytes = StringEncoding.GetBytes(password);

            using var derivator = new Rfc2898DeriveBytes(
                keyBytes, passwordSalt,
                PasswordIterationCount, HashAlgorithmName.SHA256);
            return derivator.GetBytes(PasswordByteSize);
        }

        private byte[] GenerateRandomBytes(int numberOfBytes)
        {
            var randomBytes = new byte[numberOfBytes];
            _randomNumberGenerator.GetBytes(randomBytes);
            return randomBytes;
        }

        private static byte[] MergeArrays(params byte[][] arrays)
        {
            var merged = new byte[arrays.Sum(a => a.Length)];
            var mergeIndex = 0;
            for (int i = 0; i < arrays.GetLength(0); i++)
            {
                arrays[i].CopyTo(merged, mergeIndex);
                mergeIndex += arrays[i].Length;
            }

            return merged;
        }
    }
}
