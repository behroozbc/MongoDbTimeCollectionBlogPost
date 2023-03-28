using MongoDB.Bson;
using MongoDB.Driver;

using System.Timers;

const string CollectionName = "temperatureCollection";
const string url = "mongodb://localhost:27017/TimeCollectionBlogPost";
var mongoUrl = MongoUrl.Create(url);
var mongoDbClient = new MongoClient(mongoUrl);
var database = mongoDbClient.GetDatabase(mongoUrl.DatabaseName);
var readDataTimer = new System.Timers.Timer(1200);
var insertDataTimer = new System.Timers.Timer(1000);
if (!await IsCollectionExist(CollectionName, database))
{
    var options = new CreateCollectionOptions { TimeSeriesOptions = new TimeSeriesOptions("Time") };
    await database.CreateCollectionAsync(CollectionName, options);
    Console.WriteLine("Collection Created");
}
var collection = database.GetCollection<Temperature>(CollectionName);
insertDataTimer.Elapsed += InsertDataTimer_Elapsed;
readDataTimer.Elapsed += ReadDataTimer_Elapsed;
insertDataTimer.Start();
readDataTimer.Start();
Console.ReadLine();
async void ReadDataTimer_Elapsed(object? sender, ElapsedEventArgs e)
{
    Console.WriteLine((await (await collection.FindAsync(FilterDefinition<Temperature>.Empty)).ToListAsync()).Dump(DumpStyle.Console));
}
async void InsertDataTimer_Elapsed(object? sender, ElapsedEventArgs e)
{
    Console.WriteLine("Instert Data");
    await collection.InsertOneAsync(new Temperature
    {
        TemperatureValue = Random.Shared.Next(),
        Time = DateTime.UtcNow
    });
}
async Task<bool> IsCollectionExist(string collectionName, IMongoDatabase database)
{
    var filter = new BsonDocument("name", collectionName);
    var options = new ListCollectionNamesOptions { Filter = filter };
    return await (await database.ListCollectionNamesAsync(options)).AnyAsync();
}
