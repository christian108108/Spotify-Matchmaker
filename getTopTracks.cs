using System;
using System.IO;
using System.Net;
using System.Net.Http;

namespace test
{
    public class getTopTracks
    {
        public getTopTracks()
        {


        }

        static void Main(String[] args)
        {
            var token = "BQDowp_LJuZNGYeq_6wm7enw6Y4LxDmX-LSBl1Fsor5qYuJbqTaCe-WCbjN8go1LwTyrwDx-GCW1Q0_hjjuKOqSnlMJzYYzq6-0GGtvAUwvVblKCN3n1YL82VHFx5gMFc43eUpA60u607fOYPSgVwAAdA66sEmjTHxD5r9VmDUHzpOHDy5Kg0LXRwlkacJeoMG2D6K36SU3OwFzdVZ5dl6sFEtvOPjg1YtXwhmWmkKl2Oft8-81D0F1iNmSd9sou4Q";
            var fullToken = "Bearer " + token;
                

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.spotify.com/v1/me/top/artists");
            request.Method = "Get";
            request.KeepAlive = true;
            request.ContentType = "application/json";
            request.Headers.Add("Authorization",fullToken);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string myResponse = "";
            using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
            {
                myResponse = sr.ReadToEnd();
            }
            Console.WriteLine(myResponse);
        }
    }
}
