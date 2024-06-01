using System.Net;
using System.Text.Json;

namespace LibraryAzureDevOps;

public class AzureDevOpsApi
{
    private Uri _linkApi;
    private CookieContainer _cookieContainer = new();
    private HttpClientHandler _httpClientHandler = new();

    public AzureDevOpsApi(string linkApi, string userAuthentication)
    {
        _linkApi = new Uri(linkApi);

        _cookieContainer.Add(_linkApi, new Cookie("UserAuthentication", userAuthentication));
        
        _httpClientHandler = new HttpClientHandler
        {
            CookieContainer = _cookieContainer
        };
    }

    public async Task<string> GetLibraries()
    {
        using (HttpClient client = new HttpClient(_httpClientHandler))
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(_linkApi);

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
}