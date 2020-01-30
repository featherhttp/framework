<template lang="md">
# Hello World
                
While this section will serve to show how to do something with Feather Http, we'll also be going over some of the core concepts necessary to understand what Feather Http is attempting to do and how it works.
We'll be breaking down these core concepts: 
                                
1. WebApplicationHostBuilder
2. WebApplicationHost
3. WebHostEnvironment

But first, here is the code to get your project up, running and saying "Hello World!" You can add this to the program.cs

``` csharp
static async Task Main(string[] args)
{
    var builder = WebApplicationHost.CreateDefaultBuilder(args);

    var app = builder.Build();

    app.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello World!");
    });

    await app.RunAsync();
}
```
            <prism class="overflow-scroll lg:overflow-hidden" :code="helloWorldCode" language="csharp">
            </prism>
        </div>                        
        <p>Now you can run this by typing <span class="code">dotnet run.</span> You can then navigate to <a class="underline" href="http://localhost:5000">http://localhost:5000</a> view the output.</p>
        <p>If you installed the template from getting started, then you already have this code.</p>
    </div> -->
</template>

<script>
export default {
    computed: {
        helloWorldCode() {
            return `static async Task Main(string[] args)
{
    var builder = WebApplicationHost.CreateDefaultBuilder(args);

    var app = builder.Build();

    app.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello World!");
    });

    await app.RunAsync();
}`
        }
    }
}
</script>