<template>
    <ul :class="{ 'max-h-none': isHidden, 'p-2 max-h-full transition-max-height': !isHidden }" 
    class="absolute leading-loose mt-1 rounded-lg font-bold bg-green-800 text-white w-4/5 transition-250 overflow-y-hidden lg:static lg:max-w-sm lg:w-full lg:mt-6">
        <li><nuxt-link exact to="/getting-started">Getting Started</nuxt-link></li>
        <li v-for="topic in topics" :key="topic.title"><nuxt-link exact :to="`/examples/${topic.title.split(' ').join('-').toLowerCase()}`">{{topic.title}}</nuxt-link></li>
      </ul>
</template>

<script>
export default {
    props: {
        isHidden: {
            default: false,
            type: Boolean
        }
    },
    computed: {
      topics() {
        return this.$store.getters['topics/list'];
      }
    }
}
</script>