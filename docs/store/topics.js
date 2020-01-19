export const state = () => ({
    list: [            
            { 
                title: 'Hello World', 
                category: 'Examples',
                description: 'Shows how to create a minimal .NET Core server using Feather HTTP'
            },
            {
                title: 'Cookies',
                category: 'Examples',
                description: 'Shows how to read and write cookies'
            },
            {
                title: 'Query Strings',
                category: 'Examples',
                description: 'Shows how to parse a query string into controller method parameters'
            },
            {
                title: 'HTML Form',
                category: 'Examples',
                description: 'How to receive data from an HTML form'
            },
            {
                title: 'Receive or Send files',
                category: 'Examples',
                description: 'Shows how to receive and send files'
            },
            {
                title: 'HTTP Headers',
                category: 'Examples',
                description: 'Shows how to read and write HTTP Headers within a controller method'
            },
            {
                title: 'Static Files',
                category: 'Examples',
                description: 'Shows how to serve static files'
            },
            {
                title: 'JSON Output',
                category: 'Examples',
                description: 'Shows how to convert C# objects to JSON in a controller method'
            },
            {
                title: 'Routing',
                category: 'Examples',
                description: 'Shows how to configure routing for API/MVC controllers'
            },
            {
                title: 'Logging',
                category: 'Examples',
                description: 'Shows how to set up logging in Feather HTTP'
            },
            {
                title: 'Change Ports',
                category: 'Examples',
                description: 'How to change the port of the webserver'
            },
            {
                title: 'TLS',
                category: 'Examples',
                description: 'Shows how to set up HTTPS in Feather HTTP'
            },
            {
                title: 'Exception Handling',
                category: 'Examples',
                description: 'Shows how to handle and display exceptions in development and production'
            },
            {
                title: 'ORMs',
                category: 'Data',
                description: 'How to use with Entity Framework Core, Dapper, or RepoDB'
            },
            {
                title: 'Redis',
                category: 'Data',
                description: 'How to set up Redis'
            },
            {
                title: 'Vue',
                category: 'JavaScript Frameworks',
                description: 'How to set up Vue.js'
            },
            {
                title: 'Angular',
                category: 'JavaScript Frameworks',
                description: 'How to set up Angular'
            },
            {
                title: 'Svelt',
                category: 'JavaScript Frameworks',
                description: 'How to set up Svelt'
            },
            {
                title: 'React',
                category: 'JavaScript Frameworks',
                description: 'How to set up React'
            },
            {
                title: 'Razor Pages',
                category: 'Application Framework',
                description: 'How to set up Razor Pages'
            },
            {
                title: 'MVC',
                category: 'Application Framework',
                description: 'How to set up MVC'
            },
            {
                title: 'Blazor Server Side',
                category: 'Application Framework',
                description: 'How to set up Blazor Server Side'
            },
            {
                title: 'Blazor',
                category: 'Application Framework',
                description: 'How to set up Blazor'
            },
            {
                title: 'SignalR',
                category: 'Application Framework',
                description: 'How to set up SignalR'
            }
        ]
})

export const actions = {}

export const mutations = {}

export const getters = {
    list(state) {
        return state.list;
    }
}