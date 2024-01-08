## The Implementation ##

This section presents the implemented solution of the problem stated in the next section.

### Structure ###

The project structure takes the form of three primary components: the core component, the api component and the tests component.

The Core contains the business logic, data models, services and utility functions; in essence, the core functionality of the project. The core component is designed to be independent of the user interface or API layers, allowing it to be reusable and testable. It also contains Validation, Transformation and Merging subcomponents that house the specific logic for data manipulation split by the data model it peratins to.

The API component serves the Core functionality to a network via a RESTful API. It handles the HTTP requests and responses.

Finally the Core.Tests component contains tests for the Core component.

### AWS integration ###

An S3 bucket pull service was developed, however, I ran out time when it came to implementing endpoints for it.
This pull service can be found in ETLAthena.Core/Services/DataPullService.cs or IDataPullService.cs

### Testing ###

The web api application can be launched by executing the command:

`dotnet add src/ETLAthena.Core.Tests/ETLAthena.Core.Tests.csproj reference src/ETLAthena.Core/ETLAthena.Core.csproj`

from the root directory.


Some rudementary tests have been implemented. They do not test the full functionality of the core logic, however.
Test can be run via the dotnet cli:
dotnet test

A more interactive way to test this web api is via the exposed endpoints:

- [HTML GET] http://.../buildings/all-buildings          --> returns all buildings currently in the data store (in-memory)
- [HTML POST] http://.../buildings/ingest/push/S1/bulk   --> push S1 data to the data store many json records at a time via the html Body.
- [HTML POST] http://.../buildings/ingest/push/S2/bulk   --> push S2 data to the data store many json records at a time via the html Body.
- [HTML POST] http://.../buildings/ingest/push/S1/single --> push S1 data to the data store one json record at a time via the html Body.
- [HTML POST] http://.../buildings/ingest/push/S2/single --> push S2 data to the data store one json record at a time via the html Body.
- [HTML POST] http://.../buildings/ingest/pull/S1        --> pull S1 data from API url.
- [HTML POST] http://.../buildings/ingest/pull/S2        --> pull S2 data from API url.

HTML request can be managed via a tool like Postman, a browser for gets, or something like curl.
Example pull usage: 
`curl -X POST -H "Content-Type: application/json" -d '{"ApiUrl": "https://example.com/api/data"}' http://localhost:port/ingest/pull/S2`.


The application is configured to ingest sample data on startup. If the /all-buildings endpoint is accessed after launch without ingesting any data,
the user will see S1sample2.json data printed to the screen. This configuration can be overridden by simply removing the sample data url from appsettings.json.


## The problem ##

There exist 2 sources, we'll call them S1 and S2, and their current schemas are shown below: 

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

### Solution ###

Write code that accepts data coming in the S1 or S2 format, validates and processes the data, saving the data in a data store (in memory or persistent). Things to consider:

* The ```id``` field identifies an entity per data source type only (so an entity in S1 will always have the same id, but it would be different from an entity in S2 that matches the same building) 
* Partial updates are possible (for instance an S2 update comes with missing coordinates) - In that case, merge the new data with whatever coordinates we received on the last update
* Errors in individual fields of the sources should not discard the rest of the items. For instance if the ```floorarea``` comes back as "John", I should ignore it and use the other entries if they are valid.
* The main challenge is matching between different sources: *The Shard* may come in during an update from S2 and I may want to get its floor area from there, while keeping the ```floorcount``` value I received earlier from S1. I will need to implement an algorithm that matches the incoming update with an entry in the data store. When an item matches with the existing data, the data will be updated with all the non-empty values it contains. Otherwise, a new entity will be created. 
* The following items would match with an existing entity in this order:
    * an item with an ```id``` that was encountered before. This only applies to id's coming from the same source.
    * an item that has the same ```name``` of an exisitng entity
    * an item that has the same ```postcode``` of an exisitng entity
* When matching by id, only the last processed ```id``` of an item is preserved and matched on future updates. So for example if an update of type S2 with ```id``` 1 comes in, which updates the existing entity that was last updated from type S1 with an ```id``` of 2, from now on matches will be done solely on ```id``` 1 coming in from S1. This only applies for matching by id, if the matching is done name or postcode, the id will be overwritten again.
* New data sources might need to be added in the future with a similar format.

