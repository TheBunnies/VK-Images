using System.Collections.Generic;
using System.Threading.Tasks;

namespace VK_Images.API
{
    public interface IDataService
    {
        Task<IEnumerable<string>> GetImagesAsync(string accessToken, string id, int count, int offset = 0);
        Task<UserData> GetInfoAsync(string accessToken, string id);
    }
}
