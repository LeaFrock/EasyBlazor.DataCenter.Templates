using AppName.DataCenter.Client.Models;

namespace AppName.DataCenter.Client.Pages
{
    public partial class Home
    {
        private HomePageContent[] _contentList = Array.Empty<HomePageContent>();

        protected override async Task OnInitializedAsync()
        {
            _contentList = await UIDataService.LoadHomePageContentsAsync();
        }
    }
}