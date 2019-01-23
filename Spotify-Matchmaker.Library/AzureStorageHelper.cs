using System;
using SpotifyMatchmaker.Library;
using SpotifyMatchmaker.Library.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Table;
using System.Threading.Tasks;

namespace SpotifyMatchmaker.Library
{
    public class AzureStorageHelper
    {
        /// <summary>
        /// Validate the connection string information in app.config and throws an exception if it looks like 
        /// the user hasn't updated this to valid values. 
        /// </summary>
        /// <param name="storageConnectionString">Connection string for the storage service or the emulator</param>
        /// <returns>CloudStorageAccount object</returns>
        public static CloudStorageAccount CreateStorageAccountFromConnectionString(string connectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(connectionString);
            }

            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                throw;
            }

            catch (ArgumentException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                throw;
            }

            return storageAccount;
        }

        /// <summary>
        /// Create a table for the sample application to process messages in. 
        /// </summary>
        /// <param name="tableName">name of table you'd like to create or access</param>
        /// <param name="connectionString">connection string for Azure Storage account you'd like to access</param>
        /// <returns>Task containing a CloudTable object of the cloud table you created or accessed</returns>
        public static async Task<CloudTable> GetOrCreateTableAsync(string tableName, string connectionString)
        {
            // Retrieve storage account information from connection string.
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(connectionString);

            // Create a table client for interacting with the table service
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create a table client for interacting with the table service 
            CloudTable table = tableClient.GetTableReference(tableName);
            try
            {
                await table.CreateIfNotExistsAsync();
            }
            catch (StorageException)
            {
                Console.WriteLine("If you are running with the default configuration please make sure you have started the storage emulator. Press the Windows key and type Azure Storage to select and run it from the list of applications - then restart the sample.");
                throw;
            }

            return table;
        }

        /// <summary>
        /// Create a table for the sample application to process messages in.
        /// Automatically logs into Key Vault and grabs connection string
        /// </summary>
        /// <param name="tableName">name of table you'd like to create or access</param>
        /// <returns>Task containing a CloudTable object of the cloud table you created or accessed</returns>
        public static async Task<CloudTable> GetOrCreateTableAsync(string tableName)
        {
            KeyVaultHelper.LogIntoKeyVault();
            string connectionString = KeyVaultHelper.GetSecret("https://spotify-matchmaker.vault.azure.net/secrets/storage-connection-string/");

            // Retrieve storage account information from connection string.
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(connectionString);

            // Create a table client for interacting with the table service
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create a table client for interacting with the table service 
            CloudTable table = tableClient.GetTableReference(tableName);
            try
            {
                await table.CreateIfNotExistsAsync();
            }
            catch (StorageException)
            {
                Console.WriteLine("If you are running with the default configuration please make sure you have started the storage emulator. Press the Windows key and type Azure Storage to select and run it from the list of applications - then restart the sample.");
                throw;
            }

            return table;
        }
      
        /// <summary>
        /// Inserts the party to Azure Cloud Storage
        /// </summary>
        /// <param name="party">Party object that contains Spotify access tokens</param>
        /// <param name="cloudTable">Cloud table to which you'd like to insert the party</param>
        /// <returns>Newly inserted party</returns>
        public static Party InsertOrMergeParty(Party party, CloudTable cloudTable)
        {
            var tableOperation = TableOperation.InsertOrMerge(party);
            var result = cloudTable.Execute(tableOperation);

            Party retrievedParty;

            try
            {
                retrievedParty = (Party)result.Result;
            }
            catch
            {
                retrievedParty = null;
            }
            return retrievedParty;
        }

        /// <summary>
        /// Fetches the party from Azure Cloud Storage that contains Spotify access tokens
        /// </summary>
        /// <param name="partyCode">Unique identifier for the party you'd like to access</param>
        /// <param name="cloudTable">Cloud table from which you'd like to fetch the party</param>
        /// <returns>Party object containing Spotify access tokens</returns>
        public static Party GetPartyFromPartyCode(string partyCode, CloudTable cloudTable)
        {
            var retrieveOperation = TableOperation.Retrieve<Party>("parties", partyCode);
            var result = cloudTable.Execute(retrieveOperation);

            // if there's no entity in the table yet, create it!
            if (result.HttpStatusCode == 404)
            {
                // return newly created party
                return AzureStorageHelper.InsertOrMergeParty(new Party(partyCode), cloudTable);
            }
            return (Party)result.Result;
        }

        /// <summary>
        /// Inserts new access token to new or existing party
        /// </summary>
        /// <param name="partyCode">Party code in which you'd like to insert the new access token</param>
        /// <param name="cloudTable">Azure Cloud Table in which you'd like to insert new access token</param>
        /// <param name="accessToken">Spotify access token for a user</param>
        /// <returns>Newly modified party object</returns>
        public static Party InsertAccessTokenToParty(string partyCode, CloudTable cloudTable, string accessToken)
        {
            var party = GetPartyFromPartyCode(partyCode, cloudTable);

            if(String.IsNullOrEmpty(party.Host))
            {
                party.Host = accessToken;
            }
            else if(String.IsNullOrEmpty(party.Person2))
            {
                party.Person2 = accessToken;
            }
            else if(String.IsNullOrEmpty(party.Person3))
            {
                party.Person3 = accessToken;
            }
            else if(String.IsNullOrEmpty(party.Person4))
            {
                party.Person4 = accessToken;
            }
            else if(String.IsNullOrEmpty(party.Person5))
            {
                party.Person5 = accessToken;
            }
            else
            {
                return null;
            }

            return InsertOrMergeParty(party, cloudTable);
        }
    }

}
