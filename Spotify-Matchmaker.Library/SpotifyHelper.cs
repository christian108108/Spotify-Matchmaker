using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
        /// <returns>IEnumerable of Artists</returns>
        public static async Task<IEnumerable<Artist>> GetTopArtistsAsync(string accessToken)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            var result = client.GetStringAsync($"https://api.spotify.com/v1/me/top/artists?time_range=long_term&limit=50").Result;

            var topArtists = TopArtists.FromJson(result);

            var allTopArtists = new HashSet<Artist>();

            allTopArtists.UnionWith(topArtists.Artists);

            while(topArtists.Next != null)
            {
                result = await client.GetStringAsync(topArtists.Next);

                topArtists = TopArtists.FromJson(result);

                allTopArtists.UnionWith(topArtists.Artists);
            }

            allTopArtists.UnionWith(await GetTopArtistsAsync(accessToken, "medium_term"));
            allTopArtists.UnionWith(await GetTopArtistsAsync(accessToken, "short_term"));


            return allTopArtists;
        }

        /// <summary>
        /// Creates REST client, connects, and fetches top artists from a user
        /// </summary>
        /// <param name="accessToken">Required. A valid access token from the Spotify Accounts service</param>
        /// <param name="time_range">Optional. Over what time frame the affinities are computed. Valid values: long_term (calculated from several years of data and including all new data as it becomes available), medium_term (approximately last 6 months), short_term (approximately last 4 weeks). Default: medium_term.</param>
        /// <returns>IEnumerable of Artists</returns>
        private static async Task<IEnumerable<Artist>> GetTopArtistsAsync(string accessToken, 
                                                                string time_range)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            var result = client.GetStringAsync($"https://api.spotify.com/v1/me/top/artists?time_range={time_range}&limit=50").Result;

            var topArtists = TopArtists.FromJson(result);

            var allTopArtists = new HashSet<Artist>();

            allTopArtists.UnionWith(topArtists.Artists);

            while(topArtists.Next != null)
            {
                result = await client.GetStringAsync(topArtists.Next);

                topArtists = TopArtists.FromJson(result);

                allTopArtists.UnionWith(topArtists.Artists);
            }

            return allTopArtists;
        }

        public static async Task<string> CreatePlaylistAsync(string accessToken, string playListName)
        {
            var userId = GetUserAsync(accessToken).Result.Id;

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            //create playlist object

            Playlist newPlaylist = new Playlist();
            newPlaylist.Name = playListName;

            string jsonPlaylist = PlaylistSerialize.ToJson(newPlaylist);

            var stringContent = new StringContent(jsonPlaylist, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"https://api.spotify.com/v1/users/{userId}/playlists", stringContent);

            var msg = response.Content.ReadAsStringAsync().Result;

            var publishedPlaylist = Playlist.FromJson(msg);

            return publishedPlaylist.Id;
        }

        /// <summary>
        /// Gets user object based off access token
        /// </summary>
        /// <param name="accessToken">User's Spotify access token</param>
        /// <returns>User object</returns>
        private static async Task<User> GetUserAsync(string accessToken)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            var stringTask = client.GetStringAsync("https://api.spotify.com/v1/me");

            var msg = await stringTask;

            var user = User.FromJson(msg);

            return user;
        }

        

        /// <summary>
        /// Gets top tracks from a given artist
        /// </summary>
        /// <param name="accessToken">user's access token</param>
        /// <param name="artistId">artist's Spotify ID</param>
        /// <returns>IEnumerable of song URIs</returns>
        public static async Task<IEnumerable<Track>> GetArtistsTopTracksAsync(string accessToken, Artist artist)
        {
            try
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");


                var stringTask = client.GetStringAsync($"https://api.spotify.com/v1/artists/{artist.Id}/top-tracks?country=US");

                var msg = await stringTask;

                var artistTopTracks = ArtistTopTracks.FromJson(msg);

                return artistTopTracks.Tracks;
            }
            catch
            {
                return null;
            }
        }

        public static async void AddSongsToPlaylistAsync(string accessToken, string playlistId, IEnumerable<Track> tracks )
        {
            try
            {
                var userId = GetUserAsync(accessToken).Result.Id;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var uris = new HashSet<string>();

                foreach(var t in tracks)
                {
                    uris.Add(t.Uri);
                }

                string json = JsonConvert.SerializeObject(uris, Formatting.Indented);

                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"https://api.spotify.com/v1/playlists/{playlistId}/tracks", stringContent);
            }
            catch
            {
                return;
            }
        }
    }
}
