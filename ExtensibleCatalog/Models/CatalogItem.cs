using MongoDB.Bson;
using MongoDB.Bson.DefaultSerializer;

namespace ExtensibleCatalog.Models
{
    [BsonDiscriminator(Required = true)]
    public class CatalogItem
    {

        public ObjectId Id { get; set; }

    }
}