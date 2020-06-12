using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MoreLinq;
using Newtonsoft.Json;
using VK_Images.API.Album_Models;

namespace VK_Images.API
{
    public class Album : IDataService
    {
        private readonly HttpClient _client;
        public Album()
        {
            _client = new HttpClient();
        }

        public async Task<IEnumerable<string>> GetImagesAsync(string accessToken, string id, int count, int offset = 0)
        {

            string request =
                $"https://api.vk.com/method/photos.getAll?owner_id={id}&access_token={accessToken}&v=5.125&offset={offset}&count={count}";

            var response = (await _client.GetAsync(request)).EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<Data>(responseBody);

            if (data.response == null)
                return new List<string>();

            var images = new List<string>();

            foreach (var item in data.response.items)
            {
                if (item.sizes != null)
                    images.Add(item.sizes.MaxBy(x => x.height).First().url);
            }

            return images;
        }

        public async Task<UserData> GetInfoAsync(string accessToken, string id)
        {
            string request =
                $"https://api.vk.com/method/photos.getAll?owner_id={id}&access_token={accessToken}&v=5.125&offset=0&count=1";

            var response = (await _client.GetAsync(request)).EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<Data>(responseBody);

            if (data.response == null)
                return new UserData();

            return new UserData {id = data.response.items.First().owner_id, AvatarUrl = data.response.items.First().sizes.MaxBy(x => x.height).First().url};
        }
    }
}
