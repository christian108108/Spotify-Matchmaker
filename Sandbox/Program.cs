using System;
using System.Collections.Generic;
using System.Linq;
using SpotifyMatchmaker.Library;
using SpotifyMatchmaker.Library.Models;

namespace Sandbox
{
    public class Program
    {
        private const string bearerTokenIdentifier = "https://spotify-matchmaker.vault.azure.net/secrets/secret/";

        private const string accessTokenA = "BQC-45SK8oWNPgIGhOXpgK3Crvq5zECgtmOekoae0YPr6B7IIcn4zBfLu7U5ZgMwVcfL4SsMLoaKTO-Skuy0yBjmwE4ADcRv27FVo9yw6wHV1KYfB_mKPIYoWdzUB3MwPKzL5P1y8iis8AXhH3DJrCBiBV0rypmT3L9Cu8d4n27dv3Uhy_2GbHFde5xUOxDr38S9Ykiiotj99y-Hx7k3j8vYZlzSAutMHC_qVr8yakTjsIo";
        private const string accessTokenB = "BQA5rQlydVzZMfAn-KQu6HootmJfJNt1WAlvli1lvYL29OOoKPOfw-RtjBWGKLeb80Uv4riISl1GkRkHsaZmm25eq3oCCOtnlHIsExinUqjGlKal6vNnBPhKsK22KWXGZ8Rng2vjmt3yeQ7g8Qtzhiw_ZEPs3j4wX5ZcpxXfqg";

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

            // SpotifyHelper.PrintTopArtists(topArtists);

            var commonArtists = FindCommonArtists(topArtistsA, topArtistsB);

            var commonGenres = FindCommonGenres(topArtistsA, topArtistsB);

            var commonArtistsString = String.Join(", ", commonArtists);
            
            var commonGenresString = string.Join(", ", commonGenres);

            Console.WriteLine($"You have these artists in common: {commonArtistsString}");

            Console.WriteLine($"You have these genres in common: {commonGenresString}");
            ;
        }

        static IEnumerable<string> FindCommonArtists(TopArtists artistsA, TopArtists artistsB)
        {
            var hashSetA = new HashSet<string>();
            var hashSetB = new HashSet<string>();

            foreach(var a in artistsA.Artists)
            {
                hashSetA.Add(a.Name);
            }

            foreach(var a in artistsB.Artists)
            {
                hashSetB.Add(a.Name);
            }

            return hashSetA.Intersect(hashSetB);
        }

        static IEnumerable<string> FindCommonGenres(TopArtists artistsA, TopArtists artistsB)
        {
            var genresA = new HashSet<string>();
            var genresB = new HashSet<string>();

            var commonGenres = new HashSet<string>();

            foreach(Artist a in artistsA.Artists)
            {
                genresA.UnionWith(a.Genres);
            }

            foreach(Artist a in artistsB.Artists)
            {
                genresB.UnionWith(a.Genres);
            }

            return genresA.Intersect(genresB);
        }
    }
}
