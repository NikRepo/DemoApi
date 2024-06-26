# DemoApi

This project is a .NET 8 Web API that demonstrates schema-less CRUD (Create, Read, Update, Delete) operations using MongoDB Atlas. The application is containerized using Docker and is designed to be deployed on a Linux environment.

API that allows storing and retrieving JSON data without a predefined schema. The data is persisted in
a MongoDb Atlas database.

------------------------------------------------------------------------------------------------------
Prerequisites
------------------------------------------------------------------------------------------------------

.NET 8 SDK
Docker
MongoDB Atlas account and cluster
Linux environment for deployment

-------------------------------------------------------------------------------------------------------
Setup Instructions:
--------------------------------------------------------------------------------------------------------

1. Clone the Repository
git clone https://github.com/NikRepo/DemoApi.git

cd DemoApi

3. Configure MongoDB Atlas
   
   Create a MongoDB Atlas account if you don't have one.
   
   Create a new cluster.
   
   Get the connection string from MongoDB Atlas.
   
   Replace the <Your MongoDB Connection String> in the appsettings.json file with your MongoDB connection string.
   
   Replace Database name and Collection name in appsettings.json

3.Update appsettings.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "MongoDb": {
    "ConnectionString": "mongodb+srv://username:password@cluster5.qfcy0b5.mongodb.net/?retryWrites=true&w=majority&appName=Cluster5",
    "Database": "DemoApi",
    "Collection": "DemoCollection"
  },
  "AllowedHosts": "*"
}


----------------------------------------------------------------------------
Deployment on Linux
----------------------------------------------------------------------------

Ensure Docker is installed and running on your Linux machine.
Copy the Docker image to your Linux machine or build it directly on the machine.

4.Using Docker - Build Image(go to directory of docker file)

Place it in root directory where the .sln file is present

docker build -t DemoApiImage .

5. Run the docket continer:
   
docker run -d -p 5000:80 --name container01 DemoApiImage

----------------------------------------------------------------------------------------------------------------

API Endpoints

----------------------------------------------------------------------------------------------------------------

POST api/data: Accepts a JSON object and stores it in the database.

GET api/data/{id}: Retrieves the JSON object associated with the given ID from the database.

PUT api/data/{id}: Updates the existing JSON object with the given ID in the database.

DELETE api/data/{id}: Deletes the JSON object with the given ID from the database.

------------------------------------------------------------------------------------------------------------------
Example - curl
-----------------------------------------------------------------------------------------------------------------

**********************************************************************
Create a New Item:

curl -X POST http://ip-address:5000/api/data \-H "Content-Type: application/json" \-d '{  "name": "New Item",  "description": "Description for the new item",  "price": 15.0}'

Payload body:

{
  "name": "New Item",
  "description": "Description for the new item",
  "price": 15.0
}

Response:

{
  "id": "60d5f484f1b8e5d2b0f2b0b3",
  "name": "New Item",
  "description": "Description for the new item",
  "price": 15.0
}
**********************************************************************

Retrieve a Specific Item by ID:

curl -X GET http://ip-address:5000/api/data/60d5f484f1b8e5d2b0f2b0b3

Response:

{
  "id": "60d5f484f1b8e5d2b0f2b0b3",
  "name": "New Item",
  "description": "Description for the new item",
  "price": 15.0
}
**********************************************************************

**********************************************************************

Update a Specific Item by ID:

curl -X PUT http://ip-address:5000/api/data/60d5f484f1b8e5d2b0f2b0b3

Payload body:

{
  "id": "60d5f484f1b8e5d2b0f2b0b3",
  "name": "Modified Item",
  "description": "Description for the new item",
  "price": 15.0
}

Response:

{
  "id": "60d5f484f1b8e5d2b0f2b0b3",
  "name": "Modified Item",
  "description": "Description for the new item",
  "price": 15.0
}
**********************************************************************

**********************************************************************

Delete a Specific Item by ID:

curl -X DELETE http://ip-address:5000/api/data/60d5f484f1b8e5d2b0f2b0b3

Response:

{
  "id": "60d5f484f1b8e5d2b0f2b0b3",
  "name": "New Item",
  "description": "Description for the new item",
  "price": 15.0
}
**********************************************************************







