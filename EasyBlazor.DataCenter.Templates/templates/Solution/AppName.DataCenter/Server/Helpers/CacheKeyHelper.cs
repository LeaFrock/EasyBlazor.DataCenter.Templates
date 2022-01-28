namespace AppName.DataCenter.Server.Helpers
{
    internal static class CacheKeyHelper
    {
        public static string GetModNameKeyIdCacheKey(int userId) => nameof(GetModNameKeyIdCacheKey) + userId.ToString();
    }
}