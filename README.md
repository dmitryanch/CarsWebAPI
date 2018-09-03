# CarsWebAPI
Workspace for testing Web API application with NoSQL databases

## Solution
Code: c# 7.3
Libs: .NET Standard 2.0
Server-only restfull WebApi App: .NET Core 2.1

# Steps
## Script for creating local mongodb data folder
```
cd C:\Program Files\MongoDB\Server\<your_mongo_version>\bin
mongod --dbpath <dataFolder>
```
## Creating a test record
```
cd C:\Program Files\MongoDB\Server\<your_mongo_version>\bin
mongo
use CarsDB
db.createCollection('Cars')
db.Cars.insert({'id':1,'Name':'Porche','Description':'Porche Desription'})
db.Cars.find({})
```

## Publish
```
cd <solution_derictory>
dotnet publish -c Release -r win10-x64
dotnet publish -c Release -r ubuntu.16.10-x64
```
