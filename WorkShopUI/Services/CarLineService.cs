using Google.Cloud.Firestore;
using LanguageExt;
using Typesense;
using WorkShopUI.Domain.Model;
using WorkShopUI.Domain.Views;
using WorkShopUI.Transformers;

namespace WorkShopUI.Services
{
    public class CarLineService : ServiceBase
    {
        private const string CollectionName = "car-lines";

        private readonly ILogger _logger;

        private readonly FirestoreDb _firestoreDb;

        private readonly ITypesenseClient _typesenseClient;


        public CarLineService(ILogger<CarLineService> logger, 
                              FirestoreDb firestoreDb,
                              ITypesenseClient typesenseClient)
                              : base(logger, firestoreDb, typesenseClient)
        {
            _logger = logger;
            _firestoreDb = firestoreDb;
            _typesenseClient = typesenseClient;
        }

        public async Task<Either<string, IEnumerable<CarLineView>>> SearchAsync(string carBrandId, SearchView searchView)
        {           
            try
            {
                var query = new SearchParameters(searchView.Name, "name");
                query.FilterBy = $"carBrandId:{carBrandId} && active:ACTIVE";
                query.SortBy = "name:asc";
                query.LimitHits = searchView.Size.ToString();

                return await SearchAsync<CarLineView>(CollectionName, query);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unable to search for car lines");
                return "No es posible realizar la búsqueda de lineas de vehículos";
            }
        }

        public async Task<Either<string, CarLineView>> AddAsync(CarLineView carLineView)
        {
            try
            {
                var id = Guid.NewGuid()
                    .ToString();

                var model = new CarLine
                {
                    Name = carLineView.Name,
                    Description = carLineView.Description,
                    CarBrandId = carLineView.CarBrandId,
                    Active = "ACTIVE",
                    Tenant = GetTenant()
                };

                carLineView.Id = id;
                carLineView.Tenant = model.Tenant;

                await AddToFireStoreAsync<CarLine>(CollectionName, id, model);
                await UpdateSearchIndexAsync<CarLineView>(CollectionName, carLineView);

                return carLineView;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unable to add a car line");
                return "No es posible agregar una nueva línea de vehículo";
            }
        }

        public async Task<Option<CarLineView>> FindByIdAsync(string lineId)
        {
            try
            {
                var snapshot = await FindByIdAsync(CollectionName, lineId);

                if (snapshot.Exists)
                {
                    var carLine = snapshot.ConvertTo<CarLine>();
                    return CarLineTransformer.ToView(carLine);
                }

                return null;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Unable to retrieve car_line_id={lineId}");
                return null;
            }
        }

        public async Task<Either<string, CarLineView>> UpdateAsync(CarLineView carLineView)
        {
            try
            {
                var snapshot = await FindByIdAsync(CollectionName, carLineView.Id);

                if (snapshot.Exists)
                {
                    _logger.LogInformation($"Update Car Line with id: {carLineView.Id}");

                    var model = new CarLine
                    {
                        Name = carLineView.Name,
                        Description = carLineView.Description,
                        Active = carLineView.Active
                    };

                    await AddToFireStoreAsync<CarLine>(CollectionName, carLineView.Id, model);
                    await UpdateSearchIndexAsync<CarLineView>(CollectionName, carLineView);
                }

                return carLineView;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unable to update car_line_id={0}", carLineView.Id);
                return "No es posible actualizar la línea del vehículo";
            }
        }
    }
}