using LanguageExt;
using WorkShopUI.Clients;
using WorkShopUI.Domain;
using WorkShopUI.Transformers;
using Google.Cloud.Firestore;
using Typesense;
using WorkShopUI.Clients.Domain;

namespace WorkShopUI.Services
{
    public class CarBrandService
    {
        private const string CollectionName = "car-brands";
        private readonly ILogger _logger;

        private readonly FirestoreDb _firestoreDb;

        private readonly ITypesenseClient _typesenseClient;

        public CarBrandService(ILogger<CarBrandService> logger, 
                               FirestoreDb firestoreDb,
                               ITypesenseClient typesenseClient)
        {
            _logger = logger;
            _firestoreDb = firestoreDb;
            _typesenseClient = typesenseClient;
        }

        public Either<string, IEnumerable<CarBrandView>> Search(SearchView searchView)
        {
            try
            {
                var query = new SearchParameters(searchView.Name, "name");
                query.FilterBy = "active:ACTIVE";
                query.SortBy = "name:asc";
                query.LimitHits = searchView.Size.ToString();

                var search = _typesenseClient.Search<CarBrandView>(CollectionName, query)
                    .Result;

                return search.Hits.Select(hit => hit.Document)
                    .ToList();
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
                var id = Guid.NewGuid()
                    .ToString();

                var model = CarBrandTransformer.ToModel(carBrandView);
                var docRef = _firestoreDb.Collection(CollectionName)
                    .Document(id);
                
                var task = docRef.SetAsync(model)
                    .Result;

                var view = CarBrandTransformer.ToView(id, model);

                _typesenseClient.UpsertDocument<CarBrandView>(CollectionName, view)
                    .Wait();

                return view;
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
                var docRef = _firestoreDb.Collection(CollectionName)
                    .Document(id);

                var snapshot = docRef.GetSnapshotAsync()
                    .Result;
                
                if (snapshot.Exists)
                {
                    var carBrand = snapshot.ConvertTo<CarBrand>();
                    return CarBrandTransformer.ToView(id, carBrand);
                }

                return null;

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

                var docRef = _firestoreDb.Collection(CollectionName)
                    .Document(view.Id);

                var snapshot = docRef.GetSnapshotAsync()
                    .Result;
                
                if (snapshot.Exists)
                {
                    _logger.LogInformation("Update document id: {0}", view.Id);
                    docRef.SetAsync(model)
                        .Wait();

                    _logger.LogInformation("Update document index for id: {0}", view.Id);
                    _typesenseClient.UpdateDocument<CarBrandView>(CollectionName, view.Id, view)
                        .Wait();
                }

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