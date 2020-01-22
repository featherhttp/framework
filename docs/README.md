# Docs contribution guide

## How to Contribute

This project uses [Nuxt](https://nuxtjs.org/) which is a static site generator. 

You will need to install [nodejs](https://nodejs.org/).
You can get the project running these commands from the `\docs` directory.

``` bash
    #install the projects from npm
    npm install

    #serve the project with hot reload from localhost:3000
    npm run dev

    #if you want to see what files will be output for the static site then
    npm run generate

    #if, from your fork, you'd like to generate the docs site so you can see how it will look on github pages then you can run these commands:
    npm run generate:gh-pages
    npm run deploy
```

There are several ways you may want to contribute to the docs. 

1. Add to the existing docs.
2. Create a brand new doc

### Add to the existing docs

Adding to the existing docs is easy. 
First, in pages/examples find the .vue file that contains the documentation you seek to modify.
Then within the template tags you can make your modifications.

``` html
<template lang="md">
    # Your contributions in markdown
    ...
</template>
```

You can optionally make contributions in regular html by removing `lang="md"` from the `<template>` tag.
And you also have all of the power of the [Vue](https://vuejs.org) instance that you can use from a script tag, below the `<template>` tag like so:

``` html
<template>
    <p>Your Content</p>
</template>

<script>
export default {
    //Vue stuff goes here
}
</script>
```

### Create a brand new doc

You can create a brand new doc by adding a new entry to `docs/store/topics.js`

``` JavaScript
/* In the state function there is an array of objects named list.
 These entries determine the nav and buttons on the home page of docs.
 They are displayed in the nav in order, with an appropriate category.
 You can add your new entry to this list */
export const state = () => ({
    list: [            
            { 
                title: 'Hello World', 
                category: 'Examples',
                description: 'Shows how to create a minimal .NET Core server using Feather HTTP'
            },
            //...
            {
                title: 'SignalR',
                category: 'Application Framework',
                description: 'How to set up SignalR'
            }
        ]
})

//Your new entry in the list would have this structure:
{
    title: 'Your New Title'
    category: 'The Category'
    description: 'A small description of your new doc'
}

```
Adding this to the `docs/store/topics.js` will set up the routing and add appropriate buttons, but you still haven't created the doc.

Next you'll create a new file in `docs/pages/examples`. It will be a `.vue` file. Afterwards the explanation for modifying a doc applies. 

Example
``` bash
cd docs/pages/examples
code my-new-docs.vue
#this will open the vue file in vscode for immediate editing. It won't appear in the directory until you save it though
```

Once the file is created you can set it up with a template tag. You don't have to create a script tag unless you need it.

``` html
    <template>
    </template>
```

If you want to write your doc in markdown you can add `lang="md"` to the template

``` html
    <template lang="md">
```

And that's pretty much all there is to it! Thank you very much for contributing!