using System;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotifyMatchmaker.Models;

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

            string party_code = jToken.ToString();

            Party party = JsonConvert.DeserializeObject<Party>(myQueueItem);

            ;


            //take the party code and go to Azure Cosmos DB to lookup the party

            //grab spotify tokens from Cosmos DB and snoop through their music collections
            ;
        }
    
    }
}
