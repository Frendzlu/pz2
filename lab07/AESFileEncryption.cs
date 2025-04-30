namespace lab07;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class AESFileEncryption
{
    public static void Main(string[] args)
    {
        if (args.Length < 4)
        {
            Console.WriteLine("Użycie: <plik_we> <plik_wy> <hasło> <typ_operacji: 0=szyfruj, 1=odszyfruj>");
            return;
        }

        var inputFile = args[0];
        var outputFile = args[1];
        var password = args[2];
        var operation = int.Parse(args[3]);

        var salt = Encoding.UTF8.GetBytes("SólIPieprz2025");

        try
        {
            using var key = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            using var aes = Aes.Create();
            aes.Key = key.GetBytes(32);
            aes.IV = key.GetBytes(16);

            switch (operation)
            {
                case 0:
                {
                    var plaintext = File.ReadAllBytes(inputFile);
                    using var encryptor = aes.CreateEncryptor();
                    var cipher = encryptor.TransformFinalBlock(plaintext, 0, plaintext.Length);
                    File.WriteAllBytes(outputFile, cipher);
                    Console.WriteLine("Plik został zaszyfrowany.");

                    break;
                }
                case 1:
                {
                    var cipher = File.ReadAllBytes(inputFile);
                    using var decryptor = aes.CreateDecryptor();
                    var plain = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
                    File.WriteAllBytes(outputFile, plain);
                    Console.WriteLine("Plik został odszyfrowany.");

                    break;
                }
                default:
                    Console.WriteLine("Błąd: Nieznany typ operacji. Użyj 0 (szyfruj) lub 1 (odszyfruj).");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Błąd podczas przetwarzania: " + ex.Message);
        }
    }
}
