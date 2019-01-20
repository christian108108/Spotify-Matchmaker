using SpotifyMatchmaker.Library;

namespace Sandbox
{
    public class Program
    {
        private const string bearerTokenIdentifier = "https://spotify-matchmaker.vault.azure.net/secrets/secret/";

        static void Main(string[] args)
        {
            KeyVaultHelper.LogIntoKeyVault();
            string accessToken = KeyVaultHelper.GetSecret(bearerTokenIdentifier);

            var topArtists = SpotifyHelper.GetTopArtistsAsync(accessToken, "long_term").Result;
            
            var user = SpotifyHelper.GetUserAsync(accessToken).Result;

            SpotifyHelper.CreatePlaylistAsync(accessToken, user.Id).Wait();

            SpotifyHelper.PrintTopArtists(topArtists);
        }
    }
}
