namespace WorkShopUI.Clients.Domain
{
    public class CarBrand
    {
        public string Name { get; set; }
        public string description { get; set; }
        public string Active { get; set; }
        public IEnumerable<Link> Links { get; set; }
    }
}