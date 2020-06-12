using System.Collections.Generic;
using VK_Images.API.Album_Models;

namespace VK_Images.API.Wall_Models
{
    public class Photo
    {
        public int album_id { get; set; }
        public long date { get; set; }
        public long id { get; set; }
        public long owner_id { get; set; }
        public bool has_tags { get; set; }
        public string access_key { get; set; }
        public IEnumerable<Size> sizes { get; set; }
    }
}
