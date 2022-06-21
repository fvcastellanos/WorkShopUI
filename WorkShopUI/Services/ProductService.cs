using LanguageExt;
using WorkShopUI.Clients;
using WorkShopUI.Domain.Views;
using WorkShopUI.Transformers;

namespace WorkShopUI.Services
{
    public class ProductService
    {
        private readonly ILogger _logger;

        private readonly ProductClient _productClient;

        public ProductService(ILogger<ProductService> logger, ProductClient productClient)
        {
            _logger = logger;
            _productClient = productClient;
        }

        public Either<string, PagedView<ProductView>> Search(SearchView searchView)
        {
            try
            {
                var searchResult = _productClient.Search(searchView.Active, searchView.Name, searchView.Code, searchView.Type,
                    searchView.Page, searchView.Size);

                return new PagedView<ProductView>
                {
                    Pageable = ProductTransformer.BuildPageable(searchResult),
                    Content = searchResult.Content
                                    .Select(ProductTransformer.ToView)
                                    .ToList()
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unable to perform search");
                return "No es posible realizar la búsqueda";
            }
        }
    }
}