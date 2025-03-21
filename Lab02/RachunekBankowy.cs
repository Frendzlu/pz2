namespace Lab02;

public class RachunekBankowy
{
    public string Numer {get;}
    private decimal StanRachunku {get; set; }
    private bool CzyDozwolonyDebet  {get; set;}
    private List<PosiadaczRachunku> PosiadaczeRachunku {get; set;}
    
    private List<Transakcja> Transakcje { get; set; } = new List<Transakcja>();

    public RachunekBankowy(string numer, decimal stanRachunku, bool czyDozwolonyDebet, List<PosiadaczRachunku>? posiadaczeRachunku)
    {
        if (posiadaczeRachunku == null || posiadaczeRachunku.Count == 0)
        {
            throw new Exception("Brak podanych posiadaczy rachunku");
        }
        Numer = numer;
        StanRachunku = stanRachunku;
        CzyDozwolonyDebet = czyDozwolonyDebet;
        PosiadaczeRachunku = posiadaczeRachunku;
    }
    
    public static void DokonajTransakcji(RachunekBankowy? rachunekZrodlowy, RachunekBankowy? rachunekDocelowy,
        decimal kwota, string opis)
    {
        if (kwota < 0)
        {
            throw new Exception("Kwota nie może być ujemna");
        }

        if (rachunekDocelowy == null && rachunekZrodlowy == null)
        {
            throw new Exception("Nieprawidłowy rachunek docelowy i źródłowy");
        }

        switch (rachunekZrodlowy)
        {
            case { CzyDozwolonyDebet: false } when kwota > rachunekZrodlowy.StanRachunku:
                throw new Exception("Brak środków na koncie i zgody na debet");
            case null:
                rachunekDocelowy!.StanRachunku += kwota;
                rachunekDocelowy.Transakcje.Add(new Transakcja(rachunekDocelowy, rachunekDocelowy, kwota, opis));
                return;
        }

        if (rachunekDocelowy == null)
        {
            rachunekZrodlowy!.StanRachunku -= kwota;
            rachunekZrodlowy.Transakcje.Add(new Transakcja(rachunekZrodlowy, rachunekZrodlowy, kwota, opis));
            return;
        }
        
        rachunekZrodlowy.StanRachunku -= kwota;
        rachunekDocelowy.StanRachunku += kwota;
        var transakcja = new Transakcja(rachunekZrodlowy, rachunekDocelowy, kwota, opis);
        rachunekZrodlowy.Transakcje.Add(transakcja);
        rachunekDocelowy.Transakcje.Add(transakcja);
    }

    public static RachunekBankowy operator +(RachunekBankowy rb, PosiadaczRachunku pr)
    {
        if (rb.PosiadaczeRachunku.Contains(pr))
        {
            throw new Exception("Posiadacz jest już posiadaczem tego rachunku");
        }
        rb.PosiadaczeRachunku.Add(pr);
        return rb;
    }
    
    public static RachunekBankowy operator -(RachunekBankowy rb, PosiadaczRachunku pr)
    {
        if (!rb.PosiadaczeRachunku.Contains(pr))
        {
            throw new Exception("Podany posiadacz nie jest posiadaczem tego rachunku");
        }

        if (rb.PosiadaczeRachunku.Count == 1)
        {
            throw new Exception("Nie można usunąć ostatniego posiadacza rachunku");
        }
        rb.PosiadaczeRachunku.Remove(pr);
        return rb;
    }

    public override string ToString()
    {
        var str = $"Rachunek {Numer}\n\tStan: {StanRachunku}";
        str += "\n\tPosiadacze:";
        str = PosiadaczeRachunku.Aggregate(str, (current, posiadacz) => current + $"\n\t\tPosiadacz: {posiadacz}");
        str += "\n\tTransakcje:";
        return Transakcje.Aggregate(str, (current, transakcja) => current + $"\n\t\t{transakcja}");
    }
}