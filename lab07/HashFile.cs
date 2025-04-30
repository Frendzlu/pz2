namespace lab07;

using System;
using System.IO;
using System.Security.Cryptography;

class HashFile
{
    public static void Main(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Błąd: Brak wymaganych parametrów. Użycie: <plik_wejściowy> <plik_hash> <algorytm>");
            return;
        }

        var inputFile = args[0];
        var hashFile = args[1];
        var algorithm = args[2].ToUpper();

        if (!File.Exists(inputFile))
        {
            Console.WriteLine("Błąd: Plik wejściowy nie istnieje.");
            return;
        }

        if (File.Exists(hashFile))
        {
            VerifyHash(inputFile, hashFile, algorithm);
        }
        else
        {
            GenerateHash(inputFile, hashFile, algorithm);
        }
    }

    static void GenerateHash(string inputFile, string hashFile, string algorithm)
    {
        try
        {
            using (var hashAlg = GetHashAlgorithm(algorithm))
            {
                var fileBytes = File.ReadAllBytes(inputFile);
                var hashBytes = hashAlg.ComputeHash(fileBytes);
                File.WriteAllBytes(hashFile, hashBytes);
            }
            Console.WriteLine($"Hash ({algorithm}) został zapisany do pliku.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas generowania hasha: {ex.Message}");
        }
    }

    static void VerifyHash(string inputFile, string hashFile, string algorithm)
    {
        try
        {
            var expectedHash = File.ReadAllBytes(hashFile);
            using var hashAlg = GetHashAlgorithm(algorithm);
            var fileBytes = File.ReadAllBytes(inputFile);
            var computedHash = hashAlg.ComputeHash(fileBytes);
            Console.WriteLine(CompareHashes(expectedHash, computedHash) ? "Hash jest zgodny." : "Hash jest niezgodny.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas weryfikacji hasha: {ex.Message}");
        }
    }

    static HashAlgorithm GetHashAlgorithm(string algorithm)
    {
        return algorithm switch
        {
            "SHA256" => SHA256.Create(),
            "SHA512" => SHA512.Create(),
            "MD5" => MD5.Create(),
            _ => throw new ArgumentException("Nieznany algorytm hashujący.")
        };
    }

    static bool CompareHashes(byte[] hash1, byte[] hash2)
    {
        if (hash1.Length != hash2.Length)
            return false;

        return !hash1.Where((t, i) => t != hash2[i]).Any();
    }
}
