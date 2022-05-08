namespace WorkShopUI.Clients
{
    public class ClientConstants
    {
        public const string ClientName = "workshop-api";
        public const string BasePath = "/v1/workshop";
        public const string CarBrandResource = BasePath + "/car-brands";
        public const string CarLineResource =  CarBrandResource + "/{0}/lines";
        public const string ProductResource = BasePath + "/products";
    }
}