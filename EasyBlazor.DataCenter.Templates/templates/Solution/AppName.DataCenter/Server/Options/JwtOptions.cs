namespace AppName.DataCenter.Server.Options
{
    public class JwtOptions
    {
        public string Key { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int ExpiryInHours { get; set; }
    }
}