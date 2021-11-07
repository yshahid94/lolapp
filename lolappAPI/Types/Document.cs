using lolappAPI.Types.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace lolappAPI.Types
{
    public abstract class Document : IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [BsonElement("updatedOn")]
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public bool Equals(Document doc)
        {
            if(doc is null)
            {
                return false;
            }

            return this.Id == doc.Id;
        }
    }
}
