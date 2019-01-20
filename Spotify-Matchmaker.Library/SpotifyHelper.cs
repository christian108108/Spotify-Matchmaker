using System;
using System.Net.Http;
using System.Net.Http.Headers;
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
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            client.DefaultRequestHeaders.Add("time_range", $"{time_range}");

            var stringTask = client.GetStringAsync("https://api.spotify.com/v1/me/top/artists");

            var msg = await stringTask;

            var topArtists = TopArtists.FromJson(msg);

            return topArtists;
        }

    }

}
