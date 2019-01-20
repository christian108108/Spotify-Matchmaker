using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using SpotifyMatchmaker.Library.Models;

namespace SpotifyMatchmaker.Library
{
    public class SpotifyHelper
    {
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Creates REST client, connects, and fetches top artists from a user
        /// </summary>
        /// <param name="accessToken">Required. A valid access token from the Spotify Accounts service</param>
        /// <param name="time_range">Optional. Over what time frame the affinities are computed. Valid values: long_term (calculated from several years of data and including all new data as it becomes available), medium_term (approximately last 6 months), short_term (approximately last 4 weeks). Default: medium_term.</param>
        /// <returns>TopArtists object</returns>
        public static async Task<TopArtists> GetTopArtistsAsync(string accessToken, 
                                                                string time_range = "medium_term")
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            // client.DefaultRequestHeaders.Add("time_range", $"{time_range}");

            var stringTask = client.GetStringAsync($"https://api.spotify.com/v1/me/top/artists?time_range={time_range}");

            var msg = await stringTask;

            var topArtists = TopArtists.FromJson(msg);

            return topArtists;
        }

        public static async Task<string> CreatePlaylistAsync(string accessToken, string playListName)
        {
            var userId = GetUserAsync(accessToken).Result.Id;

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            //create playlist object

            Playlist newPlaylist = new Playlist();
            newPlaylist.Name = playListName;

            string jsonPlaylist = PlaylistSerialize.ToJson(newPlaylist);

            var stringContent = new StringContent(jsonPlaylist, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"https://api.spotify.com/v1/users/{userId}/playlists", stringContent);

            var msg = response.Content.ReadAsStringAsync().Result;

            var publishedPlaylist = Playlist.FromJson(msg);

            return publishedPlaylist.Uri;
        }

        private static async Task<User> GetUserAsync(string accessToken)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            var stringTask = client.GetStringAsync("https://api.spotify.com/v1/me");

            var msg = await stringTask;

            var user = User.FromJson(msg);

            return user;
        }

        public static void PrintTopArtists(TopArtists artists)
        {
            foreach(var a in artists.Artists)
            {
                Console.Write($"You like listening to {a.Name}! Their popularity score is {a.Popularity}. Genres: ");
            
                Console.WriteLine(String.Join(", ", a.Genres));
            }
        }

        public static IEnumerable<string> FindCommonArtists(TopArtists artistsA, TopArtists artistsB)
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

        public static IEnumerable<string> FindCommonGenres(TopArtists artistsA, TopArtists artistsB)
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

        public static IEnumerable<string> SuggestArtists(TopArtists artistsA, TopArtists artistsB)
        {
            var commonGenres = SpotifyHelper.FindCommonGenres(artistsA, artistsB);

            var suggestedArtists = new HashSet<string>();

            foreach(Artist a in artistsA.Artists)
            {
                if (a.Genres.Intersect(commonGenres).Any())
                {
                    suggestedArtists.Add(a.Name);
                }
            }

            foreach(Artist a in artistsB.Artists)
            {
                if(a.Genres.Intersect(commonGenres).Any())
                {
                    suggestedArtists.Add(a.Name);
                }
            }

            return suggestedArtists;
        }

        public static IEnumerable<string> SuggestArtistIDs(TopArtists artistsA, TopArtists artistsB)
        {
            var commonGenres = SpotifyHelper.FindCommonGenres(artistsA, artistsB);

            var suggestedArtists = new HashSet<string>();

            foreach(Artist a in artistsA.Artists)
            {
                if (a.Genres.Intersect(commonGenres).Any())
                {
                    suggestedArtists.Add(a.Id);
                }
            }

            foreach(Artist a in artistsB.Artists)
            {
                if(a.Genres.Intersect(commonGenres).Any())
                {
                    suggestedArtists.Add(a.Id);
                }
            }

            return suggestedArtists;
        }

        /// <summary>
        /// Gets top tracks from a given artist
        /// </summary>
        /// <param name="accessToken">user's access token</param>
        /// <param name="artistId">artist's Spotify ID</param>
        /// <returns>IEnumerable of song URIs</returns>
        public static async Task<IEnumerable<string>> GetArtistsTopTracksAsync(string accessToken, string artistId)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");


            var stringTask = client.GetStringAsync($"https://api.spotify.com/v1/artists/{artistId}/top-tracks?country=US");

            var msg = await stringTask;

            var tracks = Tracks.FromJson(msg);

            var trackUris = new HashSet<string>();

            foreach(Track t in tracks.TracksTracks)
            {
                trackUris.Add(t.Uri);
            }

            return trackUris;
        }

        public static async void AddSongsToPlaylistAsync(string accessToken, string playlistId, IEnumerable<string> songURIs )
        {
            var userId = GetUserAsync(accessToken).Result.Id;

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            var stringContent = new StringContent("", Encoding.UTF8, "application/json");

            foreach(string songURI in songURIs)
            {
                
            }

            var response = await client.PostAsync($"https://api.spotify.com/v1/playlists/{playlistId}/tracks", stringContent);
        }


    }

}
