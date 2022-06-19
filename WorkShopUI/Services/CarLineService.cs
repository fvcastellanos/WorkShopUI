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

        public Either<string, IEnumerable<CarLineView>> Search(string carBrandId, SearchView searchView)
        {           
            try
            {
                var query = new SearchParameters(searchView.Name, "name");
                query.FilterBy = $"carBrandId:{carBrandId} && active:ACTIVE";
                query.SortBy = "name:asc";
                query.LimitHits = searchView.Size.ToString();

                return Search<CarLineView>(CollectionName, query);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unable to search for car lines");
                return "No es posible realizar la búsqueda de lineas de vehículos";
            }
        }

        public Either<string, CarLineView> Add(CarLineView carLineView)
        {
            try
            {
                var id = Guid.NewGuid()
                    .ToString();

                var model = CarLineTransformer.ToModel(carLineView);

                var docRef = _firestoreDb.Collection(CollectionName)
                    .Document(id);

               docRef.SetAsync(model)
                    .Wait();
                
                carLineView.Id = id;
                _typesenseClient.UpsertDocument<CarLineView>(CollectionName, carLineView)
                    .Wait();

                return carLineView;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unable to add a car line");
                return "No es posible agregar una nueva línea de vehículo";
            }
        }

        public Option<CarLineView> FindById(string lineId)
        {
            try
            {
                var docRef = _firestoreDb.Collection(CollectionName)
                    .Document(lineId);

                var snapshot = docRef.GetSnapshotAsync()
                    .Result;

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

        public Either<string, CarLineView> Update(CarLineView carLineView)
        {
            try
            {
                var docRef = _firestoreDb.Collection(CollectionName)
                    .Document(carLineView.Id);

                var snapshot = docRef.GetSnapshotAsync()
                    .Result;

                if (snapshot.Exists)
                {
                    var model = CarLineTransformer.ToModel(carLineView);
                    docRef.SetAsync(model)
                        .Wait();

                    _typesenseClient.UpsertDocument<CarLineView>(CollectionName, carLineView)
                        .Wait();
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