using System.Linq;
using LanguageExt;
using WorkShopUI.Clients;
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

        public Either<string, IEnumerable<CarBrandView>> Search(SearchView searchView)
        {
            try
            {
                var searchResult = _carBrandClient.Search(searchView.Active, searchView.Name, searchView.Page, searchView.Size);

                return searchResult.Content
                    .Select(carBrand => {

                        return new CarBrandView
                        {
                            Name = carBrand.Name,
                            Description = carBrand.description,
                            Active = "ACTIVE".Equals(carBrand.Active) ? 1 : 0
                        };
                    }).ToList();

            }
            catch (Exception exception)
            {
                _logger.LogError("can't get product list - {0}", exception.Message);
                return "No se pueden obtener las marcas de vehiculos";                
            }
        }

    }
}