namespace AppName.DataCenter.Client.Models
{
    public class HomePageContent
    {
        public string Title { get; set; }

        public HomePageContentItem[] Items { get; set; }

        public bool Hide { get; set; }
    }

    public sealed class HomePageContentItem
    {
        public string Name { get; set; }

        public string Route { get; set; }

        public bool Disabled { get; set; }
    }
}