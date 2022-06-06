using LanguageExt;
using WorkShopUI.Clients;
using WorkShopUI.Domain;
using WorkShopUI.Transformers;

namespace WorkShopUI.Services
{
    public class ContactService
    {
        private readonly ILogger _logger;

        private readonly ContactClient _contactClient;

        public ContactService(ILogger<ContactService> logger, ContactClient contactClient)
        {
            _logger = logger;
            _contactClient = contactClient;
        }

        public Either<string, PagedView<ContactView>> Search(ContactSearchView searchView) {

            try {
                
                var searchResult = _contactClient.Search(searchView.Active, searchView.Name, searchView.Code, searchView.Type, 
                    searchView.Page, searchView.Size);

                return new PagedView<ContactView>
                {
                    Pageable = ContactTransformer.BuildPageable(searchResult),
                    Content = searchResult.Content 
                                .Select(ContactTransformer.ToView)
                                .ToList()
                };
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
                var model = ContactTransformer.ToModel(contactView);
                _contactClient.Add(model);

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