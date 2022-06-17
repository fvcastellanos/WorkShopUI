using LanguageExt;
using WorkShopUI.Domain;
using WorkShopUI.Transformers;
using Google.Cloud.Firestore;
using Typesense;
using WorkShopUI.Clients.Domain;

namespace WorkShopUI.Services
{
    public class CarBrandService : ServiceBase
    {
        private const string CollectionName = "car-brands";
        private readonly ILogger _logger;

        private readonly FirestoreDb _firestoreDb;

        private readonly ITypesenseClient _typesenseClient;

        public CarBrandService(ILogger<CarBrandService> logger, 
                               FirestoreDb firestoreDb,
                               ITypesenseClient typesenseClient) 
                               : base(logger, firestoreDb, typesenseClient)
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
                query.FilterBy = $"active:ACTIVE && tenant:{GetTenant()}";
                query.SortBy = "name:asc";
                query.LimitHits = searchView.Size.ToString();

                return Search<CarBrandView>(CollectionName, query);
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
                if (findExistingBrand(carBrandView.Name))
                {
                    _logger.LogError("car brand with name={0} already exists", carBrandView.Name);
                    return $"Ya existe una marca {carBrandView.Name}";
                }
                
                var id = BuildGuid();
                var model = new CarBrand
                {
                    Name = carBrandView.Name,
                    Description = carBrandView.Description,
                    Active = "ACTIVE",
                    Tenant = GetTenant()
                };

                var view = CarBrandTransformer.ToView(model);
                view.Id = id;

                AddToFireStore<CarBrand>(CollectionName, id, model);
                UpdateSearchIndex<CarBrandView>(CollectionName, view);

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
                var snapshot = FindById(CollectionName, id);
                
                if (snapshot.Exists)
                {
                    var carBrand = snapshot.ConvertTo<CarBrand>();
                    return CarBrandTransformer.ToView(carBrand);
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
                var snapshot = FindById(CollectionName, view.Id);

                if (snapshot.Exists)
                {
                    _logger.LogInformation("Update document id: {0}", view.Id);
                    var model = new CarBrand
                    {
                        Name = view.Name,
                        Description = view.Description,
                        Active = view.Active,
                        Tenant = GetTenant()
                    };

                    AddToFireStore<CarBrand>(CollectionName, view.Id, model);
                    UpdateSearchIndex<CarBrandView>(CollectionName, view);
                }

                return view;
            }
            catch (Exception exception)
            {
                _logger.LogError("unable to update car_brand_id={0} - {1}", view.Id, exception.Message);
                return "No es posible actualizar la informacion del fabricante";
            }
        }

        // -----------------------------------------------------------------------------------------------

        private bool findExistingBrand(string name)
        {
            var query = new SearchParameters(name, "name");
            query.FilterBy = $"name:{name}";

            var search = _typesenseClient.Search<CarBrand>(CollectionName, query)
                .Result;

            return search.Found > 0;
        }
    }
}