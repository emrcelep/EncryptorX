using System;
using System.IO;
using System.Security.Cryptography;

public class SecureFileEncryptor
{
    private static readonly byte[] salt = new byte[] { 0x26, 0xdc, 0x49, 0x16, 0x85, 0x7a, 0x2b, 0xb3 };

    public static void EncryptFile(string inputFile, string outputFile, string password)
    {
        using (var aesAlg = new AesCryptoServiceProvider())
        {
            aesAlg.KeySize = 256;
            var key = new Rfc2898DeriveBytes(password, salt, 10000);
            aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
            aesAlg.GenerateIV();
            File.WriteAllBytes(outputFile + ".iv", aesAlg.IV); // Store IV for decryption

            using (var fsIn = new FileStream(inputFile, FileMode.Open))
            using (var fsOut = new FileStream(outputFile, FileMode.Create))
            using (var cryptoStream = new CryptoStream(fsOut, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
            {
                fsIn.CopyTo(cryptoStream);
            }
        }
    }

    public static void DecryptFile(string inputFile, string outputFile, string password)
    {
        byte[] iv = File.ReadAllBytes(inputFile + ".iv");

        using (var aesAlg = new AesCryptoServiceProvider())
        {
            aesAlg.KeySize = 256;
            var key = new Rfc2898DeriveBytes(password, salt, 10000);
            aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
            aesAlg.IV = iv;

            using (var fsIn = new FileStream(inputFile, FileMode.Open))
            using (var fsOut = new FileStream(outputFile, FileMode.Create))
            using (var cryptoStream = new CryptoStream(fsIn, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
            {
                try
                {
                    cryptoStream.CopyTo(fsOut);
                }
                catch {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[!!!] You entered the wrong key");
                }

                
            }
        }
    }

}
