# Savills tech test #

### The problem ###

You are tasked with writing an ETL (extract-transform-load) system that ingests building data from different sources. Some sources use a push model (example: a service pushes a json file to S3) and some use a pull model (an API call to an external service). After processing, this data needs to be stored in an SQL database.

There currently exist 2 sources, we'll call them S1 and S2, and their current schemas are shown below: 

s1.json
```json
{
  "id": 1234,
  "address1": "The Shard, 32 London Bridge St",
  "address2": "London SE1 9SG",
  "lat": -0.0865,
  "lon": 51.5045,
  "floorcount": 95
}
```
s2.json
```json
{
  "id": 2345,
  "name": "The Gherkin",
  "address": "30 St Mary Axe, London EC3A 8EP",
  "postcode": "EC3A 8EP",
  "coordinates": [0.0803, 51.5145],
  "floorarea": 47950
}
```

### Your tasks ###

1. Think of the overall building blocks of the solution and how you'd setup the architecture in a cloud environment - we will go into details in the face to face interview
2. Write code that accepts data coming in the S1 or S2 format, validates and processes the data, saving the data in an SQL Database. Things to consider:

* The ```id``` field identifies an entity per data source type only (so an entity in S1 will always have the same id, but it would be different from an entity in S2 that matches the same building) 
* Partial updates are possible (for instance an S2 update comes with missing coordinates) - In that case, merge the new data with whatever coordinates we received on the last update
* Errors in individual fields of the sources should not discard the rest of the items. For instance if the ```floorarea``` comes back as "John", you should ignore it and use the other entries if they are valid.
* The main challenge is matching between different sources: you may get *The Shard* coming in an update from S2 and you want to get its floor area from there, while keeping the floorcount value you received earlier from S1. You will need to implement an algorithm that matches the incoming update with an entry in the DB. It then updates the existing entry or creates a new one, depending on a succesful match 
* There isn't a single correct way to do the matching, we want to see how you approach the problem.

### Tips for a succesful submission ###

* Do regular commits in the repository
* Add some tests for the code you write
* Update this readme to contain information on how to run/test your project. Make sure to include all the information needed in order to run the services. If you like, you can use Docker for abstracting the external components away (for the DB for instance). 

