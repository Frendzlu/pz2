namespace Lab02;

public class OsobaFizyczna : PosiadaczRachunku
{
    private string Imie { get; set; }
    private string Nazwisko { get; set; }
    private string DrugieImie { get; set; }

    private string _pesel;
    public string Pesel
    {
        get => _pesel;
        set
        {
            if (value.Length != 11 || value == null)
            {
                throw new Exception("PESEL musi mieć 11 cyfr");
            }
            _pesel = value;
        }
    }
    private string NumerPaszportu { get; set; }

    public override string ToString()
    {
        return $"\n\tOsoba Fizyczna: {Imie} {Nazwisko}";
    }

    public OsobaFizyczna(string imie, string nazwisko, string drugieImie, string? pesel, string? numerPaszportu)
    {
        if (pesel == null && numerPaszportu == null)
        {
            throw new Exception("PESEL albo numer paszportu musi nie być null");
        }

        if (pesel != null && pesel.Length != 11)
        {
            throw new Exception("PESEL musi mieć 11 cyfr");
        }
        Imie = imie;
        Nazwisko = nazwisko;
        DrugieImie = drugieImie;
        _pesel = pesel ?? string.Empty;
        NumerPaszportu = numerPaszportu ?? string.Empty;
    }
}