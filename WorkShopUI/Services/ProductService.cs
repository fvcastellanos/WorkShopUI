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

        public Either<string, ProductView> Add(ProductView productView)
        {
            try
            {
                var model = ProductTransformer.ToModel(productView);                
                _productClient.Add(model);

                return productView;
            }
            catch (HttpRequestException httpRequestException)
            {
                _logger.LogError(httpRequestException, $"Unable to add a new product with id: {productView.Id}");
                return httpRequestException.Message;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unable to add a new product");
                return "No es posible agregar el producto";
            }
        }

        public Option<ProductView> FindById(string id)
        {
            try
            {
                return _productClient.FindById(id)
                    .Map(ProductTransformer.ToView);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Unable to find product with id: {id}");
                return null;
            }
        }

        public Either<string, ProductView> Update(ProductView productView)
        {
            try
            {
                var model = ProductTransformer.ToModel(productView);
                _productClient.Update(productView.Id, model);

                return productView;
            }
             catch (HttpRequestException httpRequestException)
            {
                _logger.LogError(httpRequestException, $"Unable to update product with id: {productView.Id}");
                return httpRequestException.Message;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unable to update product");
                return "No es posible actualizar el producto";
            }
       }
    }
}