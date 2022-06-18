using Google.Cloud.Firestore;
using Typesense;
using WorkShopUI.Clients.Domain;
using WorkShopUI.Domain;
using WorkShopUI.Transformers;

namespace WorkShopUI.SearchSchema.IndexUpdate
{
    public class CarBrandSearchIndexUpdate : ISearchIndexUpdate
    {
        private readonly ITypesenseClient _typesenseClient;
        
        public CarBrandSearchIndexUpdate(ITypesenseClient typesenseClient)
        {
            _typesenseClient = typesenseClient;
        }

        public async Task UpdateIndexAsync(string collectionName, DocumentSnapshot documentSnapshot)
        {
            var model = documentSnapshot.ConvertTo<CarBrand>();
            var view = CarBrandTransformer.ToView(model);

            await _typesenseClient.UpsertDocument<CarBrandView>(collectionName, view);
        }
    }
}