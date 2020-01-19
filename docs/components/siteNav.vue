<template>
    <nav>
      <div class="flex flex-row justify-between w-full">
        <div class="flex flex-col">
          <nuxt-link class="text-green-800 text-xs font-bold underline" to="/">Home</nuxt-link>
          <a class="text-green-800 text-xs font-bold underline" href="https://github.com/davidfowl/FeatherHttp">.NET Feather HTTP GitHub</a>
        </div>
        <button class="lg:hidden flex items-center" @click="toggleMenu">
          <svg class="mr-1" height="32px" id="Layer_1" style="enable-background:new 0 0 32 32; color: white;" version="1.1" viewBox="0 0 32 32" width="32px" xml:space="preserve" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink">
              <path fill="#38a169" d="m4,8.491662l24,0c1.104,0 2,-1.121245 2,-2.502779s-0.896,-2.502779 -2,-2.502779l-24,0c-1.104,0 -2,1.121245 -2,2.502779s0.896,2.502779 2,2.502779zm24,5.005559l-24,0c-1.104,0 -2,1.121245 -2,2.502779s0.896,2.502779 2,2.502779l24,0c1.104,0 2,-1.121245 2,-2.502779s-0.896,-2.502779 -2,-2.502779zm0,10.011118l-24,0c-1.104,0 -2,1.121245 -2,2.502779s0.896,2.502779 2,2.502779l24,0c1.104,0 2,-1.121245 2,-2.502779s-0.896,-2.502779 -2,-2.502779z"></path>
          </svg>
          <span class="ml-1 uppercase">Menu</span>
        </button>
      </div>
      <topics-list class="lg:hidden" :isHidden="isHidden"></topics-list>
      <div class="lg:max-w-md lg:w-1/5 hidden lg:block absolute mt-12">
        <topics-list></topics-list>
      </div>
    </nav>
</template>

<script>
import topicsList from './topicsList';
export default {
    data() {
      return {
        isHidden: true
      }
    },
    methods: {
      toggleMenu() {
        this.isHidden = !this.isHidden;
      }
    },
    components: {
      topicsList
    }    
}
</script>