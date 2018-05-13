# NDtoGenerator

A visual studio extension to create new DTO classes from database tables/views inside any visual studio project.

## How to install 

### Dotnet global cli tool
`dotnet tool install NModelsGenerator.Cli --version 0.0.2`
- Go the the project folder where you want to generate classes. Open your command line tool and go: 
`nmg init` which will add a json config file in the same folder. Setup your config and go `nmg run`
- Rest is quite self explanotary

## Visual Studio Extension
- Currently non functional.

## Usage.
1. Add a nModelsconfig.json file to your project (It will added by default when you do `nmg init` for the first time). The schema of the json file is like this. This is the configuration file for the generator
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
  "BaseClassProperties": [ "Id" ],
  "Imports": [ "NDbPortal.Names.MappingAttributes", "System.Collections.Generic","System.Text" ],
  "Schemas": "schema_one,schema_two",
  "RootNameSpace":"My.Root.NameSpace",
  "NameSpaceSuffix": "$tableName",
  "SchemaBasedFolders": true
}
```
### Configuration
|Config Options|Type|Default|Usage|
|--------------|----|-------|-----|
|ConnectionString|string|null|The connection string to your database.|
|DbType|int|0|The type of database. 0 for PostgreSql, 1 for MSSQL, 2 for MySql|
|ClassAttributes|object|null|Custom Class attributes in object notation. **Accepts variables**.|
|BaseClassTypes|string[]|null|Array of strings of base classes or interfaces. Must begin with base class followed by interfaces as its supported by the language|
|BaseClassProperties|string[]|null|Array of strings of base classes properties. This will help you to exclude any properties that are already specified in the base class|
|Imports|string[]|["System"]|Array of strings for custom imports(using declarations).|
|Schemas|string|All schemas in database|Comma seperated names of schemas for which models are to be generated. If not provided all schemas will be assumed|
|NameSpaceSuffix|string|null|A suffix to be added after the root namespace. For example if "MyAwesomeModels" then the full namespace will be RootNameSpace.MyAwesomeModels for each class thats generated. **Accepts variables**.|
"SchemaBasedFolders"|bool|false|if true, generated classes are placed inside a folder by the name of its schema. Otherwise a flat list of entities will be generated.

### Variables

|Vairable name  | Usage |
|---------------|-------|
| $tableName    | Name of the table for which the attribute is being generated. Example: see the Json schema above.|
| $schemaName   | name of the schema for which the attribute is being generated. |


Enjoy !





