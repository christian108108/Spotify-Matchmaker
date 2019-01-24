using System;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotifyMatchmaker.Library;
using SpotifyMatchmaker.Library.Models;

namespace SpotifyMatchmaker.Service
{
    public static class QueueTriggerCSharp
    {
        [FunctionName("QueueTriggerCSharp")]
        public static void Run([QueueTrigger("spotify-queue", Connection = "AzureWebJobsStorage")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            // parse the json queue data for party code
            var jObject = JObject.Parse(myQueueItem);
            var jToken = jObject.GetValue("party_code");

            string partyCode = jToken.ToString();

            KeyVaultHelper.LogIntoKeyVault();
            var connectionString = KeyVaultHelper.GetSecret("https://spotify-matchmaker.vault.azure.net/secrets/storage-connection-string/");

            var table = AzureStorageHelper.GetOrCreateTableAsync("partyCodes", connectionString).Result;

            var accessTokens = AzureStorageHelper.GetParty(partyCode, table).GetAccessTokens();

            log.LogInformation($"Access token for party code: {partyCode}");
            foreach(var token in accessTokens)
            {
                // log.LogInformation($"Access token: {token}");
            }


            //take the party code and go to Azure Cosmos DB to lookup the party

            //grab spotify tokens from Cosmos DB and snoop through their music collections
            ;
        }
    
    }
}
