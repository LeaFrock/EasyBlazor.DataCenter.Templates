using AntDesign;

namespace AppName.DataCenter.Client.Helpers
{
    internal static class ConstantHelper
    {
        public static SortDirection[] DefaultSortDirections { get; } = new SortDirection[] { SortDirection.Descending, SortDirection.Ascending, SortDirection.Descending };

        public static string ToolTip4AccurateMatch(string label) => "精确匹配" + label;

        public static string ToolTip4KeywordMatch(string label) => "关键词匹配" + label;

        public static string ToolTip4PrefixMatch(string label) => "匹配以填写内容开头的" + label;

        public static string ToolTip4PrefixMatch(string label, string extra) => $"匹配以填写内容开头的{label}。{extra}";

        public static string SimplifyProvince(string province)
        {
            string p = province?.Trim();
            if (string.IsNullOrEmpty(p))
            {
                return p;
            }
            if (p.EndsWith('省') || p.EndsWith('市'))
            {
                return p[0..^1];
            }
            return p;
        }

        public static string SimplifyCity(string city)
        {
            string c = city?.Trim();
            if (string.IsNullOrEmpty(c))
            {
                return c;
            }
            if (c.EndsWith('市'))
            {
                return c[0..^1];
            }
            return c;
        }
    }
}