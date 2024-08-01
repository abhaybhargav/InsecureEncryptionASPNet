namespace InsecureEncryptionDemo.Services
{
    public interface IEncryptionService
    {
        bool IsSecureMode { get; }
        string Encrypt(string plaintext);
        string Decrypt(string ciphertext);
    }
}