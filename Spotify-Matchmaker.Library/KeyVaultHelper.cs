using System;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace SpotifyMatchmaker.Library
{
    public class KeyVaultHelper
    {
        /// <summary>
        /// Key Vault client that can get keys and secrets
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
        /// Gets secret value from Azure KeyVault
        /// </summary>
        /// <param name="secretIdentifier">URI found in the Azure portal that identifies a secret</param>
        /// <returns>string value of secret</returns>
        public static string GetSecret(string secretIdentifier)
        {
            return keyVaultClient.GetSecretAsync(secretIdentifier).Result.Value;
        }
    }

}
