<template>
  <div class="bg-grey-100">
    <site-nav class="p-10 pb-0"></site-nav>
    <nuxt />
  </div>
</template>

<script>
import siteNav from '../components/siteNav';
export default {
  components: {
    siteNav
  }
}
</script>

<style lang="postcss">
  .h1 {
    @apply text-5xl text-green-800 font-semibold;
  }
  .h2 {
    @apply text-4xl text-green-800
  }
  .h3 {
    @apply text-3xl text-green-800
  }
  .code {
    @apply bg-gray-400 p-1 font-mono
  }
  ol {
    @apply list-decimal list-inside
  }
  section pre.language-csharp {
    @apply bg-black pl-40
  }
  a {
    @apply underline
  }
  p, ol {
    @apply text-xl p-4;
    @screen lg {
      @apply text-base p-8;
    }
  }
</style>
