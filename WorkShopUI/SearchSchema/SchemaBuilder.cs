using Typesense;
using WorkShopUI.SearchSchema.Domain;

namespace WorkShopUI.SearchSchema
{
    public class SchemaBuilder
    {
        private readonly ILogger _logger;

        private readonly ITypesenseClient _typesenseClient;

        public IEnumerable<SchemaDefinition> SchemaDefinitions { get; }

        public SchemaBuilder(ILogger<SchemaBuilder> logger,
                             ITypesenseClient typesenseClient)
        {
            _logger = logger;
            _typesenseClient = typesenseClient;
            SchemaDefinitions = buildSchemaDefinition();
        }

        public void BuildSchema()
        {
            buildSchema(SchemaDefinitions);
        }

        // ------------------------------------------------------------------------------------------

        private void buildSchema(IEnumerable<SchemaDefinition> schemaDefinitions)
        {
            _logger.LogInformation("Starting to build Search Schema");
            var collections = _typesenseClient.RetrieveCollections()
                .Result;

            schemaDefinitions.ToList()
                .ForEach(schemaDefinition => {

                    var schemaName = schemaDefinition.Name;
                    var fields = schemaDefinition.Fields;

                    var collection = collections.Filter(collection => collection.Name.Equals(schemaName))                
                        .FirstOrDefault();

                    if (collection == null) {

                        _logger.LogInformation("Creating schema: {0}", schemaName);
                        var schema = new Schema(schemaName, fields);
                        _typesenseClient.CreateCollection(schema)
                            .Wait();                            

                        return;
                    }

                    _logger.LogInformation("Schema: {0} already exists", schemaName);
                });

            _logger.LogInformation("Search Schema completed");
        }

        private IEnumerable<SchemaDefinition> buildSchemaDefinition()
        {
            return new List<SchemaDefinition>
            {
                new SchemaDefinition
                {
                    Name = "car-brands",
                    Fields = new List<Field>
                    {
                        new Field("id", FieldType.String),
                        new Field("name", FieldType.String, true, false, true, true),
                        new Field("description", FieldType.String, false, true),
                        new Field("active", FieldType.String, false, false, true, true),
                        new Field("tenant", FieldType.String, false, false, true),
                    }
                },
                new SchemaDefinition
                {
                    Name = "car-lines",
                    Fields = new List<Field>
                    {
                        new Field("id", FieldType.String),
                        new Field("carBrandId", FieldType.String, false, false, true),
                        new Field("name", FieldType.String, true, false, true, true),
                        new Field("description", FieldType.String, false, true),
                        new Field("active", FieldType.String, false, false, true, true),
                        new Field("tenant", FieldType.String, false, false, true),
                    }
                },
                new SchemaDefinition
                {
                    Name = "contacts",
                    Fields = new List<Field>
                    {
                        new Field("id", FieldType.String),
                        new Field("code", FieldType.String, false, false, true),
                        new Field("name", FieldType.String, true, false, true, true),
                        new Field("type", FieldType.String, false, false, true, true),
                        new Field("active", FieldType.String, false, false, true, true),
                        new Field("taxId", FieldType.String, false, true),
                        new Field("contact", FieldType.String, false, true),
                        new Field("description", FieldType.String, false, true),
                        new Field("tenant", FieldType.String, false, false, true),
                    }
                },
            };
        }
    }
}