<template>
    <div class="m-6 ml-0 lg:ml-6 lg:mb-0 lg:mr-0 p-10 pl-5 pr-5 shadow-md bg-green-200 rounded-lg">
        <h2 class="text-2xl font-semibold rounded-lg text-blue-900">
            {{title}}
        </h2>
        <p class="mt-2 pl-0">{{description}}</p>
        <div class="mt-8 flex justify-end">
            <nuxt-link class="underline p-3 pr-5 pl-5 bg-green-800 text-white font-bold rounded-lg" :to="`${path}/${title.split(' ').join('-').toLowerCase()}`">Go to Tutorial</nuxt-link>
        </div>        
    </div>
</template>

<script>
export default {
    props: {
        title: {
            default: 'Default Title',
            type: String
        },
        description: {
            default: 'Default Description',
            type: String
        },
        path: {
            default: 'examples',
            type: String
        }
    }
}
</script>