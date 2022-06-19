using Google.Cloud.Firestore;
using LanguageExt;
using Typesense;
using WorkShopUI.Domain.Model;
using WorkShopUI.Domain.Views;
using WorkShopUI.Transformers;

namespace WorkShopUI.Services
{
    public class ContactService : ServiceBase
    {
        private const string CollectionName = "contacts";

        private readonly ILogger _logger;

        private readonly ITypesenseClient _typesenseClient;

        private readonly FirestoreDb _firestoreDb;

        public ContactService(ILogger<ContactService> logger, 
                              ITypesenseClient typesenseClient,
                              FirestoreDb firestoreDb)
                              : base(logger, firestoreDb, typesenseClient)
        {
            _logger = logger;
            _typesenseClient = typesenseClient;
            _firestoreDb = firestoreDb;
        }

        public Either<string, IEnumerable<ContactView>> Search(ContactSearchView searchView) {

            try {
                var query = new SearchParameters(searchView.Text, "code, name");
                query.FilterBy = $"active:ACTIVE && type:{searchView.Type}";
                query.SortBy = "name:asc";
                query.LimitHits = searchView.Size.ToString();

                return Search<ContactView>(CollectionName, query);
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
                if (findByCode(contactView.Code) != null)
                {
                    _logger.LogError("Code: {0} already exists for tenant: {1}", contactView.Code, GetTenant());
                    return $"El código: {contactView.Code} ya existe";
                }

                var id = BuildGuid();

                var model = new Contact
                {
                    Code = contactView.Code,
                    Name = contactView.Name,
                    Description = contactView.Description,
                    TaxId = contactView.TaxId,
                    ContactName = contactView.Contact,
                    Type = contactView.Type,
                    Active = "ACTIVE",
                    Tenant = GetTenant()
                };

                contactView.Id = id;

                AddToFireStore<Contact>(CollectionName, id, model);
                UpdateSearchIndex<ContactView>(CollectionName, contactView);

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
                var snapshot = FindById(CollectionName, id);

                if (snapshot.Exists)
                {
                    var contact = snapshot.ConvertTo<Contact>();
                    return ContactTransformer.ToView(contact);
                }

                _logger.LogInformation("Contact with id: {0}  not found", id);
                return null;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Unable to find Contact with id: {id}");
                return null;
            }
        }

        private Contact findByCode(string code)
        {
            var reference = FirestoreDb.Collection(CollectionName);
            var query = reference.WhereEqualTo("code", code)
                .WhereEqualTo("tenant", GetTenant());
                
            var snapshot = query.GetSnapshotAsync()
                .Result;

            var contact = snapshot.FirstOrDefault();

            return contact != null ? contact.ConvertTo<Contact>()
                : null;
        }
    }
}