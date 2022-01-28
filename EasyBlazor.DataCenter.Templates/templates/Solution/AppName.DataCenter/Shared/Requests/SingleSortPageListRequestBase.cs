namespace AppName.DataCenter.Shared.Requests
{
    public class SingleSortPageListRequestBase : PageListRequestBase
    {
        public string SortType { get; set; }

        public bool Desc { get; set; }
    }
}