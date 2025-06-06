using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

static async Task Main(string[] args)
{
    Console.WriteLine("=== REST API Client ===");

    Console.Write("Login: ");
    var login = Console.ReadLine()?.Trim();

    Console.Write("Token: ");
    var token = Console.ReadLine()?.Trim();

    if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(token))
    {
        Console.WriteLine("Błąd: Login i token są wymagane.");
        return;
    }

    if (!await ValidateCredentials(login, token))
    {
        Console.WriteLine("Niepoprawny login lub token.");
        return;
    }

    Console.WriteLine("Zalogowano pomyślnie.");

    Console.Write("Wpisz operację (show / create / update / destroy): ");
    var operation = Console.ReadLine()?.Trim().ToLower();

    Console.Write("Wpisz nazwę tabeli (np. dane): ");
    var table = Console.ReadLine()?.Trim().ToLower();

    string baseUrl = $"https://localhost:5001/api/{table}?login={login}&token={token}";
    var client = new HttpClient();

    switch (operation)
    {
        case "show":
            var showResp = await client.GetAsync(baseUrl);
            Console.WriteLine(await showResp.Content.ReadAsStringAsync());
            break;

        case "create":
            Console.Write("Podaj dane JSON do utworzenia: ");
            var jsonCreate = Console.ReadLine();
            var createResp = await client.PostAsync(baseUrl, new StringContent(jsonCreate, Encoding.UTF8, "application/json"));
            Console.WriteLine(await createResp.Content.ReadAsStringAsync());
            break;

        case "update":
            Console.Write("ID rekordu do zaktualizowania: ");
            var updateId = Console.ReadLine();
            Console.Write("Nowe dane JSON: ");
            var jsonUpdate = Console.ReadLine();
            var updateUrl = $"https://localhost:5001/api/{table}/{updateId}?login={login}&token={token}";
            var updateResp = await client.PutAsync(updateUrl, new StringContent(jsonUpdate, Encoding.UTF8, "application/json"));
            Console.WriteLine(await updateResp.Content.ReadAsStringAsync());
            break;

        case "destroy":
            Console.Write("ID rekordu do usunięcia: ");
            var deleteId = Console.ReadLine();
            var deleteUrl = $"https://localhost:5001/api/{table}/{deleteId}?login={login}&token={token}";
            var deleteResp = await client.DeleteAsync(deleteUrl);
            Console.WriteLine(await deleteResp.Content.ReadAsStringAsync());
            break;

        default:
            Console.WriteLine("Nieznana operacja.");
            break;
    }
}

static async Task<bool> ValidateCredentials(string login, string token)
{
    string url = $"https://localhost:5001/api/auth/validate?login={login}&token={token}";

    try
    {
        var client = new HttpClient();
        var response = await client.GetAsync(url);
        return response.IsSuccessStatusCode;
    }
    catch
    {
        return false;
    }
}
