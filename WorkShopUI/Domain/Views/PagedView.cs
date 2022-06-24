namespace WorkShopUI.Domain.Views
{
    public class PagedView<T>
    {

        public PageableView Pageable { get; set; }
        public IEnumerable<T> Content { get; set; }
    }
}