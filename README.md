![EfDiffLog logo](https://github.com/HodStudio/EfDiffLog/blob/master/EfDiffLogIcon.png)

# EfDiffLog by HodStudio

_An easy easy to create entity changes' log using Entity Framework_

[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.XitSoap&metric=bugs)](https://sonarqube.com/dashboard?id=HodStudio.XitSoap) [![CodeSmells](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.XitSoap&metric=code_smells)](https://sonarqube.com/dashboard?id=HodStudio.XitSoap) [![vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.XitSoap&metric=vulnerabilities)](https://sonarqube.com/dashboard?id=HodStudio.XitSoap) [![Duplicated Lines](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.XitSoap&metric=duplicated_lines_density)](https://sonarqube.com/dashboard?id=HodStudio.XitSoap) [![sqale_index](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.XitSoap&metric=sqale_index)](https://sonarqube.com/dashboard?id=HodStudio.XitSoap) [![sqale_rating](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.XitSoap&metric=sqale_rating)](https://sonarqube.com/dashboard?id=HodStudio.XitSoap) [![reliability_rating](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.XitSoap&metric=reliability_rating)](https://sonarqube.com/dashboard?id=HodStudio.XitSoap) [![security_rating](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.XitSoap&metric=security_rating)](https://sonarqube.com/dashboard?id=HodStudio.XitSoap) 

During 2018, I was working on a open-source project and one of the very interesting things that I did there was to create a good way to have an entity changes' logs. The solution was very interesting, but not so good if you wanted to reuse that, So, I decided to create a library to make it easier for anyone that wants to use it!

## Download it using NuGet
```
Install-Package HodStudio.EfDiffLog
```
Link: https://www.nuget.org/packages/HodStudio.EfDiffLog

## How to use
After installing the library via NuGet, you need to do some modiffications.

### Include the `LoggedEntity` on your Entities
To know which entities will generate the change logs, the EfDiffLog will use the `LoggedEntity` attribute to execute it.
If you use the attribute without providing the property name, it will use the default ("Id"). Pay attention that it's case sensitive.
```cs
using HodStudio.EfDiffLog.Model;

[LoggedEntity]
public class User
{
	public int Id { get; set; }
	public string UserName { get; set; }
}
```

In case you have an entity which the Id property is not named "Id", you can use the overload.
```cs
using HodStudio.EfDiffLog.Model;

[LoggedEntity("UserId")]
public class User
{
	public int UserId { get; set; }
	public string UserName { get; set; }
}
```

To avoid problems, especially during renames, we recommend using the `nameof` to configure the property name.
```cs
using HodStudio.EfDiffLog.Model;

[LoggedEntity(nameof(UserId))]
public class User
{
	public int UserId { get; set; }
	public string UserName { get; set; }
}
```

### Initialize the Dictionary of LoggedEntities
To have a better performance, we create a dictionary with the entities that will be logged and keep it. To do that, initialize the dictionary during the start of the app using the following method:
```cs
using System.Reflection;
using HodStudio.EfDiffLog.Repository;

LoggingContext.InitializeIdColumnNames(Assembly.GetEntryAssembly());
```

In case your models are stored in another assembly (or assemblies), just pass them to this parameter.

### Change your DbContext
To be able to use the full power of EfDiffLog, the easiest way is to inherits your DbContext from LoggingDbContext.
```cs
public class ApplicationDbContext : LoggingDbContext
```

#### Change the LogEntries table name
In case you have some configuration about the table name or schema name, change the properties `LogEntriesTableName` and `LogEntriesSchemaName`.
```cs
public SchoolContext()
{
    LogEntriesTableName = "LogsEntries";
    LogEntriesSchemaName = "dbo";
}
```

#### Special attention to override methods
In case you have some override on your DbContext class, make sure you call the `base`, to execute the methods from the `LoggingDbContext`.
```cs
protected override void OnModelCreating(DbModelBuilder modelBuilder)
{
	// your code here

    base.OnModelCreating(modelBuilder);
}

public override int SaveChanges()
{
    // your code here

    return base.SaveChanges();
}
```

### Add Migration to generate the LogEntry Database and Update the database
```
Add-Migration AddLogEntries
Update-Database
```

## Documentation
For more situations and examples, please, take a look on our Documentation on the [Wiki](https://github.com/HodStudio/EfDiffLog/wiki).

To see the planned new features, take a look on our [RoadMap](https://github.com/HodStudio/EfDiffLog/wiki#road-map-in-eternal-construction).