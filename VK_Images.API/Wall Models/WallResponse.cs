using System.Collections.Generic;

namespace VK_Images.API.Wall_Models
{
    public class WallResponse
    {
        public int count { get; set; }
        public IEnumerable<WallItem> items { get; set; }
        public IEnumerable<Profile> profiles { get; set; }
        public IEnumerable<Group> groups { get; set; }
    }
}
