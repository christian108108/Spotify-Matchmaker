using System;
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

        public static async Task CreatePlaylistAsync(string accessToken, string userId)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            //create playlist object

            Playlist newPlaylist = new Playlist();
            newPlaylist.Name = "Spotify Matchmaker Playlist";

            string jsonPlaylist = PlaylistSerialize.ToJson(newPlaylist);

            var stringContent = new StringContent(jsonPlaylist, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"https://api.spotify.com/v1/users/{userId}/playlists", stringContent);
        }

        public static async Task<User> GetUserAsync(string accessToken)
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



    }

}
