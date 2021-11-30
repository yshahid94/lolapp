<template>
        <v-col cols="12"
               sm="6">
            <v-card>
                <v-list two-line>
                    <template v-for="(item, index) in formatedSummonerList">
                        <v-subheader v-if="item.header"
                                     :key="item.header">
                            {{ item.header }}
                        </v-subheader>
                        <v-divider v-else-if="item.divider"
                                   :key="index"
                                   :inset="item.inset"></v-divider>
                        <v-list-item v-else
                                     :key="item.summonerId">
                            <v-list-item-avatar>
                                <img :src="item.avatar">
                            </v-list-item-avatar>
                            <v-list-item-content>
                                <v-list-item-title v-html="item.name"></v-list-item-title>
                                <!--<v-list-item-subtitle v-html="item.subtitle"></v-list-item-subtitle>-->
                            </v-list-item-content>
                        </v-list-item>
                    </template>
                </v-list>
            </v-card>
        </v-col>
</template>

<script lang="ts">
    import { Component, Vue, Prop } from "vue-property-decorator";
    import { Summoner } from "@/api/api-client";
    @Component({})
    export default class Summoners extends Vue {
        @Prop() summonerList!: Summoner[];
        get formatedSummonerList() {
            var list = [];
            list.push({ header: 'Summoner list' });
            this.summonerList.forEach(x => {
                list.push( x );
                list.push({ divider: true, insert: true });
            });
            list.pop();
            return list;
        }
    }
</script>