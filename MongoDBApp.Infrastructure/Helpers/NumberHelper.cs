using MongoDBApp.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBApp.Infrastructure.Helpers
{
    public class NumberHelper : INumberHelper
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<string> GetRandomAvatar()
        {
            string apiUrl = "https://avatars.dicebear.com/api/male/"; // (male or female)

            string randomId = Guid.NewGuid().ToString();

            string avatarUrl = apiUrl + randomId + ".svg";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(avatarUrl);

                if (response.IsSuccessStatusCode)
                {
                    return avatarUrl;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при отриманні аватари: " + ex.Message);
            }

            return string.Empty;
        }
    }
}
