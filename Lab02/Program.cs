// See https://aka.ms/new-console-template for more information

using Lab02;

var mf = new OsobaFizyczna("Mateusz", "Francik", "Teodor", "01234567890", null);
var an = new OsobaFizyczna("Adam", "Niezgoda", "Eliasz", "18462857281", null);
var pr = new OsobaFizyczna("Paweł", "Reiner", "Tomasz", null, "44");

var op1 = new OsobaPrawna("ACME", "Gorzów, Aleje Jarocińskie 5");

var r1 = new RachunekBankowy("1424-2536", 1000, false, new List<PosiadaczRachunku> { mf });
var r2 = new RachunekBankowy("4395-2435", 0, true, new List<PosiadaczRachunku> { an, pr });

try
{
    var _ = new OsobaFizyczna("a", "e", "u", null, null);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

try
{
    var _ = new OsobaFizyczna("a", "e", "u", "10132", null);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

try
{
    var _ = new RachunekBankowy("a", 0, true, null);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

try
{
    r1 = r1 - mf;
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

try
{
    r2 = r2 - mf;
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

try
{
    r2 = r2 + pr;
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

try
{
    RachunekBankowy.DokonajTransakcji(r1, r2, 1001, "Wywołanie błędu");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

try
{
    RachunekBankowy.DokonajTransakcji(null, null, 1001, "Wywołanie błędu");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

try
{
    RachunekBankowy.DokonajTransakcji(r1, r2, -1001, "Wywołanie błędu");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

try
{
    var _ = new Transakcja(null, r2, 1, "");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

try
{
    var _ = new Transakcja(r1, null, 1, "");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

Console.WriteLine("\nKoniec testów błędów \n");

r1 = r1 + op1;
RachunekBankowy.DokonajTransakcji(r2, r1, (decimal) 10.24, "Wywołanie debetu");
RachunekBankowy.DokonajTransakcji(r1, r2, 140, "Zwrot środków");
RachunekBankowy.DokonajTransakcji(null, r1, (decimal) 5367.99, "Wpłata");
RachunekBankowy.DokonajTransakcji(r2, null, 100, "Wypłata");

Console.WriteLine(r1);
Console.WriteLine(r2);
Console.WriteLine("Przeniesienie podmiotu prawnego");
r1 = r1 - op1;
r2 = r2 + op1;
Console.WriteLine(r1);
Console.WriteLine(r2);