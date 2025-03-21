namespace Lab02;

public class Transakcja
{
    private RachunekBankowy RachunekZrodlowy { get; set; }
    private RachunekBankowy RachunekDocelowy { get; set; }
    private decimal Kwota {get; set;}
    private string Opis {get; set;}

    public Transakcja(RachunekBankowy? rachunekZrodlowy, RachunekBankowy? rachunekDocelowy, decimal kwota, string opis)
    {
        RachunekZrodlowy = rachunekZrodlowy ?? throw new Exception("Nieprawidłowy rachunek docelowy");
        RachunekDocelowy = rachunekDocelowy ?? throw new Exception("Nieprawidłowy rachunek docelowy");
        Kwota = kwota;
        Opis = opis;
    }

    public override string ToString()
    {
        return $"Dane transakcji:\n\tZ: {RachunekZrodlowy.Numer}\n\tDo: {RachunekDocelowy.Numer}\n\tKwota: {Kwota}\n\t{Opis}";
    }
}