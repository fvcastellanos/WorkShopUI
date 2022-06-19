using Google.Cloud.Firestore;
using Typesense;
using WorkShopUI.Clients.Domain;
using WorkShopUI.Domain;
using WorkShopUI.Transformers;

namespace WorkShopUI.SearchSchema.IndexUpdate
{
    public class ContactSearchIndexUpdate : ISearchIndexUpdate
    {
        private readonly ITypesenseClient _typesenseClient;

        public ContactSearchIndexUpdate(ITypesenseClient typesenseClient)
        {
            _typesenseClient = typesenseClient;
        }

        public async Task UpdateIndexAsync(string collectionName, DocumentSnapshot documentSnapshot)
        {
            var model = documentSnapshot.ConvertTo<Contact>();
            var view = ContactTransformer.ToView(model);

            await _typesenseClient.UpsertDocument<ContactView>(collectionName, view);
        }
    }
}