﻿namespace VK_Images.API.Wall_Models
{
    public class Group
    {
        public int id { get; set; }
        public string name { get; set; }
        public string screen_name { get; set; }
        public int is_closed { get; set; }
        public string type { get; set; }
        public int is_admin { get; set; }
        public int is_member { get; set; }
        public int is_advertiser { get; set; }
        public string photo_50 { get; set; }
        public string photo_100 { get; set; }
        public string photo_200 { get; set; }
    }
}
