using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace LibraryAzureDevOps;

public class AzureDevOpsApi(Settings settings)
{
    private readonly Uri _linkApi = new(
        $"https://dev.azure.com/{settings.Organization}/{settings.Project}/_apis/distributedtask/variablegroups?api-version={settings.ApiVersion}&groupName=*");

    private readonly HttpClient _client = new();
    private readonly string _accessToken = ConvertToBase64(settings.PersonalAccessTokens);

    private static string ConvertToBase64(string personalAccessToken)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes($":{personalAccessToken}");
        return Convert.ToBase64String(plainTextBytes);
    }

    public async Task<string> GetLibraries()
    {
        try
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", $" {_accessToken}");

            HttpResponseMessage response = await _client.GetAsync(_linkApi);

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            using (JsonDocument document = JsonDocument.Parse(responseBody))
            {
                JsonElement root = document.RootElement;

                var values = root.GetProperty("value");

                return values.ToString();
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("Ocorreu um erro na requisição: " + e.Message);
            return string.Empty;
        }
    }
}