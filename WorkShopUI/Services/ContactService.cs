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

        public async Task<Either<string, IEnumerable<ContactView>>> SearchAsync(ContactSearchView searchView) {

            try {
                var query = new SearchParameters(searchView.Text, "code, name");
                query.FilterBy = $"active:{searchView.Active} && type:{searchView.Type} && tenant: {GetTenant()}";
                query.SortBy = "name:asc";
                query.LimitHits = searchView.Size.ToString();

                return await SearchAsync<ContactView>(CollectionName, query);
            }
            catch (Exception exception) 
            {
                _logger.LogError(exception, "Unable to perform search");
                return "No es posible realizar la búsqueda de Contactos";
            }            
        }

        public async Task<Either<string, ContactView>> AddAsync(ContactView contactView)
        {
            try
            {
                if (await findByCodeAsync(contactView.Code) != null)
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
                contactView.Tenant = model.Tenant;

                await AddToFireStoreAsync<Contact>(CollectionName, id, model);
                await UpdateSearchIndexAsync<ContactView>(CollectionName, contactView);

                return contactView;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unable to add new contact");
                return $"No es posible agregar el contacto de: {contactView.Name}";
            }
        }

        public async Task<Option<ContactView>> FindByIdAsync(string id)
        {
            try
            {
                var snapshot = await FindByIdAsync(CollectionName, id);

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

        public async Task<Either<string, ContactView>> UpdateAsync(ContactView contactView)
        {
            try
            {
                var snapshot = await FindByIdAsync(CollectionName, contactView.Id);

                if (!snapshot.Exists)
                {
                    _logger.LogError($"Contact Id: {contactView.Id} not found for tenant: {contactView.Tenant}");
                    return $"No se encontro el Contacto con Id: {contactView.Id}";
                }

                var contact = snapshot.ConvertTo<Contact>();

                if (!contactView.Code.Equals(contact.Code))
                {
                    var existingContact = await findByCodeAsync(contactView.Code);
                    
                    if ((existingContact != null) && (existingContact.Id != contact.Id))
                    {
                        _logger.LogError($"Code: {contactView.Code} is already used by other contact for tenant: {contactView.Tenant}");
                        return $"El codigo: {contact.Code} ya se encuentra en uso";
                    }
                }

                contact.Name = contactView.Name;
                contact.Description = contactView.Description;
                contact.Code = contactView.Code;
                contact.ContactName = contactView.Contact;
                contact.TaxId = contactView.TaxId;
                contact.Active = contactView.Active;

                await AddToFireStoreAsync<Contact>(CollectionName, contactView.Id, contact);
                await UpdateSearchIndexAsync<ContactView>(CollectionName, contactView);

                return contactView;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Unable to update Contact with id: {contactView.Id} and tenant: {contactView.Tenant}");
                return $"No es posible actualizar el Contacto de: {contactView.Name}";
            }
        }

        private async Task<Contact> findByCodeAsync(string code)
        {
            var reference = FirestoreDb.Collection(CollectionName);
            var query = reference.WhereEqualTo("code", code)
                .WhereEqualTo("tenant", GetTenant());
                
            var snapshot = await query.GetSnapshotAsync();

            var contact = snapshot.FirstOrDefault();

            return contact != null ? contact.ConvertTo<Contact>()
                : null;
        }
    }
}