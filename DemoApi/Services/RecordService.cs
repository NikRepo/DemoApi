using DemoApi.DatabaseContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DemoApi.Services
{
    public class RecordService
    {
        private readonly IMongoCollection<BsonDocument> _records;

        /// <summary>
        /// constructor intializes records /documents
        /// </summary>
        /// <param name="context"></param>
        public RecordService(MongoDbContext context)
        {
            _records = context.Records;
        }

        /// <summary>
        /// This is service method - which gets the document from mongoDb by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Document</returns>
        public async Task<BsonDocument> GetAsync(string id)
        {
            return await _records.Find(new BsonDocument { { "_id", new ObjectId(id) } }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Creates a new document in MongoDb Atlas Database
        /// </summary>
        /// <param name="record"></param>
        /// <returns>The newly created document</returns>
        public async Task<BsonDocument> CreateAsync(BsonDocument record)
        {
            await _records.InsertOneAsync(record);
            return record;
        }

        /// <summary>
        /// Updates the document by its unique id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="record"></param>
        /// <returns>Updated Document</returns>
        public async Task<BsonDocument> UpdateAsync(string id, BsonDocument record)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            var options = new FindOneAndReplaceOptions<BsonDocument> { ReturnDocument = ReturnDocument.After };
            return await _records.FindOneAndReplaceAsync(filter, record, options);
        }

        /// <summary>
        /// Get all the documents
        /// </summary>
        /// <returns>all documents</returns>
        public async Task<List<BsonDocument>> GetAllAsync()
        {
            return await _records.Find(new BsonDocument()).ToListAsync();
        }

        /// <summary>
        /// Delete a document/data item from MongoDb Atlas database by its unique id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BsonDocument> DeleteAsync(string id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            return await _records.FindOneAndDeleteAsync(filter);
        }
    }
}
