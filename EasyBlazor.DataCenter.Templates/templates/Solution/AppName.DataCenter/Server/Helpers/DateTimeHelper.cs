namespace AppName.DataCenter.Server.Helpers
{
    internal static class DateTimeHelper
    {
        public static bool Convert2LocalDateTime(long? ts, out DateTime date)
        {
            if (ts > 0)
            {
                date = DateTimeOffset.FromUnixTimeSeconds(ts.Value).LocalDateTime;
                return true;
            }
            date = default;
            return false;
        }
    }
}