<template>
    <div>
        <input type="text" name="name" v-model="name" value="" />
        <button @click="getAC">hi</button>
        <p>Message is: {{ name }}</p>
        <div v-if="summoner != null">
            <p>Message is: {{ summoner.accountID }}</p>
        </div>
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from "vue-property-decorator";
    import { IConfig, Client, Summoner } from "@/api/api-client";
    @Component({})
    export default class LeagueGraphView extends Vue {
        name: string = "";
        summoner: Summoner | null = null;

        get apiClient() {
            return new Client(new IConfig(""), "https://localhost:5001");
        }

        async getAC() {
            try {
                const response = await this.apiClient.getSummoner(this.name);

                if (response.httpStatusCode === 200) {
                    this.summoner = response.result!;
                    console.log(this.summoner);
                }
            }
            catch (error) {
                console.log();
            }
            finally {
                console.log();
            }
        }
    }

</script>

<style lang="scss"></style>
