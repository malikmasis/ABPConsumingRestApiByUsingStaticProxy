# Consuming REST APIs from a .NET Client Using ABP's Client Proxy System
In this article, we will show how to consume rest api by using static client proxy by creating a new project and converting that from dynamic client proxy to static client proxy. Also, I will glance the differences between static and dynamic generic proxies.

Article flow
* Create a new ABP application with ABP CLI
* Create application service interface
* Implement the application service 
* Consume the app service from the console application
* Convert application to use static client proxies 
* Add authorization to the application service endpoint
* Grant the permission 
* Further reading

Firstly create a new template via ABP CLI. 

````shell
abp new Acme.BookStore -t app
````

> If you haven't installed it yet, you should install the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI).

At the same folder build the project with the following command on the cli.
````shell
dotnet build /graphbuild
````

It will restore the project and download the NuGet packages.

Now you should run the DbMigrator project to up your database.

Now your project is ready you can run it properly.

![structure-of-the-project](images/structure.png)

From now on, we will add some files to show the case to you.  

### Creating the Application Service

Assume that we have an `IBookAppService` interface:

````csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Books
{
    public interface IBookAppService : IApplicationService
    {
        Task<List<BookDto>> GetListAsync();
    }
}
````

That uses a `BookDto` defined as shown:

```csharp
using System;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Books
{
    public class BookDto : AuditedEntityDto<Guid>
    {
        public string AuthorName { get; set; }

        public string Name { get; set; }

        public DateTime PublishDate { get; set; }

        public float Price { get; set; }
    }
}
```

And implemented as the following:

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Books
{
    public class BookAppService : ApplicationService, IBookAppService
    {
        public Task<List<BookDto>> GetListAsync()
        {
            return Task.FromResult(new List<BookDto>()
            {
                new BookDto(){ Name = "Anna Karenina", AuthorName ="Tolstoy", Price = 50},
                new BookDto(){ Name = "Crime and Punishment", AuthorName ="Dostoevsky", Price = 60},
                new BookDto(){ Name = "Mother", AuthorName ="Gorki", Price = 70}
            });
        }
    }
}
```
It simply returns a list of books. You probably want to get the books from a database, but it doesn't matter for this article. To do it you can visit [here] (https://docs.abp.io/en/abp/latest/Tutorials/Part-1?UI=MVC&DB=EF)

### Creating the Application Service Tests
Add a new test class, named BookAppService_Tests in the Application.Tests

```csharp
using System.Threading.Tasks;
using Xunit;

namespace Acme.BookStore.Books
{
    public class BookAppService_Tests : BookStoreApplicationTestBase
    {
        private readonly IBookAppService _bookAppService;

        public BookAppService_Tests()
        {
            _bookAppService = GetRequiredService<IBookAppService>();
        }

        [Fact]
        public async Task Should_Get_List_Of_Books()
        {
            var result = await _bookAppService.GetListAsync();
            Assert.Equal(3, result.Count);
        }
    }
}
```

### Convert application to use static client proxies
First, add Volo.Abp.Http.Client NuGet package to your client project:
````shell
Install-Package Volo.Abp.Http.Client
````
Then add AbpHttpClientModule dependency to your module:
```csharp
[DependsOn(
    typeof(AbpHttpClientModule)
    //the other dependencies
    )]

public class BookStoreApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
       //Other configurations

        // Prepare for static client proxy generation
        context.Services.AddStaticHttpClientProxies(
            typeof(BookStoreApplicationContractsModule).Assembly
        );
    }
}
```

`AddStaticHttpClientProxies` method gets an assembly, finds all service interfaces in the given assembly, and prepares for static client proxy generation.

> The [application startup template](https://docs.abp.io/en/abp/latest/Startup-Templates/Application) comes pre-configured for the **dynamic** client proxy generation, in the `HttpApi.Client` project. If you want to switch to the **static** client proxies, change `context.Services.AddHttpClientProxies` to `context.Services.AddStaticHttpClientProxies` in the module class of your `HttpApi.Client` project.

Now you're ready to generate the client proxy code by running the following the command in the root folder of your client project when your project is running.

````bash
abp generate-proxy -t csharp -u http://localhost:44397/
````

You have been should the generated files under the same folder.


### Further Reading
In this small tutorial, I explained how you can create an example project and apply static client proxy instead of dyamic client proxy. Also summarized the differences of both approaches.

If you want to get more information about, you can read the following documents:

[Static C# API Client Proxies](https://docs.abp.io/en/abp/latest/API/Static-CSharp-API-Clients)
[Dynamic C# API Client Proxies](https://docs.abp.io/en/abp/latest/API/Dynamic-CSharp-API-Clients)
[Web Application Development Tutorial ](https://docs.abp.io/en/abp/latest/Tutorials/Part-1?UI=MVC&DB=EF)
