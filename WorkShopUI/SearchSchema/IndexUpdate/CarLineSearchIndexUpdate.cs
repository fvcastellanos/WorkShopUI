using Google.Cloud.Firestore;
using Typesense;
using WorkShopUI.Domain.Model;
using WorkShopUI.Domain.Views;
using WorkShopUI.Transformers;

namespace WorkShopUI.SearchSchema.IndexUpdate
{
    public class CarLineSearchIndexUpdate : ISearchIndexUpdate
    {
        private readonly ITypesenseClient _typesenseClient;
        
        public CarLineSearchIndexUpdate(ITypesenseClient typesenseClient)
        {
            _typesenseClient = typesenseClient;
        }

        public async Task UpdateIndexAsync(string collectionName, DocumentSnapshot documentSnapshot)
        {
            var model = documentSnapshot.ConvertTo<CarLine>();
            var view = CarLineTransformer.ToView(model);

            await _typesenseClient.UpsertDocument<CarLineView>(collectionName, view);
        }
    }
}