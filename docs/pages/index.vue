<template>
  <div class="lg:ml-10 p-10 pt-0">    
    <div class="lg:flex lg:justify-center w-full">      
      <div class="lg:w-2/3 mt-6">
        <section>
          <div class="lg:ml-10">
            <h1 class="mt-6 h1 leading-tight text-center">Learn .NET Feather HTTP</h1>
            <h2 class="h2 text-center">Getting Started</h2>
            <h3 class="h3 text-center">Download VS Code</h3>
            <p>We recommend using VS Code in order to get a consistent experience with what we describe in these docs.
            You can get VS Code by clicking the link. It will open to the VS Code download page in a new tab.</p>
            <p class="text-center">
              <a class="underline" href="https://code.visualstudio.com/" target="_blank" rel="noreferrer">Download VS Code</a>
            </p>            
            <p>Once you've installed VS Code you may want to get some extensions so you can have a better editing experience while working with C#.</p>
            <h3 class="h3 text-center">Download .NET Core 3.1</h3>
            <p>The .NET Core runtime is required in order to run .NET applications. You should get the latest SDK version. You can download using the link below. The link will open in a new tab.</p>
            <p class="text-center">
              <a class="underline" target="_blank" rel="noreferrer" href="https://dotnet.microsoft.com/download/dotnet-core/3.1">.NET Core Download page</a>
            </p>
            <p>The .NET SDK comes with the dotnet CLI. This CLI tool will allow you to create new projects and download nuget packages to enable the tutorials below.</p>       
            <h3 class="h3 text-center">(optional) Copy and add the dotnet feather template repository</h3>
            <p>One of the great things the .NET CLI can do for you is quicky spin up a project by using <span class="code">dotnet new</span>. 
            Unfortunately, the Feather HTTP project template does not yet come as a part of the CLI. However you can remedy this by creating your own featherhttp template.</p>
            <p>You can do this in several easy steps.</p>
            <ol class="pt-0 max-w-full overflow-scroll lg:overflow-hidden">
              <li>Clone this repository: <span class="code">git clone git@github.com:RobotOptimist/featherhttp-template.git</span></li>
              <li>From the terminal, navigate to the root of the cloned repository</li>     
              <li>run <span class="code">dotnet new -i ./</span></li>         
            </ol>
            <p>Now you can spin up a new dotnet feather project in a new directory any time you want by typing <span class="code">dotnet new featherhttp</span></p>            
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
