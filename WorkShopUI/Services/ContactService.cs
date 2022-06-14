using Google.Cloud.Firestore;
using LanguageExt;
using Typesense;
using WorkShopUI.Clients;
using WorkShopUI.Clients.Domain;
using WorkShopUI.Domain;
using WorkShopUI.Transformers;

namespace WorkShopUI.Services
{
    public class ContactService
    {
        private const string CollectionName = "contacts";

        private readonly ILogger _logger;

        private readonly ContactClient _contactClient;

        private readonly ITypesenseClient _typesenseClient;

        private readonly FirestoreDb _firestoreDb;

        public ContactService(ILogger<ContactService> logger, 
                              ContactClient contactClient,
                              ITypesenseClient typesenseClient,
                              FirestoreDb firestoreDb)
        {
            _logger = logger;
            _contactClient = contactClient;
            _typesenseClient = typesenseClient;
            _firestoreDb = firestoreDb;
        }

        public Either<string, IEnumerable<ContactView>> Search(ContactSearchView searchView) {

            try {
                var query = new SearchParameters(searchView.Text, "code, name");
                query.FilterBy = $"active:ACTIVE && type:{searchView.Type}";
                query.SortBy = "name:asc";
                query.LimitHits = searchView.Size.ToString();

                var search = _typesenseClient.Search<Contact>(CollectionName, query)
                    .Result;

                return search.Hits
                    .Select(hit => ContactTransformer.ToView(hit.Document))
                    .ToList();

                // var search = _sen

                // var searchResult = _contactClient.Search(searchView.Active, searchView.Name, searchView.Code, searchView.Type, 
                //     searchView.Page, searchView.Size);

                // return new PagedView<ContactView>
                // {
                //     Pageable = ContactTransformer.BuildPageable(searchResult),
                //     Content = searchResult.Content 
                //                 .Select(ContactTransformer.ToView)
                //                 .ToList()
                // };
            }
            catch (Exception exception) 
            {
                _logger.LogError(exception, "Unable to perform search");
                return "No es posible realizar la búsqueda de Contactos";
            }
            
        }

        public Either<string, ContactView> Add(ContactView contactView)
        {
            try
            {
                var Id = Guid.NewGuid()
                    .ToString();

                var model = ContactTransformer.ToModel(contactView);               

                var docRef = _firestoreDb.Collection(CollectionName)
                    .Document(Id);

                docRef.SetAsync(model)
                    .Wait();

                contactView.Id = Id;
                _typesenseClient.UpsertDocument<ContactView>(CollectionName, contactView)
                    .Wait();

                return contactView;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unable to add new contact");
                return $"No es posible agregar el contacto de: {contactView.Name}";
            }
        }

        public Option<ContactView> FindById(string id)
        {
            try
            {
                return _contactClient.FindById(id)
                    .Map(ContactTransformer.ToView);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Unable to find Contact with id: {id}");
                return null;
            }
        }
    }
}