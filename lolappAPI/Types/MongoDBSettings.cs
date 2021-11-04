using lolappAPI.Types.Interfaces;

namespace lolappAPI.Types
{
    public class MongoDbSettings : IMongoDbSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
    }
}
