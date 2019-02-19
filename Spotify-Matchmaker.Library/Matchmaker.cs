using System.Collections.Generic;
using System.Linq;
using SpotifyMatchmaker.Library.Models;

public class Matchmaker
{
        /// <summary>
        /// Given multiple IEnumerables of Artists, finds what they have in common
        /// </summary>
        /// <param name="artistsA">Top artists for person A</param>
        /// <param name="artistsB">Top artists for person B</param>
        /// <returns>IEnumerable of Artists with no duplicates</returns>
        public static IEnumerable<Artist> FindCommonArtists(IEnumerable<Artist> artistsA, IEnumerable<Artist> artistsB)
        {
            var commonArtists = new HashSet<Artist>();

            commonArtists.UnionWith(artistsA);
            commonArtists.IntersectWith(artistsB);

            return commonArtists;
        }

        public static IEnumerable<string> FindCommonGenres(IEnumerable<Artist> artistsA, IEnumerable<Artist> artistsB)
        {
            var genresA = new HashSet<string>();
            var genresB = new HashSet<string>();

            foreach(Artist a in artistsA)
            {
                genresA.UnionWith(a.Genres);
            }

            foreach(Artist a in artistsB)
            {
                genresB.UnionWith(a.Genres);
            }

            return genresA.Intersect(genresB);
        }

        public static IEnumerable<Artist> SuggestArtists(IEnumerable<Artist> artistsA, IEnumerable<Artist> artistsB)
        {
            var commonGenres = FindCommonGenres(artistsA, artistsB);

            var suggestedArtists = new HashSet<Artist>();

            foreach(Artist a in artistsA)
            {
                if (a.Genres.Intersect(commonGenres).Any())
                {
                    suggestedArtists.Add(a);
                }
            }

            foreach(Artist a in artistsB)
            {
                if(a.Genres.Intersect(commonGenres).Any())
                {
                    suggestedArtists.Add(a);
                }
            }

            return suggestedArtists;
        }
}        