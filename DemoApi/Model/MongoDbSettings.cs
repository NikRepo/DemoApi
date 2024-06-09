namespace DemoApi.Model
{
    /// <summary>
    /// This class represents the settings of MongoDb Atlas connection details - mapped with configuration in application.json
    /// </summary>
    public class MongoDbSettings
    {
        public string? ConnectionString { get; set; }
        public string? Database { get; set; }
        public string? Collection { get; set; }
    }
}
