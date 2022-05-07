using System.Linq;
using LanguageExt;
using WorkShopUI.Clients;
using WorkShopUI.Clients.Domain;
using WorkShopUI.Domain;
using WorkShopUI.Transformers;

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
                    Pageable = CarBrandTransformer.BuildPageable(searchResult),
                    Content = searchResult.Content
                                .Select(CarBrandTransformer.ToView)
                                .ToList()
                };
            }
            catch (Exception exception)
            {
                _logger.LogError("can't get product list - {0}", exception.Message);
                return "No se pueden obtener las marcas de vehiculos";                
            }
        }

        public Either<string, CarBrandView> Add(CarBrandView carBrandView)
        {
            try
            {
                var model = CarBrandTransformer.ToModel(carBrandView);
                var carBrand = _carBrandClient.Add(model);
                return CarBrandTransformer.ToView(carBrand);
            }
            catch (Exception exception)
            {
                _logger.LogError("can't create new car brand with name={0} - {1}", carBrandView.Name, exception.Message);
                return $"No es posible agregar el fabricante: {carBrandView.Name}";
            }
        }

        public Option<CarBrandView> FindById(string id)
        {
            try
            {
                return _carBrandClient.FindById(id)
                    .Map(CarBrandTransformer.ToView)
                    .FirstOrDefault();
            }
            catch (Exception exception)
            {
                _logger.LogError("unable to retrieve car_brand_id={0} - {1}", id, exception.Message);
                return null;
            }
        }

        public Either<string, CarBrandView> Update(CarBrandView view)
        {
            try
            {
                var model = CarBrandTransformer.ToModel(view);
                _carBrandClient.Update(view.Id, model);

                return view;
            }
            catch (Exception exception)
            {
                _logger.LogError("unable to update car_brand_id={0} - {1}", view.Id, exception.Message);
                return "No es posible actualizar la informacion del fabricante";
            }
        }
    }
}