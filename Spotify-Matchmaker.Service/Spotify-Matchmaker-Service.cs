using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace SpotifyMatchmaker.Service
{
    public static class Spotify_Matchmaker_Service
    {
        [FunctionName("Spotify_Matchmaker_Service")]
        public static void Run([QueueTrigger("myqueue-items", Connection = "spotifymatchmakb77a_STORAGE")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
