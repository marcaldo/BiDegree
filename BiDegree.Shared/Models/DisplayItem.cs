﻿namespace BiDegree.Shared.Models
{
    public class DisplayItem
    {
        public int ItemNumber { get; set; }
        public string FileName { get; set; } = default!;
        public string SourceUrl { get; set; } = default!;
        public string Title { get; set; } = default!;
        public int Width { get; set; }
        public int Height { get; set; }
        public int Rotation { get; set; }
        public string CssClass { get; set; } = default!;
        public string FileSize { get; set; } = default!;
        public string Orientation
        {
            get
            {
                if (Width > Height && Rotation > 0) { return "contain"; }

                if (Width < Height) { return "contain"; }

                return "cover";
            }
        }
        public DisplayLayer DisplayLayer
        {
            get
            {
                if (ItemNumber % 2 == 0) { return DisplayLayer.Top; }
                return DisplayLayer.Bottom;
            }
        }
        public DisplayItemType ItemType { get; set; } = DisplayItemType.Image;
    }

    public enum DisplayItemType
    {
        Image = 0,
        Video = 1,
        Weather = 2
    }

    public enum DisplayLayer
    {
        Top = 0,
        Bottom = 1
    }

    public enum DisplayWeatherWidgetType
    {
        Standard = 0,
        Extended = 1
    }
}
