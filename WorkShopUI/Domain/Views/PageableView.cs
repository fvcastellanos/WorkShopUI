namespace WorkShopUI.Domain.Views
{
    public class PageableView
    {
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalElements { get; set; }
        public bool First { get; set; }
        public bool Last { get; set; }

        public int NumberOfElements { get; set;}
    }
}