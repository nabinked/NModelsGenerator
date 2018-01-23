# NDtoGenerator

A visual studio extension to create new DTO classes from database tables/views inside any visual studio project.

## How to use.
1. Add a nModelsconfig.json file to your project. The schema of the json file is like this. This is the configuration file for the generator
```json
  {
  "ConnectionString": "Connection string to your database.",
  "DbType": 0,
  "ClassAttributes": {
    "Table": "$tableName",
    "SomeOtherAttribute":{
        "ChildProperty1":"Value1",
        "ChildProprety2":"Value2"
    }
  },
  "BaseClassTypes": [ "BaseClass", "IMyEntity","IAwesome" ],
  "Imports": [ "NDbPortal.Names.MappingAttributes", "System.Collections.Generic","System.Text" ],
  "Schemas": "schema_one,schema_two",
  "NameSpaceSuffix": "Entities",
  "SchemaBasedFolders": true
}
```

|Config Options|Type|Default|Usage|
|--------------|----|-------|-----|
|ConnectionString|string|null|The connection string to your database.|
|DbType|int|0|The type of database. 0 for PostgreSql, 1 for MSSQL, 2 for MySql|
|ClassAttributes|object|null|Custom Class attributes in object notation. Can include runtime variables.|
|BaseClassTypes|string[]|null|Array of strings of base classes or interfaces. Must begin with base class followed by interfaces as its supported by the language|
|Imports|string[]|["System"]|Array of strings for custom imports(using declarations).|
|Schemas|string|All schemas in database|Comma seperated names of schemas for which models are to be generated. If not provided all schemas will be assumed|
|NameSpaceSuffix|string|null|A suffix to be added after the root namespace. For example if "MyAwesomeModels" then the full namespace will be ProjectDefaultNameSpace.MyAwesomeModels for each class thats generated|
"SchemaBasedFolders"|bool|false|if true, generated classes are placed inside a folder by the name of its schema. Otherwise a flat list of entities will be generated.

2. Install NModelsGenerator from the visual studio extensions [marketplace/gallery](https://marketplace.visualstudio.com/items?itemName=NabinKarkiThapa.NModelsGenerator) or search for NModelsGenerator inside Visual Studio Tools > Extensions and Updates

3. Right click on the project and click "Generate Models in this Project". If a config.json file is not found then a new nModelsconfg.json file will be created with default schema. You can try again after providing necessary configurations.

4. Enjoy !





