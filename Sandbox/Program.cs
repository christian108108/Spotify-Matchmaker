using System;
using SpotifyMatchmaker.Library;
using SpotifyMatchmaker.Library.Models;

namespace Sandbox
{
    public class Program
    {
        static TopArtists topArtists;
        static TopTracks topTracks;

        static void Main(string[] args)
        {
            ApiHelper.InitializeClient();
            SpotifyConnection _spotify = new SpotifyConnection();

            topArtists = _spotify.GetTopArtists().Result;
            topTracks =  _spotify.GetTopTracks().Result;
            ;
        }

        //public static async void GetUser()
        //{
        //    Sandbox.Library.SpotifyConnection _spotify = new Sandbox.Library.SpotifyConnection();
        //    UserModel user = await _spotify.GetUser();
        //    Console.WriteLine(user.Display_name);
        //}

    }
}
