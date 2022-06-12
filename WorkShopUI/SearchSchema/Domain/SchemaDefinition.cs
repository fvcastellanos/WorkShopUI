using Typesense;

namespace WorkShopUI.SearchSchema.Domain
{
    public class SchemaDefinition
    {
        public string Name { get; set; }

        public IEnumerable<Field> Fields { get; set; }
    }
}