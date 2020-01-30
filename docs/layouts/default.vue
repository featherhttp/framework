<template>
  <div class="bg-grey-100">
    <site-nav class="p-10 pb-0"></site-nav>
    <div class="lg:ml-10 p-10 pt-0">   
        <div class="lg:flex lg:justify-center w-full">      
            <div class="lg:w-2/3 mt-6">
                <section>
                    <div class="lg:ml-10">
                      <nuxt />
                    </div>
                </section>
            </div>
        </div>
    </div>
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
  h1 {
    @apply mt-6 leading-tight text-center text-5xl text-green-800 font-semibold;
  }
  h2 {
    @apply text-center text-4xl text-green-800
  }
  h3 {
    @apply text-center text-3xl text-green-800
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
  li {
    @apply m-1;
  }
  li > p {
    @apply p-0 inline-block ml-4;
  }
</style>
