<template>
    <v-col cols="12"
            sm="6">
        <v-card>
            hio
        </v-card>
    </v-col>
</template>

<script lang="ts">
    import { Component, Vue, Prop } from "vue-property-decorator";
    import { Summoner } from "@/api/api-client";
    @Component({})
    export default class LeagueGraph extends Vue {
        @Prop() summonerList!: Summoner[];
    }
</script>