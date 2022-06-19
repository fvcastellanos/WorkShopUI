using Typesense;
using WorkShopUI.SearchSchema.IndexUpdate;

namespace WorkShopUI.SearchSchema
{
    public class IndexUpdateProvider
    {
        private readonly IDictionary<string, ISearchIndexUpdate> _searchIndexUpdateDictionary;
        
        private readonly ITypesenseClient _typesenseClient;

        public IndexUpdateProvider(ITypesenseClient typesenseClient)
        {
            _typesenseClient = typesenseClient;
            _searchIndexUpdateDictionary = buildSearchIndexUpdateDictionary();
        }

        public ISearchIndexUpdate GetSearchIndexUpdate(string collectionName)
        {
            return _searchIndexUpdateDictionary[collectionName];
        }

        // --------------------------------------------------------------------------------------------------

        private IDictionary<string, ISearchIndexUpdate> buildSearchIndexUpdateDictionary()
        {
            return new Dictionary<string, ISearchIndexUpdate>
            {
                { "car-brands", new CarBrandSearchIndexUpdate(_typesenseClient) },
                { "car-lines", new CarLineSearchIndexUpdate(_typesenseClient) },
                { "contacts", new ContactSearchIndexUpdate(_typesenseClient) },
            };
        }
    }
}
