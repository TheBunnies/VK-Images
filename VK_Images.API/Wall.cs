using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MoreLinq;
using Newtonsoft.Json;
using VK_Images.API.Wall_Models;

namespace VK_Images.API
{
    public class Wall : IDataService
    {
        private readonly HttpClient _client;

        public Wall()
        {
            _client = new HttpClient();
        }

        public async Task<IEnumerable<string>> GetImagesAsync(string accessToken, string id, int count, int offset = 0)
        {
            string request =
                $"https://api.vk.com/method/wall.get?owner_id={id}&access_token={accessToken}&v=5.110&offset={offset}&count={count}&extended=1";

            var response = (await _client.GetAsync(request)).EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<WallData>(responseBody);

            if (data.response == null)
                return new List<string>();

            var images = new List<string>();

            foreach (var item in data.response.items)
            {
                if (item.attachments != null)
                {
                    foreach (var attachment in item.attachments)
                    {
                        if (attachment.photo != null)
                            images.Add(attachment.photo.sizes.MaxBy(x => x.height).First().url);
                    }
                }
            }

            return images;
        }

        public async Task<UserData> GetInfoAsync(string accessToken, string id)
        {
            string request =
                $"https://api.vk.com/method/wall.get?owner_id={id}&access_token={accessToken}&v=5.110&offset=0&count=1&extended=1";

            var response = (await _client.GetAsync(request)).EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<WallData>(responseBody);

            if (data.response == null)
                return new UserData();

            if (data.response.groups?.Count() <= 0)
            {
                var name = $"{data.response.profiles.First().first_name} {data.response.profiles.First().last_name}";
                var UserId = data.response.profiles.First().id;
                var avatar = data.response.profiles.First().photo_100;

                return new UserData {id = UserId, Name = name, AvatarUrl = avatar};
            }

            else
            {
                var name = data.response.groups.First().name;
                var groupId = data.response.groups.First().id;
                var avatar = data.response.groups.First().photo_100;

                return new UserData {id = groupId, Name = name, AvatarUrl = avatar};
            }
        }
    }
}
