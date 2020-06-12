using System.Collections.Generic;

namespace VK_Images.API.Album_Models
{
    public class Response
    {
        public int count { get; set; }
        public IEnumerable<Item> items { get; set; }
    }
}
