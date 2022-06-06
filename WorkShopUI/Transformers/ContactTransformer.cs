using WorkShopUI.Clients.Domain;
using WorkShopUI.Domain;

namespace WorkShopUI.Transformers
{
    public class ContactTransformer : BaseTransformer
    {
        public static ContactView ToView(Contact contact)
        {
            return new ContactView
            {
                Id = GetId(contact),
                Code = contact.Code,
                Description = contact.Description,
                Name = contact.Name,
                Contact = contact.ContactName,
                TaxId = contact.TaxId,
                Active = contact.Active,
                Type = contact.Type
            };
        }

        public static Contact ToModel(ContactView contactView)
        {
            return new Contact
            {
                Code = contactView.Code,
                Name = contactView.Name,
                ContactName = contactView.Contact,
                Description = contactView.Description,
                TaxId = contactView.TaxId,
                Type = contactView.Type,
                Active = contactView.Active
            };
        }
    }
}