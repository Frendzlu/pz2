namespace lab07;

using System;
using System.IO;
using System.Security.Cryptography;

class RSASignVerify
{
    public static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Użycie: <plik_danych> <plik_podpisu>");
            return;
        }

        var dataFile = args[0];
        var signatureFile = args[1];

        if (!File.Exists("private_key.xml") || !File.Exists("public_key.xml"))
        {
            Console.WriteLine("Błąd: Brakuje plików kluczy RSA. Wygeneruj je najpierw.");
            return;
        }

        var data = File.ReadAllBytes(dataFile);

        if (File.Exists(signatureFile))
        {
            var signature = File.ReadAllBytes(signatureFile);
            using var rsa = RSA.Create();
            rsa.FromXmlString(File.ReadAllText("public_key.xml"));
            var verified = rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            Console.WriteLine(verified ? "Podpis jest prawidłowy." : "Podpis jest NIEPRAWIDŁOWY.");
        }
        else
        {
            using var rsa = RSA.Create();
            rsa.FromXmlString(File.ReadAllText("private_key.xml"));
            var signature = rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            File.WriteAllBytes(signatureFile, signature);
            Console.WriteLine("Podpis został zapisany.");
        }
    }
}
