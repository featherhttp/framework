<template lang="md">
# Hello World
                
While this section will serve to show how to do something with Feather Http, we'll also be going over some of the core concepts necessary to understand what Feather Http is attempting to do and how it works.

## The Code

Heres the code that will get your Hello World app off the ground

``` csharp
static async Task Main(string[] args)
{
    var app = WebApplication.Create(args);    

    app.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello World!");
    });

    await app.RunAsync();
}
```
                   
Now you can run this by typing `dotnet run` You can then navigate to [http://localhost:5000](http://localhost:5000) to view the output.

## WebApplication

WebApplication is a class used to configure the http pipeline and routes for your application.
It does this by accepting the calling assembly as a host and an application builder object. Much of this has been streamlined via static methods on WebApplication and is being demonstrated in the hello world code above.

### Create

The Create method creates both of these with reasonable defaults which is all that is necessary to create this hello world application. When creating a FeatherHttp application with, Controllers, for example - then you will need to instantiate the builder object differently. Similarly for other web server purposes, the builder object will need different methods called in order to appropriately create the piping.

## MapGet

In this HelloWorld web application - after we create the builder and run it via the Create method, we are registering a single route at `/` using [MapGet](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.endpointroutebuilderextensions.mapget?view=aspnetcore-3.1) and then within that, we're executing a delegate (which is basically equivalent to a JavaScript callback method with type safety) and returning a single string "Hello World".

## RunAsync

RunAsync is a wrapper around the [HostingAbstractionsHostExtensions](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.hostingabstractionshostextensions?view=dotnet-plat-ext-3.1) class. This is the method that takes the host - and runs the application. Running Async just means that it does not block the calling thread until a shutdown is triggered.

</template>

<script>
export default {

}
</script>