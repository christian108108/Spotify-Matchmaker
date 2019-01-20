using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpotifyMatchmaker.Library;
using SpotifyMatchmaker.Library.Models;

namespace Sandbox
{
    public class Program
    {
        private const string bearerTokenIdentifier = "https://spotify-matchmaker.vault.azure.net/secrets/secret/";
        private const string accessTokenA = "";
        private const string accessTokenB = "";

        static void Main(string[] args)
        {
            // KeyVaultHelper.LogIntoKeyVault();
            // string accessToken = KeyVaultHelper.GetSecret(bearerTokenIdentifier);

            var topArtistsA = SpotifyHelper.GetTopArtistsAsync(accessTokenA, "short_term").Result;
            var topArtistsB = SpotifyHelper.GetTopArtistsAsync(accessTokenB, "short_term").Result;
            ;
            // var userA = SpotifyHelper.GetUserAsync(accessTokenA).Result;
            // var userB = SpotifyHelper.GetUserAsync(accessTokenB).Result;

            // SpotifyHelper.CreatePlaylistAsync(accessToken, user.Id).Wait();

            var commonArtists = SpotifyHelper.FindCommonArtists(topArtistsA, topArtistsB);

            var commonGenres = SpotifyHelper.FindCommonGenres(topArtistsA, topArtistsB);

            var commonArtistsString = String.Join(", ", commonArtists);
            
            var commonGenresString = string.Join(", ", commonGenres);

            Console.WriteLine($"You have these artists in common: {commonArtistsString}");

            Console.WriteLine($"You have these genres in common: {commonGenresString}");

            var suggestedArtists = SpotifyHelper.SuggestArtists(topArtistsA, topArtistsB);
            var suggestedArtistsString = String.Join(", ", suggestedArtists);
            Console.WriteLine($"We suggest listening to these artists: {suggestedArtistsString}");

            var artistIDs = SpotifyHelper.SuggestArtistIDs(topArtistsA, topArtistsB).ToList();

            var playListId = SpotifyHelper.CreatePlaylistAsync(accessTokenB, "Spotify Matchmaker Playlist").Result;

            foreach(var id in artistIDs)
            {
                var trackUris = SpotifyHelper.GetArtistsTopTracksAsync(accessTokenB, id).Result;
                SpotifyHelper.AddSongsToPlaylistAsync(accessTokenB, playListId, trackUris);
            }

            ;
        }
    }
}
