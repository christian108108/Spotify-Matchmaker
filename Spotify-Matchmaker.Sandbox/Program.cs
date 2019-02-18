using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpotifyMatchmaker.Library;
using SpotifyMatchmaker.Library.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Table;

namespace SpotifyMatchmaker.Sandbox
{
    public class Program
    {
        private const string bearerTokenIdentifier = "https://spotify-matchmaker.vault.azure.net/secrets/secret/";
        private const string accessTokenA = "";
        private const string accessTokenB = "";

        static void Main(string[] args)
        {
            // string accessToken = KeyVaultHelper.GetSecret(bearerTokenIdentifier);

            // var storageAccount = AzureStorageHelper.CreateStorageAccountFromConnectionString(connectionString);
            
            CloudTable table = AzureStorageHelper.GetOrCreateTableAsync("partyCodes").Result;

            // AzureStorageHelper.DeleteParty("WXYZ", table);

            // AzureStorageHelper.InsertOrMergeParty(new Party("IYEY"), table);
            // AzureStorageHelper.InsertAccessTokenToParty("WXYZ", table, accessTokenA);

            var accessTokens = AzureStorageHelper.GetParty("WXYZ", table).GetAccessTokens();

            var topArtists = new List<IEnumerable<Artist>>();

            foreach(var token in accessTokens)
            {
                topArtists.Add(SpotifyHelper.GetTopArtistsAsync(token).Result);
            }

            var commonArtists = SpotifyHelper.FindCommonArtists(topArtists.ElementAt(0), topArtists.ElementAt(1));
            var commonGenres = SpotifyHelper.FindCommonGenres(topArtists.ElementAt(0), topArtists.ElementAt(1));

            var difference = topArtists.ElementAt(0).Except(commonArtists);

            ;

            // var topArtistsA = SpotifyHelper.GetTopArtistsAsync(accessTokenA, "short_term").Result;
            // var topArtistsB = SpotifyHelper.GetTopArtistsAsync(accessTokenB, "short_term").Result;
            // ;
            // // var userA = SpotifyHelper.GetUserAsync(accessTokenA).Result;
            // // var userB = SpotifyHelper.GetUserAsync(accessTokenB).Result;

            // // SpotifyHelper.CreatePlaylistAsync(accessToken, user.Id).Wait();

            // var commonArtists = SpotifyHelper.FindCommonArtists(topArtistsA, topArtistsB);

            // var commonGenres = SpotifyHelper.FindCommonGenres(topArtistsA, topArtistsB);

            // var commonArtistsString = String.Join(", ", commonArtists);
            
            // var commonGenresString = string.Join(", ", commonGenres);

            // Console.WriteLine($"You have these artists in common: {commonArtistsString}");

            // Console.WriteLine($"You have these genres in common: {commonGenresString}");

            // var suggestedArtists = SpotifyHelper.SuggestArtists(topArtistsA, topArtistsB);
            // var suggestedArtistsString = String.Join(", ", suggestedArtists);
            // Console.WriteLine($"We suggest listening to these artists: {suggestedArtistsString}");

            // var artistIDs = SpotifyHelper.SuggestArtistIDs(topArtistsA, topArtistsB).ToList();

            var playListId = SpotifyHelper.CreatePlaylistAsync(accessTokens[0], "Spotify Matchmaker Playlist").Result;

            foreach(var artist in commonArtists)
            {
                var tracks = SpotifyHelper.GetArtistsTopTracksAsync(accessTokens[0], artist).Result;
                SpotifyHelper.AddSongsToPlaylistAsync(accessTokens[0], playListId, tracks);
            }

            ;
        }
    }
}
