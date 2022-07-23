using System.Net.Http.Headers;

namespace RicardoSalesWeb.DAL
{
    public class DataAgents
    {
        private readonly string urlApi;
        private readonly Uri urlApiCall;
        private HttpResponseMessage response;
        public DataAgents(IConfiguration configuration, string controllerName) 
        {
            urlApi = configuration["urlApi"];
            urlApiCall = new Uri(String.Concat(urlApi, "/", controllerName));
            response=new HttpResponseMessage();
        }

        private const int TIMEOUT = 5;

        public async Task<string> ActionGet()
        {
            using HttpClient httpClient = new();
            configureClient(httpClient, "GET");
            response = await httpClient.GetAsync(urlApiCall).ConfigureAwait(false);
            return await getResponseAsString(response).ConfigureAwait(false);
        }

        public async Task<string> ActionPost(object body)
        {   
            using HttpClient httpClient = new();
            configureClient(httpClient, "POST");
            response = await httpClient.PostAsJsonAsync(urlApiCall, body).ConfigureAwait(false);
            return await getResponseAsString(response).ConfigureAwait(false);
        }

        public async Task<string> ActionPut(object body)
        {               
            using HttpClient httpClient = new();
            configureClient(httpClient, "PUT");
            response = await httpClient.PutAsJsonAsync( urlApiCall, body).ConfigureAwait(false);
            return await getResponseAsString(response).ConfigureAwait(false);
        }

        public async Task<string> ActionDelete(object body)
        {
            using HttpClient httpClient = new();
            configureClient(httpClient, "DELETE");
            response = await httpClient.PutAsJsonAsync(urlApiCall, body).ConfigureAwait(false);
            return await getResponseAsString(response).ConfigureAwait(false);
        }

        private static void configureClient(HttpClient client, string verJson)
        {
            client.Timeout = TimeSpan.FromMinutes(TIMEOUT);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-HTTP-Method-Override", verJson);
        }
        private async Task<string> getResponseAsString (HttpResponseMessage data)
        {
            if (data!=null)
            {
                return await data.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            return String.Empty;
        }
    }
}
