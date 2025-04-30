// See https://aka.ms/new-console-template for more information

using lab07;

using System;
using System.IO;
using System.Diagnostics;

const string inputFile = "../../../artifacts/dane.txt";
File.WriteAllText(inputFile, "Lorem ipsum dolor sit amet consectetur adipiscing elit");

Console.WriteLine("\n[1] Generowanie kluczy RSA...");
RSAEncryption.Main("0 ../../../artifacts/public_key.xml ../../../artifacts/private_key.xml ".Split(" "));

Console.WriteLine("\n[2] Szyfrowanie RSA...");
RSAEncryption.Main($"1 {inputFile} ../../../artifacts/dane_szyfr.bin ../../../artifacts/public_key.xml".Split(" "));

Console.WriteLine("\n[3] Deszyfrowanie RSA...");
RSAEncryption.Main("2 ../../../artifacts/dane_szyfr.bin ../../../artifacts/dane_odszyfr.txt ../../../artifacts/private_key.xml".Split(" "));

Console.WriteLine("\n[4] Podpisywanie pliku...");
RSASignVerify.Main($"{inputFile} ../../../artifacts/podpis.bin".Split(" "));

Console.WriteLine("\n[5] Weryfikacja podpisu...");
RSASignVerify.Main($"{inputFile} ../../../artifacts/podpis.bin".Split(" "));

Console.WriteLine("\n[6] Generowanie SHA256...");
HashFile.Main($"{inputFile} ../../../artifacts/hash.sha256 SHA256".Split(" "));

Console.WriteLine("\n[7] Weryfikacja SHA256...");
HashFile.Main($"{inputFile} ../../../artifacts/hash.sha256 SHA256".Split(" "));

Console.WriteLine("\n[8] Szyfrowanie AES...");
AESFileEncryption.Main($"{inputFile} ../../../artifacts/dane_aes.bin mojehaslo 0".Split(" "));

Console.WriteLine("\n[9] Deszyfrowanie AES...");
AESFileEncryption.Main("../../../artifacts/dane_aes.bin ../../../artifacts/dane_aes_odszyfr.txt mojehaslo 1".Split(" "));