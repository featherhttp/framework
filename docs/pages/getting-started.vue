<template lang="md">
    # Learn .NET Feather HTTP
    ## Getting Started
    ### Download VS Code
    We recommend using VS Code in order to get a consistent experience with what we describe in these docs.
    You can get VS Code by clicking the link (it will open in a new tab): 
    
    <p class="text-center">
        <a class="underline" href="https://code.visualstudio.com/" target="_blank" rel="noreferrer">Download VS Code</a>
    </p>  
    
    Once you've installed VS Code you may want to get some extensions so you can have a better editing experience while working with C#.

    ### Download .NET Core 3.1
    
    The .NET Core runtime is required in order to run .NET applications. You should get the latest SDK version. You can download using the link below. The link will open in a new tab.
    
    <p class="text-center">
        <a class="underline" target="_blank" rel="noreferrer" href="https://dotnet.microsoft.com/download/dotnet-core/3.1">.NET Core Download page</a>
    </p>

    To use the `dotnet new` template, use the following command    

    ```
    dotnet new -i FeatherHttp.Templates::{version} --nuget-source https://f.feedz.io/davidfowl/featherhttp/nuget/index.json
    ```
    Where version is:
    [![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fdavidfowl%2Ffeatherhttp%2Fshield%2FFeatherHttp%2Flatest&label=FeatherHttp)](https://f.feedz.io/davidfowl/featherhttp/packages/FeatherHttp/latest/download)

    Once you've installed the template, run:

    ```
    dotnet new feather -n {name}
    ``` 

    This will create a new project using FeatherHttp.
</template>