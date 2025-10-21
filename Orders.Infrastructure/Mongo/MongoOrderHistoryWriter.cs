using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Options;
using Orders.Application.Abstractions;

namespace Orders.Infrastructure.Mongo
{
    public class MongoOrderHistoryWriter : IOrderHistoryWriter
    {
        private readonly IMongoCollection<BsonDocument> _col;

        public MongoOrderHistoryWriter(IOptions<MongoOptions> opt)
        {
            var clientSettings = MongoClientSettings.FromConnectionString(opt.Value.ConnectionString);
            clientSettings.GuidRepresentation = GuidRepresentation.Standard;

            var client = new MongoClient(clientSettings);
            var db = client.GetDatabase(opt.Value.Database);

            _col = db.GetCollection<BsonDocument>(opt.Value.HistoryCollection);
        }

        public Task WriteAsync(object record, CancellationToken ct) =>
            _col.InsertOneAsync(record.ToBsonDocument(), cancellationToken: ct);
    }
}
