using AntDesign.TableModels;

namespace AppName.DataCenter.Client.Extensions
{
    internal static class AntDesignExtension
    {
        public static (string orderBy, bool isDesc) SingleSortOrder<T>(this QueryModel<T> query)
        {
            foreach (var item in query.SortModel)
            {
                if (item.Sort is not null)
                {
                    return (item.FieldName, item.Sort == "descend");
                }
            }
            return default;
        }
    }
}