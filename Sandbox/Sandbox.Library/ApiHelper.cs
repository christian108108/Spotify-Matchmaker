using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Sandbox.Library
{
    public class ApiHelper
    {
        public static HttpClient ApiClient { get; set; }

        public static void InitializeClient()
        {
            ApiClient = new HttpClient();
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer BQCSuG3yxTwpBCQVCY-CSVEiuo6SoKMBU_dEy5K_M_xHu6BO_lzkwHMRr4vHpwDPBOKiMU1eBsWmLuc1_CplxoyPdGjMBPB3-hS4a16PeeUlC9W5EhdYnfxjR77ccJyxonRUcnGXCBcoMz7_olVUMy2ejkExA8TyCm6K1dc-PH7yt40MbVtPEPWBRUMZEtz57qAK2gsLqZ8S6Jkf8VvzP7DbE9tmAOmbfXVUcCaCgZdp0A4nG23-VgkXRUCaVFKpDw");
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }

}
