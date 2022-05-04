namespace WorkShopUI.Domain
{
    public class PageableView
    {
        public bool Paged { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalElements { get; set; }
        public bool First { get; set; }
        public bool Last { get; set; }
        public int PageNumber { get; set; }
    }
}