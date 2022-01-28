using AppName.DataCenter.Client.Models;

namespace AppName.DataCenter.Client.Shared
{
    public partial class AudioPlayerModal
    {
        private AudioInfo Audio { get; set; }

        protected override void OnInitialized()
        {
            Audio = Options;
            base.OnInitialized();
        }
    }
}
