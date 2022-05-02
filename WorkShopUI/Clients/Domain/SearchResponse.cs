namespace WorkShopUI.Clients.Domain
{
    public class SearchResponse<T>
    {
        public IEnumerable<T> Content { get; set; }
        public bool Last { get; set; }
        public int TotalElements { get; set; }
        public int TotalPages { get; set; }
        public int Size { get; set; }
        public int Number { get; set; }
        public int NumberOfElements { get; set; }
        public bool first { get; set; }
        public bool Empty { get; set; }

        public Pageable Pageable { get; set; }        
    }
}