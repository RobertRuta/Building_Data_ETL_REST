# Savills tech test #

### The problem ###

You are tasked with writing an ETL (extract-transform-load) system that ingests building data from different sources. Some sources use a push model (example: a service pushes a json file to S3) and some use a pull model (an API call to an external service). After processing, this data needs to be stored in some sort of data store (you could use a SQL database if you want, but some sort of file or memory store is perfectly fine, taking into account the limited time)

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
2. Write code that accepts data coming in the S1 or S2 format, validates and processes the data, saving the data in a data store (in memory or persistent). Things to consider:

* The ```id``` field identifies an entity per data source type only (so an entity in S1 will always have the same id, but it would be different from an entity in S2 that matches the same building) 
* Partial updates are possible (for instance an S2 update comes with missing coordinates) - In that case, merge the new data with whatever coordinates we received on the last update
* Errors in individual fields of the sources should not discard the rest of the items. For instance if the ```floorarea``` comes back as "John", you should ignore it and use the other entries if they are valid.
* The main challenge is matching between different sources: you may get *The Shard* coming in an update from S2 and you want to get its floor area from there, while keeping the ```floorcount``` value you received earlier from S1. You will need to implement an algorithm that matches the incoming update with an entry in the data store. When an item matches with the existing data, the data will be updated with all the non-empty values it contains. Otherwise, a new entity will be created. 
* The following items would match with an existing entity in this order:
    * an item with an ```id``` that was encountered before. This only applies to id's coming from the same source.
    * an item that has the same ```name``` of an exisitng entity
    * an item that has the same ```postcode``` of an exisitng entity
* When matching by id, only the last processed ```id``` of an item is preserved and matched on future updates. So for example if an update of type S2 with ```id``` 1 comes in, which updates the existing entity that was last updated from type S1 with an ```id``` of 2, from now on matches will be done solely on ```id``` 1 coming in from S1. This only applies for matching by id, if the matching is done name or postcode, the id will be overwritten again.
* New data sources might need to be added in the future with a similar format.

### Tips for a succesful submission ###

* Do regular commits in the repository
* There is limited time, so please spend most of your time on the core matching and updating logic of the system - you can leave in stubbs or very basic implementations of things like validation or persistence (for instance you could store objects in memory rather than use a DB)
* Add some tests for the core logic
* Update this readme to contain information on how to run/test your project. Make sure to include all the information needed in order to run the services. If you like, you can use Docker for abstracting the external components away (for the DB if you choose to use a persistent data store). 

