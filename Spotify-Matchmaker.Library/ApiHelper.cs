using System;
using System.Net.Http;
using System.Net.Http.Headers;
using SpotifyMatchmaker.Library;

namespace SpotifyMatchmaker.Library
{
    public class ApiHelper
    {
        public static HttpClient ApiClient { get; set; }
        const string bearerTokenIdentifier = "https://spotify-matchmaker.vault.azure.net/secrets/secret/";

        public static void InitializeClient()
        {
            KeyVaultHelper.LogIntoKeyVault();
            string bearerToken = KeyVaultHelper.GetSecret(bearerTokenIdentifier);

            ApiClient = new HttpClient();
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {bearerToken}");
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }

}
