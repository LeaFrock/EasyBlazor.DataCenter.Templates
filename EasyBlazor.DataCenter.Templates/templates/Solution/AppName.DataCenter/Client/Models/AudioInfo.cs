namespace AppName.DataCenter.Client.Models
{
    public record AudioInfo
    {
        public string FileExtension { get; init; }

        public string FileName { get; init; }

        public string Url { get; init; }
    }
}