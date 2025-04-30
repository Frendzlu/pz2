namespace lab07;

using System;
using System.IO;
using System.Security.Cryptography;

class RSAEncryption
{
    public static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Błąd: Nie podano typu operacji (0 - generowanie kluczy, 1 - szyfrowanie, 2 - deszyfrowanie).");
            return;
        }

        var operationType = int.Parse(args[0]);

        switch (operationType)
        {
            case 0:
                if (args.Length < 3)
                {
                    Console.WriteLine("Błąd: Brak wymaganych parametrów. Użycie: 0 <ścieżka_pliku_klucza_publicznego> <ścieżka_pliku_klucza_prywatnego>");
                    return;
                }
                GenerateKeys(args[1], args[2]);
                break;
            case 1:
                if (args.Length < 4)
                {
                    Console.WriteLine("Błąd: Brak wymaganych parametrów. Użycie: 1 <plik_wejściowy> <plik_wyjściowy> <plik_klucza_publicznego>");
                    return;
                }
                EncryptFile(args[1], args[2], args[3]);
                break;
            case 2:
                if (args.Length < 4)
                {
                    Console.WriteLine("Błąd: Brak wymaganych parametrów. Użycie: 2 <plik_wejściowy> <plik_wyjściowy> <plik_klucza_prywatnego>");
                    return;
                }
                DecryptFile(args[1], args[2], args[3]);
                break;
            default:
                Console.WriteLine("Błąd: Nieznany typ operacji.");
                break;
        }
    }

    static void GenerateKeys(string publicKey, string privateKey)
    {
        using (var rsa = RSA.Create(2048))
        {
            File.WriteAllText(publicKey, rsa.ToXmlString(false));
            File.WriteAllText(privateKey,rsa.ToXmlString(true));
        }
        Console.WriteLine("Klucze RSA zostały wygenerowane i zapisane.");
    }

    static void EncryptFile(string inputFile, string outputFile, string publicKeyFile)
    {
        try
        {
            var publicKey = File.ReadAllText(publicKeyFile);
            using (var rsa = RSA.Create())
            {
                rsa.FromXmlString(publicKey);
                var dataToEncrypt = File.ReadAllBytes(inputFile);
                var encryptedData = rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.OaepSHA256);
                File.WriteAllBytes(outputFile, encryptedData);
            }
            Console.WriteLine("Plik został zaszyfrowany.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas szyfrowania: {ex.Message}");
        }
    }

    static void DecryptFile(string inputFile, string outputFile, string privateKeyFile)
    {
        try
        {
            var privateKey = File.ReadAllText(privateKeyFile);
            using (var rsa = RSA.Create())
            {
                rsa.FromXmlString(privateKey);
                var dataToDecrypt = File.ReadAllBytes(inputFile);
                var decryptedData = rsa.Decrypt(dataToDecrypt, RSAEncryptionPadding.OaepSHA256);
                File.WriteAllBytes(outputFile, decryptedData);
            }
            Console.WriteLine("Plik został odszyfrowany.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas deszyfrowania: {ex.Message}");
        }
    }
}
