using MongoDB.Bson.Serialization.Attributes;

class Temperature
{
    [BsonId]
    public Guid Id { get; set; }
    public int TemperatureValue { get; set; }
    public DateTime? Time { get; set; }
}