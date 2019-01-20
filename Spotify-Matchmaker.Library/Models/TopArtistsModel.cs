using System;

namespace SpotifyMatchmaker.Library.Models
{
    public class TopArtistsModel
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String Uri { get; set; }
        public String[] Genres { get; set; }
    }
}
