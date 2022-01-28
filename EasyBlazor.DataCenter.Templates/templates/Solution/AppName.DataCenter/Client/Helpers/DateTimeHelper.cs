namespace AppName.DataCenter.Client.Helpers
{
    internal static class DateTimeHelper
    {
        public static string Convert2TimestampOfDate(DateTime? date)
        {
            if (!date.HasValue)
            {
                return default;
            }
            long ts = new DateTimeOffset(date.Value.Date).ToUnixTimeSeconds();
            return ts.ToString();
        }
    }
}