using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace lolappAPI.Types.Interfaces
{
    public interface IDocument
    {
        public string Id { get; set; }

        DateTime CreatedAt { get; }
    }
}
