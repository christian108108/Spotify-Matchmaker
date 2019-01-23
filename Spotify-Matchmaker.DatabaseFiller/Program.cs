using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using SpotifyMatchmaker.Library;
using SpotifyMatchmaker.Library.Models;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage;

namespace DatabaseFiller
{
    class Program
    {

        /// <summary>
        /// Randomly generates and saves 100 entries of Party entities as JSON
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            
            ;
        }

        public void FillQueue()
        {
            KeyVaultHelper.LogIntoKeyVault();
            var connectionString = KeyVaultHelper.GetSecret("https://spotify-matchmaker.vault.azure.net/secrets/storage-connection-string/");

            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference("spotify-queue");

            // Create the queue if it doesn't already exist.
            queue.CreateIfNotExists();

            HashSet<string> partyCodes = new HashSet<string>();

            for(int i=0; i<100; i++)
            {
                // Party newParty = MakeRandomParty();
                string randomCode = RandomString(4);

                partyCodes.Add(randomCode);
            }

            // Create a message and add it to the queue.
            CloudQueueMessage message = new CloudQueueMessage("Hello, World");
            queue.AddMessage(message);
        }

        public static Party MakeRandomParty()
        {
            Party party = new Party();

            party.PartyCode = RandomString(4);
            party.Host = RandomString(258);
            party.Person2 = RandomString(258);
            party.Person3 = RandomString(258);
            party.Person4 = RandomString(258);
            party.Person5 = RandomString(258);

            return party;
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
