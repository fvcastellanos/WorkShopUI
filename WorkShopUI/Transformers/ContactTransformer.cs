using WorkShopUI.Domain.Model;
using WorkShopUI.Domain.Views;

namespace WorkShopUI.Transformers
{
    public class ContactTransformer
    {
        public static ContactView ToView(Contact contact)
        {
            return new ContactView
            {
                Id = contact.Id,
                Code = contact.Code,
                Description = contact.Description,
                Name = contact.Name,
                Contact = contact.ContactName,
                TaxId = contact.TaxId,
                Active = contact.Active,
                Type = contact.Type,
                Tenant = contact.Tenant
            };
        }
    }
}