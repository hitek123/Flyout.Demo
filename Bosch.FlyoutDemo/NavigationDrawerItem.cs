using Android.Nfc;

namespace Bosch.FlyoutDemo
{
    public class NavigationDrawerItem
    {

        public NavigationDrawerItem()
        {
        }
        
        public NavigationDrawerItem(string title, string tag, int imageId)
            : this (title, tag, imageId, false)
        {
        }

        public NavigationDrawerItem(string title, string tag, int imageId, bool isCounterVisible)
        {
            Title = title;
            Tag = tag;
            ImageId = imageId;
            IsCounterVisible = isCounterVisible;
        }

        public bool IsCounterVisible { get; set; }

        public int ImageId { get; set; }

        public string Tag { get; set; }

        public string Title { get; set; }

        public string Count { get; set; }
    }
}