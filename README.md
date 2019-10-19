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
```cs
[LoggedEntity]
public class User
{
	public int Id { get; set; }
	public string UserName { get; set; }
}
```

In case you have an entity which the Id property is not named "Id", you can use the overload.
```cs
[LoggedEntity("UserId")]
public class User
{
	public int UserId { get; set; }
	public string UserName { get; set; }
}
```

### Change your DbContext
To be able to use the full power of EfDiffLog, the easiest way is to inherits your DbContext from LoggingDbContext.
```cs
public class ApplicationDbContext : LoggingDbContext
```

Our LoggingDbContext has a new parameter constructor: it receives an params list of assemblies where your model is defined, and it will be used to find the entities that has the `LoggedEntity` property defined.
```cs
public class ApplicationDbContext : LoggingDbContext
{
	public ApplicationDbContext() : base() { }
}
```

## Documentation
For more situations and examples, please, take a look on our Documentation on the [Wiki](https://github.com/HodStudio/EfDiffLog/wiki).

To see the planned new features, take a look on our [RoadMap](https://github.com/HodStudio/EfDiffLog/wiki#road-map-in-eternal-construction).