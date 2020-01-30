<template>
  <div class="lg:ml-10 p-10 pt-0">    
    <div class="lg:flex lg:justify-center w-full">      
      <div class="lg:w-2/3 mt-6">
        <section>
          <div class="lg:ml-10">
            <NuxtChild/>
          </div>
        </section>
        <section class="lg:flex flex-wrap justify-center">
          <card class="lg:w-64 w-full" v-for="topic in topics" :key="topic.title" :title="topic.title" :description="topic.description"></card>
        </section>
      </div>
    </div>
  </div>
</template>

<script>
import card from '../components/card'
import topicsList from '../components/topicsList'
export default {
  components: {
    card,
    topicsList
  },
  computed: {
    topics() {
      return this.$store.getters['topics/list'];
    }
  }
}
</script>
