using System;
using Sandbox.Library;
using Sandbox.Library.Models;

namespace Sandbox
{
    public class Program
    {
        static void Main(string[] args)
        {
            Sandbox.Library.ApiHelper.InitializeClient();
            Console.ReadLine();
        }

        //public static async void GetUser()
        //{
        //    Sandbox.Library.SpotifyConnection _spotify = new Sandbox.Library.SpotifyConnection();
        //    UserModel user = await _spotify.GetUser();
        //    Console.WriteLine(user.Display_name);
        //}

        public static async void GetTopTracks()
        {
            Sandbox.Library.SpotifyConnection _spotify = new Sandbox.Library.SpotifyConnection();
            TopTracksModel topTracks = await _spotify.GetTopTracks();
        }

        public static async void GetTopArtists()
        {
            Sandbox.Library.SpotifyConnection _spotify = new SpotifyConnection();
            TopArtistsModel topArtists = await _spotify.GetTopArtists();
        }
    }
}
