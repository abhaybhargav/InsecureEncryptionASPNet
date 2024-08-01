using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace InsecureEncryptionDemo.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly byte[] _key;
        private readonly bool _isSecureMode;

        public EncryptionService(IConfiguration configuration)
        {
            _isSecureMode = configuration.GetValue<bool>("UseSecureEncryption");
            _key = _isSecureMode ? GenerateSecureKey() : Encoding.UTF8.GetBytes("insecurekey12345");
        }

        public bool IsSecureMode => _isSecureMode;

        public string Encrypt(string plaintext)
        {
            if (_isSecureMode)
            {
                return SecureEncrypt(plaintext);
            }
            else
            {
                return InsecureEncrypt(plaintext);
            }
        }

        public string Decrypt(string ciphertext)
        {
            if (_isSecureMode)
            {
                return SecureDecrypt(ciphertext);
            }
            else
            {
                return InsecureDecrypt(ciphertext);
            }
        }

        private string InsecureEncrypt(string plaintext)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = _key;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plaintext);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        private string InsecureDecrypt(string ciphertext)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = _key;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(Convert.FromBase64String(ciphertext)))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        private string SecureEncrypt(string plaintext)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CFB;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = _key;
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plaintext);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        private string SecureDecrypt(string ciphertext)
        {
            byte[] ciphertextBytes = Convert.FromBase64String(ciphertext);

            using (var aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CFB;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = _key;

                byte[] iv = new byte[aes.BlockSize / 8];
                Array.Copy(ciphertextBytes, 0, iv, 0, iv.Length);

                using (var decryptor = aes.CreateDecryptor(aes.Key, iv))
                using (var ms = new MemoryStream(ciphertextBytes, iv.Length, ciphertextBytes.Length - iv.Length))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        private byte[] GenerateSecureKey()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[32]; // 256 bits
                rng.GetBytes(key);
                return key;
            }
        }
    }
}