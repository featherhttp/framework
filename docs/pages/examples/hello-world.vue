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

We'll be breaking down these core concepts: 
1. WebApplication
    1. What is the WebApplication class?        
    2. What is really happening in Create?
2. MapGet
3. RunAsync
</template>

<script>
export default {

}
</script>