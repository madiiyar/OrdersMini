namespace Orders.Infrastructure.Mongo
{
    public class MongoOptions
    {
        public string ConnectionString { get; set; } = default!;
        public string Database { get; set; } = "orders";
        public string HistoryCollection { get; set; } = "OrderHistory";
    }
}
