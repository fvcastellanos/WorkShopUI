using System.Linq;
using LanguageExt;
using WorkShopUI.Clients;
using WorkShopUI.Clients.Domain;
using WorkShopUI.Domain;

namespace WorkShopUI.Services
{
    public class CarBrandService
    {
        private readonly ILogger _logger;

        private readonly CarBrandClient _carBrandClient;

        public CarBrandService(ILogger<CarBrandService> logger, CarBrandClient carBrandClient)
        {
            _logger = logger;
            _carBrandClient = carBrandClient;
        }

        public Either<string, PagedView<CarBrandView>> Search(SearchView searchView)
        {
            try
            {
                var searchResult = _carBrandClient.Search(searchView.Active, searchView.Name, searchView.Page, searchView.Size);

                return new PagedView<CarBrandView>
                {
                    Pageable = buildPageable(searchResult),
                    Content = searchResult.Content
                                .Select(carBrand => {

                                    return new CarBrandView
                                    {
                                        Name = carBrand.Name,
                                        Description = carBrand.description,
                                        Active = carBrand.Active
                                    };
                                }).ToList()
                };
            }
            catch (Exception exception)
            {
                _logger.LogError("can't get product list - {0}", exception.Message);
                return "No se pueden obtener las marcas de vehiculos";                
            }
        }

        private PageableView buildPageable(SearchResponse<CarBrand> searchResponse) {

            return new PageableView
            {
                    First = searchResponse.First,
                    Last = searchResponse.Last,
                    TotalPages = searchResponse.TotalPages,
                    TotalElements = searchResponse.TotalElements,
                    PageNumber = searchResponse.Number,
                    PageSize = searchResponse.Size,                
            };
        }

    }
}