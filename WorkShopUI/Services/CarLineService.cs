using LanguageExt;
using WorkShopUI.Clients;
using WorkShopUI.Domain;
using WorkShopUI.Transformers;

namespace WorkShopUI.Services
{
    public class CarLineService
    {
        private readonly ILogger _logger;
        private readonly CarLineClient _carLineClient;

        public CarLineService(ILogger<CarLineService> logger, CarLineClient carLineClient)
        {
            _carLineClient = carLineClient;
            _logger = logger;
        }

        public Either<string, PagedView<CarLineView>> Search(string carBrandId, SearchView searchView)
        {           
            try
            {
                var searchResult = _carLineClient.Search(carBrandId, searchView.Active, searchView.Name, searchView.Page, searchView.Size);

                return new PagedView<CarLineView>
                {
                    Pageable = CarLineTransformer.BuildPageable(searchResult),
                    Content = searchResult.Content
                                .Select(CarLineTransformer.ToView)
                                .ToList()
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unable to search for car lines");
                return "No es posible realizar la búsqueda de lineas de vehículos";
            }
        }

        public Either<string, CarLineView> Add(string brandId, CarLineView carLineView)
        {
            try
            {
                var model = CarLineTransformer.ToModel(carLineView);
                _carLineClient.Add(brandId, model);

                return carLineView;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unable to add a car line");
                return "No es posible agregar una nueva línea de vehículo";
            }
        }

        public Option<CarLineView> FindById(string brandId, string lineId)
        {
            try
            {
                return _carLineClient.FindById(brandId, lineId)
                    .Map(CarLineTransformer.ToView);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Unable to retrieve car_line_id={lineId} for brand_id={brandId}");
                return null;
            }
        }

        public Either<string, CarLineView> Update(string brandId, CarLineView carLineView)
        {
            try
            {
                var model = CarLineTransformer.ToModel(carLineView);
                _carLineClient.Update(brandId, carLineView.Id, model);

                return carLineView;
            }
            catch (Exception exception)
            {

                _logger.LogError(exception, "Unable to update car_line_id={0} for brand_id={1}", carLineView.Id, brandId);
                return "No es posible actualizar la línea del vehículo";
            }
        }
    }
}