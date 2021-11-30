<template>
    <div>
        <v-row>
            <Summoners v-if="summonersList != null" :summonerList="summonerList"></Summoners>
            <LeagueGraph></LeagueGraph>
        </v-row>
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from "vue-property-decorator";
    import { IConfig, Client, Summoner, SummonerLeagues } from "@/api/api-client";
    import Summoners from "@/components/Summoners.vue";
    import LeagueGraph from "@/components/LeagueGraph.vue";
    @Component({
        components: {
            Summoners,
            LeagueGraph            
        }
    })
    export default class LeagueGraphView extends Vue {
        name: string = "";
        summoner: Summoner | null = null;
        summonerList: Summoner[] | null = null;
        summonersLeaguesList: SummonerLeagues[] | null = null;

        //#region Hooks
        async mounted() {
            this.getSummonerList();
        }
        //#endregion

        get apiClient() {
            return new Client(new IConfig(""), "https://localhost:5001");
        }

        async getSummonerList() {
            try {
                const response = await this.apiClient.getAllSummoners();

                if (response.httpStatusCode === 200) {
                    this.summonerList = response.result!;
                }
            }
            catch (error) {
                console.log();
            }
            finally {
                console.log();
            }
        }

        async getSummoner(name: string) {
            try {
                const response = await this.apiClient.getSummoner(name);

                if (response.httpStatusCode === 200) {
                    this.summoner = response.result!;
                }
            }
            catch (error) {
                console.log();
            }
            finally {
                console.log();
            }
        }

        async getAllSummonersLeagues() {
            try {
                const response = await this.apiClient.getAllSummonersLeagues();

                if (response.httpStatusCode === 200) {
                    this.summonersLeaguesList = response.result!;
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
