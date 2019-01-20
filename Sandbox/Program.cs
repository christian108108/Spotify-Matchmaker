using System;
using SpotifyMatchmaker.Library;
using SpotifyMatchmaker.Library.Models;

namespace Sandbox
{
    public class Program
    {
        static TopArtistsModel topArtists;
        static TopTracksModel topTracks;

        static void Main(string[] args)
        {
            ApiHelper.InitializeClient();
            topArtists = GetTopArtists();
            topTracks =  GetTopTracks();
            ;
        }

        //public static async void GetUser()
        //{
        //    Sandbox.Library.SpotifyConnection _spotify = new Sandbox.Library.SpotifyConnection();
        //    UserModel user = await _spotify.GetUser();
        //    Console.WriteLine(user.Display_name);
        //}

        public static TopTracksModel GetTopTracks()
        {
            SpotifyConnection _spotify = new SpotifyConnection();
            return _spotify.GetTopTracks().Result;
        }

        public static TopArtistsModel GetTopArtists()
        {
            SpotifyConnection _spotify = new SpotifyConnection();
            return _spotify.GetTopArtists().Result;
        }
    }
}
