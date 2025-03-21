namespace Lab02;

public class OsobaPrawna : PosiadaczRachunku
{
    private string Nazwa {get;}
    private string Siedziba {get;}

    public OsobaPrawna(string nazwa, string siedziba)
    {
        Nazwa = nazwa;
        Siedziba = siedziba;
    }
    
    public override string ToString()
    {
        return $"\n\tOsoba prawna: {Nazwa} {Siedziba}";
    }
}