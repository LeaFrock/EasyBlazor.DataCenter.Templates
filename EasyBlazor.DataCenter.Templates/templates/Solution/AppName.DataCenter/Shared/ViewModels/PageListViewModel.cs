namespace AppName.DataCenter.Shared.ViewModels
{
    public class PageListViewModel<T>
    {
        public int Total { get; set; }

        public T[] Items { get; set; }
    }
}