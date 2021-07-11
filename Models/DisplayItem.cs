using System;

namespace BiDegree.Models
{
    public class DisplayItem
    {
        public int ItemNumber { get; set; }
        public string SourceUrl { get; set; }
        public string Title { get; set; }

        [Obsolete(message:"Replaced with DisplayItemType")]
        public bool IsVideo { get; set; }

        public DisplayItemType ItemType { get; set; } = DisplayItemType.Image;
    }

    public enum DisplayItemType
    {
        Image = 0,
        Video = 1,
        Weather = 2
    }
}
