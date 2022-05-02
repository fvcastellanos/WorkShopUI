namespace WorkShopUI.Clients.Domain
{
    public class Pageable
    {
        public int Offset { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool Paged { get; set; }
        public bool Unpaged { get; set; }
    }
}