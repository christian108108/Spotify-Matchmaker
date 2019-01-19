using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Sandbox.Library.Models;
using Newtonsoft.Json;

namespace Sandbox.Library
{
    public class SpotifyConnection
    {
        public async Task<TopTracksModel> GetTopTracks() 
        {
            string url = "https://api.spotify.com/v1/me/top/tracks";
            
            try 
            {
                using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        TopTracksModel topTracks = JsonConvert.DeserializeObject<TopTracksModel>(await response.Content.ReadAsStringAsync());
                        return topTracks;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<TopArtistsModel> GetTopArtists()
        {
            string url = "https://api.spotify.com/v1/me/top/artists";

            try
            {
                using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        TopArtistsModel topArtists = JsonConvert.DeserializeObject<TopArtistsModel>(await response.Content.ReadAsStringAsync());
                        return topArtists;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async void CreatePlaylist()
        {
            string url = "https://api.spotify.com/v1/playlists";

        }


        //public async Task<UserModel> GetUser()
        //{
        //    string url = "https://api.spotify.com/v1/me";
        //    //string url = "https://xkcd.com/info.0.json";
        //    Console.Write("Hello World");
        //    try
        //    {
        //        using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
        //        {
        //            if (response.IsSuccessStatusCode)
        //            {
        //                UserModel user = JsonConvert.DeserializeObject<UserModel>(await response.Content.ReadAsStringAsync());
        //                Console.WriteLine("Success");
        //                return user;
        //            }
        //            else
        //            {
        //                Console.WriteLine("Failure");
        //                throw new Exception(response.ReasonPhrase);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        return null;
        //    }

        //}
    }
}
