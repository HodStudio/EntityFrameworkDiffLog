![EfDiffLog logo](https://github.com/HodStudio/EfDiffLog/blob/master/EfDiffLogIcon.png)

# EfDiffLog by HodStudio

_An easy easy to create entity changes' log using Entity Framework_

[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.EfDiffLog&metric=bugs)](https://sonarqube.com/dashboard?id=HodStudio.EfDiffLog) [![CodeSmells](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.EfDiffLog&metric=code_smells)](https://sonarqube.com/dashboard?id=HodStudio.EfDiffLog) [![vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.EfDiffLog&metric=vulnerabilities)](https://sonarqube.com/dashboard?id=HodStudio.EfDiffLog) [![Duplicated Lines](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.EfDiffLog&metric=duplicated_lines_density)](https://sonarqube.com/dashboard?id=HodStudio.EfDiffLog) [![sqale_index](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.EfDiffLog&metric=sqale_index)](https://sonarqube.com/dashboard?id=HodStudio.EfDiffLog) [![sqale_rating](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.EfDiffLog&metric=sqale_rating)](https://sonarqube.com/dashboard?id=HodStudio.EfDiffLog) [![reliability_rating](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.EfDiffLog&metric=reliability_rating)](https://sonarqube.com/dashboard?id=HodStudio.EfDiffLog) [![security_rating](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.EfDiffLog&metric=security_rating)](https://sonarqube.com/dashboard?id=HodStudio.EfDiffLog) 

During 2018, we worked on a open-source project and one of the very interesting things that we did there was to create a good way to have an entity changes' logs. The solution was very interesting, but not so good if you wanted to reuse that, So, we decided to create a library to make it easier for anyone that wants to use it!

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
    LogEntriesTableName = "LogEntries";
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

### Configure the user that is executing the operation
In case your application have authenticated users, it makes sense that you would like to know which user made some operation. Based on that, the EfDiffLog is prepared to log this information too. You just need to configure the property `UserId` from the LoggingDbContext. For example, you can do that on the `SaveChanges` from your DbContext. This is just an example. Please, take the user based on the logic from you application.
```cs
public override int SaveChanges()
{
    // your code here

    UserId = System.Threading.Thread.CurrentPrincipal.Identity.Name;

    return base.SaveChanges();
}
```

## Examples
The source code contains two examples: one for .Net 4.5 and another for .Net Core 2.2. Both of them are the official tutorials projects from Microsoft. For more information about them, please, take a look on the links bellow:
- .Net 4.5: https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/creating-an-entity-framework-data-model-for-an-asp-net-mvc-application
- .Net Core 2.2: https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-2.2

We decided to make it available to version .Net Core 2.2, the newest version (3.0) is not used so much yet.

## Documentation
For more situations and examples, please, take a look on our Documentation on the [Wiki](https://github.com/HodStudio/EfDiffLog/wiki).

To see the planned new features, take a look on our [RoadMap](https://github.com/HodStudio/EfDiffLog/wiki#road-map-in-eternal-construction).