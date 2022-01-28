using System.Text;
using System.Text.Encodings.Web;

namespace AppName.DataCenter.Client.Helpers
{
    public static class UrlHelper
    {
        public const string AccountAPI = "api/Account";
        
        public static string BuildHttpGetUrl(string uri, IEnumerable<KeyValuePair<string, string>> queryParams)
        {
            StringBuilder sb = new(uri, 16);
            sb.Append('?');
            foreach (var pair in queryParams)
            {
                if (!string.IsNullOrEmpty(pair.Value))
                {
                    sb.Append(pair.Key)
                        .Append('=')
                        .Append(UrlEncoder.Default.Encode(pair.Value))
                        .Append('&');
                }
            }
            return sb.ToString(0, sb.Length - 1);
        }
    }
}