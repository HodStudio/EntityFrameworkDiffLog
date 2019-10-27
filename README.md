![EntityFrameworkDiffLog logo](https://github.com/HodStudio/EntityFrameworkDiffLog/blob/master/EntityFrameworkDiffLogIcon.png)

# EntityFrameworkDiffLog by HodStudio

_An easy way to create entity changes' log using Entity Framework_

[![License](https://img.shields.io/github/license/hodstudio/entityframeworkdifflog)](https://github.com/HodStudio/EntityFrameworkDiffLog/blob/master/LICENSE.md)
[![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/hodstudio/entityframeworkdifflog)](https://github.com/HodStudio/EntityFrameworkDiffLog/releases)
[![GitHub Release Date](https://img.shields.io/github/release-date/hodstudio/entityframeworkdifflog)](https://github.com/HodStudio/EntityFrameworkDiffLog/releases)
[![Nuget](https://img.shields.io/nuget/dt/HodStudio.EntityFrameworkDiffLog)](https://www.nuget.org/packages/HodStudio.EntityFrameworkDiffLog/)
[![GitHub issues by-label](https://img.shields.io/github/issues-raw/hodstudio/entityframeworkdifflog/bug)](https://github.com/HodStudio/EntityFrameworkDiffLog/labels/bug)

[![Build status](https://ci.appveyor.com/api/projects/status/1r9ebih0q0ntqfu4?svg=true)](https://ci.appveyor.com/project/Cussa/entityframeworkdifflog)
[![AppVeyor tests (compact)](https://img.shields.io/appveyor/tests/cussa/entityframeworkdifflog?compact_message&logo=appveyor)](https://ci.appveyor.com/project/Cussa/entityframeworkdifflog/build/tests)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.EntityFrameworkDiffLog&metric=alert_status)](https://sonarcloud.io/dashboard?id=HodStudio.EntityFrameworkDiffLog)
[![sqale_rating](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.EntityFrameworkDiffLog&metric=sqale_rating)](https://sonarqube.com/dashboard?id=HodStudio.EntityFrameworkDiffLog)
[![reliability_rating](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.EntityFrameworkDiffLog&metric=reliability_rating)](https://sonarqube.com/dashboard?id=HodStudio.EntityFrameworkDiffLog)
[![security_rating](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.EntityFrameworkDiffLog&metric=security_rating)](https://sonarqube.com/dashboard?id=HodStudio.EntityFrameworkDiffLog) 
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.EntityFrameworkDiffLog&metric=bugs)](https://sonarqube.com/dashboard?id=HodStudio.EntityFrameworkDiffLog)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.EntityFrameworkDiffLog&metric=vulnerabilities)](https://sonarqube.com/dashboard?id=HodStudio.EntityFrameworkDiffLog)
[![CodeSmells](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.EntityFrameworkDiffLog&metric=code_smells)](https://sonarqube.com/dashboard?id=HodStudio.EntityFrameworkDiffLog)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.EntityFrameworkDiffLog&metric=sqale_index)](https://sonarqube.com/dashboard?id=HodStudio.EntityFrameworkDiffLog)
[![Duplicated Lines](https://sonarcloud.io/api/project_badges/measure?project=HodStudio.EntityFrameworkDiffLog&metric=duplicated_lines_density)](https://sonarqube.com/dashboard?id=HodStudio.EntityFrameworkDiffLog)

During 2018, we worked on an open-source project, and one of the exciting things that we did there was to create an excellent way to have an entity changes' logs. The solution was exciting, but not so good if you wanted to reuse that. So, we decided to create a library to make it easier for anyone that wants to use it!

## Download it using NuGet
```
Install-Package HodStudio.EntityFrameworkDiffLog
```
Link: https://www.nuget.org/packages/HodStudio.EntityFrameworkDiffLog

## How to use
After installing the library via NuGet, you need to make some modifications.

### Include the `LoggedEntity` on your Entities
To know which entities will generate the changelogs, the EntityFrameworkDiffLog will use the `LoggedEntity` attribute to execute it.
If you use the attribute without providing the property name, it uses the default ("Id"). Pay attention that it's case sensitive.
```cs
using HodStudio.EntityFrameworkDiffLog.Model;

[LoggedEntity]
public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
}
```

In case you have an entity which the Id property is not named "Id," you can use the overload.
```cs
using HodStudio.EntityFrameworkDiffLog.Model;

[LoggedEntity("UserId")]
public class User
{
    public int UserId { get; set; }
    public string UserName { get; set; }
}
```

To avoid problems, especially during renames, we recommend using the `nameof` to configure the property name.
```cs
using HodStudio.EntityFrameworkDiffLog.Model;

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
using HodStudio.EntityFrameworkDiffLog.Repository;

LoggingContext.InitializeIdColumnNames(Assembly.GetEntryAssembly());
```

In case your models are stored in another assembly (or assemblies), pass them to this parameter.

### Change your DbContext
To be able to use the full power of EntityFrameworkDiffLog, the easiest way is to inherit your DbContext from LoggingDbContext.
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

### Add-Migration to generate the LogEntry Database and Update the database
```
Add-Migration AddLogEntries
Update-Database
```

### Configure the user that is executing the operation
In case your application has authenticated users, it makes sense that you would like to know which user made some operation. Based on that, the EntityFrameworkDiffLog is prepared to log this information too. You need to configure the property `UserId` from the LoggingDbContext. For example, you can do that on the `SaveChanges` from your DbContext. This just an example. Please, take the user based on the logic from your application.
```cs
public override int SaveChanges()
{
    // your code here

    UserId = System.Threading.Thread.CurrentPrincipal.Identity.Name;

    return base.SaveChanges();
}
```

### Configure the type of id used on your project
In most of the cases, the database generates the id. That creates a problem that the id on the logs would always be as zero. So, to avoid this, we introduced the property IdGeneratedByDatabase. In the case of true (default), the log for added entities occurs after the first SaveChanges.
Realize, please, that it executes two SaveChanges to the database: one for the entities itself + updated entities' logs + deleted entities' log and another for the added entities' logs.
In case you use, for example, GUID for the Id property, most probably you generate this ID inside your application's logic. In that case, you can turn off this functionality. It executes only one SaveChanges in that case.
```cs
public SchoolContext()
{
    IdGeneratedByDatabase = false;
}
```

### If you don't want to use UTC for log time
As the library can be used in several places with different timezones, the default configuration is to use Utc. However, if your application is only used in one timezone, you can configure the library to use the time in the current timezone.
```cs
public SchoolContext()
{
    UseUtcTime = false;
}
```

## Examples
The source code contains two examples: one for .Net 4.5 and another for .Net Core 2.2. Both of them are the official tutorials projects from Microsoft. For more information about them, please, take a look at the links below:
- .Net 4.5: https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/creating-an-entity-framework-data-model-for-an-asp-net-mvc-application
- .Net Core 2.2: https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-2.2

We decided to make it available to version .Net Core 2.2, the newest version (3.0) is not used so much yet.

## Documentation
For more situations and examples, please, take a look at our Documentation on the [Wiki](https://github.com/HodStudio/EntityFrameworkDiffLog/wiki).

To see the planned new features, take a look at our [RoadMap](https://github.com/HodStudio/EntityFrameworkDiffLog/wiki#road-map-in-eternal-construction).
