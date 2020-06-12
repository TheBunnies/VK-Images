namespace VK_Images.API.Wall_Models
{
    public class Profile
    {
        public long id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public bool is_closed { get; set; }
        public bool can_access_closed { get; set; }
        public int sex { get; set; }
        public string screen_name { get; set; }
        public string photo_50 { get; set; }
        public string photo_100 { get; set; }
        public int online { get; set; }
    }

}
