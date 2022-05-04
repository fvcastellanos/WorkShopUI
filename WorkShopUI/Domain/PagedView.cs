namespace WorkShopUI.Domain
{
    public class PagedView<T>
    {

        public PageableView Pageable { get; set; }
        public IEnumerable<T> Content { get; set; }
    }
}