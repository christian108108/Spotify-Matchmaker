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
        private const string accessTokenA = "BQDODVv7y1kzIjnH6RjEu70O2FxHYydzytD0KH7H05IJnBcYGhN3IEVJ9hCP5tMuHhtjQYdeGvPJ6GrVJs9eeRnkzaMDqSRjKruy4pidsFTeRl2fqY-WR6shga4SsQ4_8d1ZP_2N64CBlRj2ljUj7EMp88fRw171XERvuje6Kvbm3jkmzSQJrSEnPeTI8CqVAbuilvhLBX1JsQVVnj2D3WciXa1aFN_QQCLy";
        private const string accessTokenB = "BQAF8JHlKN5PcxGgn_hHPBP1X7m0LFB8ZxixSCBfZq_vHM5q9HFYo4EosHiBpCwf4u-jpyUgAQVqGnNqWFdRU8_ULiN1Bo9frXciDresnepSPgjuy0ALxMPGqbuPaRwVXCjxHlz98qXxoH1H_k5Nw46qB9IEpx2yMdZZHr5iLiDvI2QfTbLogdSZvtaXKNe3yQiuJQqepImhA83xbWChTQzWupjsqsIlPmQyCzPuxn8zjGw";

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

            // var playListURI = SpotifyHelper.CreatePlaylistAsync(accessTokenB, "Spotify Matchmaker Playlist").Result;

            var trackUris = SpotifyHelper.GetArtistsTopTracksAsync(accessTokenB, artistIDs[0]).Result;

            ;
        }
    }
}
