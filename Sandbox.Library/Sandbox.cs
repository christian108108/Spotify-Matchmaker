using System;
using System.Net;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace Sandbox.Library
{
    public class Sandbox
    {
        /// <summary>
        /// Key Vault client to conveniently get secrets from Azure
        /// </summary>
        /// <value></value>
        public static KeyVaultClient keyVaultClient { get; private set; }

        /// <summary>
        /// Logs into Azure KeyVault and makes keyVaultClient active
        /// </summary>
        public static void LogIntoKeyVault()
        {
            // authenticating with Azure Managed Service Identity
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();

            keyVaultClient = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
        }
        
        /// <summary>
        /// Gets Spotify Data from given access tokens
        /// </summary>
        public static void GetSpotifyData()
        {
            LogIntoKeyVault();

            var token = keyVaultClient.GetSecretAsync("https://spotify-matchmaker.vault.azure.net/secrets/secret").Result.Value;
            var fullToken = "Bearer " + token;


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.spotify.com/v1/me/top/artists");
            request.Method = "Get";
            request.KeepAlive = true;
            request.ContentType = "application/json";
            request.Headers.Add("Authorization",fullToken);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string myResponse = "";
            using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
            {
                myResponse = sr.ReadToEnd();
            }
            Console.WriteLine(myResponse);
        }
    }
}
