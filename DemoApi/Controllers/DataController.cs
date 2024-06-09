using DemoApi.DatabaseContext;
using DemoApi.Model;
using DemoApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DataController : ControllerBase
    {
        private readonly MongoDbContext? _context = null;
        //private readonly IMongoCollection<BsonDocument>? _collection = null;
        private readonly RecordService _recordService;
        public DataController(RecordService recordService)
        {
            _recordService = recordService;
        }

        /// <summary>
        /// Gets data.
        /// </summary>
        /// <returns>Gets a single data item by id.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets a data item by Id", Description = "Returns a single data item by its unique ID.")]
        [SwaggerResponse(200, "The data item was found")] //, typeof(Model)
        [SwaggerResponse(404, "The data item was not found")]
        public async Task<IActionResult> Get(string id)
        {

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("Id cannot be null or empty.");
                }

                // Retrieve the record by id
                var record = await _recordService.GetAsync(id);

                if (record == null)
                {
                    return NotFound();
                }

                return Ok(record);
            }
            catch (MongoConnectionException ex)
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, new ApiErrorMessage { Error = "MongoDB connection issue.", Details = ex.Message });
            }
            catch (MongoException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiErrorMessage { Error = "An unexpected MongoDB error occurred.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Create a data item using POST Request
        /// </summary>
        /// <param name="record"></param>
        /// <returns>returns the json data - from mongodb atlas Document file</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Create Data", Description = "Returns a data item created successfuly")]
        [SwaggerResponse(200, "The data item created successfuly")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Something went wrong")]
        [Authorize]
        public async Task<IActionResult> Create([Microsoft.AspNetCore.Mvc.FromBody] JObject record)
        {
            try
            {
                if (record == null)
                {
                    return BadRequest("Record cannot be null.");
                }

                // Convert JObject to BsonDocument
                var bsonDocument = BsonDocument.Parse(record.ToString());

                // Process the record (e.g., validate, store in database)
                _ = await _recordService.CreateAsync(bsonDocument);

                // Return success response
                return Ok(bsonDocument);

            }
            catch (MongoConnectionException ex)
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, new ApiErrorMessage { Error = "MongoDB connection issue.", Details = ex.Message });
            }
            catch (MongoException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiErrorMessage { Error = "An unexpected MongoDB error occurred.", Details = ex.Message });
            }

        }

        /// <summary>
        /// Update Data Item by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="record" is a payload></param>
        /// <returns>Update data item</returns>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update Data", Description = "Returns a updated data item")]
        [SwaggerResponse(200, "The data item updated successfuly")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<IActionResult> Update(string id, [Microsoft.AspNetCore.Mvc.FromBody] JObject record)
        {
            try
            {

                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("Id cannot be null or empty.");
                }

                if (record == null)
                {
                    return BadRequest("Record cannot be null.");
                }

                // Convert JObject to BsonDocument
                var bsonDocument = BsonDocument.Parse(record.ToString());

                // Update the record
                var updatedRecord = await _recordService.UpdateAsync(id, bsonDocument);

                if (updatedRecord == null)
                {
                    return NotFound();
                }

                return Ok(updatedRecord);
            }
            catch (MongoConnectionException ex)
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, new ApiErrorMessage { Error = "MongoDB connection issue.", Details = ex.Message });
            }
            catch (MongoException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiErrorMessage { Error = "An unexpected MongoDB error occurred.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Get all data items
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Get all Data items", Description = "Get all Dat items")]
        [SwaggerResponse(200, "Get all data items")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            // Retrieve all records
            var records = await _recordService.GetAllAsync();
            return Ok(records);
        }

        /// <summary>
        /// Delete the data item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Deleted data item</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete data item", Description = "Delete data item")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("Id cannot be null or empty.");
                }

                // Delete the record
                var deletedRecord = await _recordService.DeleteAsync(id);

                if (deletedRecord == null)
                {
                    return NotFound();
                }

                return Ok(deletedRecord);
            }
            catch (MongoConnectionException ex)
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, new ApiErrorMessage { Error = "MongoDB connection issue.", Details = ex.Message });
            }
            catch (MongoException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiErrorMessage { Error = "An unexpected MongoDB error occurred.", Details = ex.Message });
            }
        }
    }
}
