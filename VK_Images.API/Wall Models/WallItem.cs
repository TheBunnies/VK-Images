using System.Collections.Generic;

namespace VK_Images.API.Wall_Models
{
    public class WallItem
    {
        public long id { get; set; }
        public long form_id { get; set; }
        public long owner_id { get; set; }
        public long date { get; set; }
        public int marked_as_ads { get; set; }
        public string post_type { get; set; }
        public string text { get; set; }
        public int is_pinned { get; set; }
        public IEnumerable<Attachments> attachments { get; set; }
        public PostSource post_source { get; set; }
        public Comments comments { get; set; }
        public Likes likes { get; set; }
        public Reposts reposts { get; set; }
        public Views views { get; set; }
        public bool is_favorite { get; set; }
    }
}
