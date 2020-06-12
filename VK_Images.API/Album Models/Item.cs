using System.Collections.Generic;

namespace VK_Images.API.Album_Models
{
    public class Item
    {
        public int id { get; set; }
        public long album_id { get; set; }
        public long owner_id { get; set; }
        public long user_id { get; set; }
        public IEnumerable<Size> sizes { get; set; }
        public string text { get; set; }
        public string date { get; set; }
        public bool has_tags { get; set; }
    }
}
