using Microsoft.VisualStudio.TestTools.UnitTesting;
using DemoApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using DemoApi;
using DemoApi.Controllers;
using DemoApi.DatabaseContext;
using DemoApi.Model;
using Microsoft.Extensions.Options;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
namespace DemoApi.Services.Tests
{
    [TestClass()]
    public class RecordServiceTests
    {
        private MongoDbContext _dbContext;
        private DataController _controller;

        public RecordServiceTests()
        {
            var settings = new MongoDbSettings
            {
                ConnectionString = "mongodb+srv://nikhildeshmukh2210:9FWGUaSKnK0FBXLD@cluster0.qfcy0b5.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0",
                Database = "DemoTestApi",
                Collection = "DemoTestCollection"
            };
            var options = Options.Create(settings);

            // Initialize the actual MongoDbContext and RecordsController
            _dbContext = new MongoDbContext(options);
            _controller = new DataController(new RecordService(_dbContext));
        }

        [TestMethod()]
        public async Task GetAsyncTestAsync()
        {
            try
            {
                // Arrange: Insert a record into the database
                var record = new JObject(new JProperty("name", "John Dire"));
                var createResult = await _controller.Create(record);
                var createdRecord = (createResult as OkObjectResult).Value as BsonDocument;
                var id = createdRecord["_id"].AsObjectId;

                //var id = ""; can use hardcoded for Test database

                // Act
                var result = await _controller.Get(id.ToString());

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(OkObjectResult));
                var okResult = result as OkObjectResult;
                Assert.IsNotNull(okResult.Value);
                Assert.IsInstanceOfType(okResult.Value, typeof(BsonDocument));
            }
            catch (Exception)
            {

                Assert.Fail();
            }
        }

        [TestMethod()]
        public async Task CreateAsyncTestAsync()
        {
            // Arrange
            var record = new JObject(new JProperty("name", "Madanlal Dhingra"));

            // Act
            var result = await _controller.Create(record);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.IsInstanceOfType(okResult.Value, typeof(BsonDocument));
        }

        [TestMethod()]
        public async Task UpdateAsyncTestAsync()
        {
            // Arrange: Insert a record into the database
            var record = new JObject(new JProperty("name", "Vasudev Phadke"));
            var createResult = await _controller.Create(record);
            var createdRecord = (createResult as OkObjectResult).Value as BsonDocument;
            var id = createdRecord["_id"].AsObjectId;

            // Update the record
            var updatedRecord = new JObject(new JProperty("name", "Lakshmibai Pai"));

            // Act
            var result = await _controller.Update(id.ToString(), updatedRecord);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.IsInstanceOfType(okResult.Value, typeof(BsonDocument));
        }

        [TestMethod()]
        public async Task GetAllAsyncTestAsync()
        {
            // Arrange: Nothing needed

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.IsInstanceOfType(okResult.Value, typeof(List<BsonDocument>));
        }

        [TestMethod()]
        public async Task DeleteAsyncTestAsync()
        {
            // Arrange: Insert a record into the database
            var record = new JObject(new JProperty("name", "John Doe"));
            var createResult = await _controller.Create(record);
            var createdRecord = (createResult as OkObjectResult).Value as BsonDocument;
            var id = createdRecord["_id"].AsObjectId;

            // Act
            var result = await _controller.Delete(id.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.IsInstanceOfType(okResult.Value, typeof(BsonDocument));
        }
    }
}