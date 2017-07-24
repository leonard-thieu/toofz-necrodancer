import * as _ from 'lodash';

import { PageTitle } from '../page-title/page-title';

import PlayerEntries = toofz.PlayerEntries;
import Categories = toofzSite.Leaderboard.Categories;
import Entry = toofz.Entry;

export class PlayerProfileController {
    constructor(private readonly pageTitle: PageTitle) {
        'ngInject';
    }

    readonly data: PlayerEntries;
    readonly categories: Categories;

    leaderboardGroups: LeaderboardGroup[];

    $onInit() {
        this.setTitle();

        const groupedEntries = _.groupBy(this.data.entries, this.getGroupKey.bind(this));
        const leaderboardGroups = _.map(groupedEntries, this.mapLeaderboardGroup.bind(this));
        this.leaderboardGroups = _.sortBy(leaderboardGroups, [
            this.compareProduct.bind(this),
            this.compareMode.bind(this),
            this.compareRun.bind(this)
        ]) as any as LeaderboardGroup[];

        const _leaderboardGroups = this.leaderboardGroups as any;
        _leaderboardGroups.updated_at = this.leaderboardGroups[0].entries[0].leaderboard!.updated_at;
    }

    private setTitle() {
        const { display_name, id } = this.data.player;
        
        this.pageTitle.set(display_name || id, false);
    }

    private getGroupKey(entry: Entry) {
        const { run, product, mode } = entry.leaderboard!;

        const run_name = this.categories['runs'][run].display_name;
        const product_name = this.categories['products'][product].display_name;

        if (mode === 'standard') {
            return `${run_name} (${product_name})`;
        } else {
            const mode_name = this.categories['modes'][mode].display_name;

            return `${mode_name} ${run_name} (${product_name})`;
        }
    }

    private mapLeaderboardGroup(entries: Entry[], key: string | undefined): LeaderboardGroup {
        for (const entry of entries) {
            const leaderboard = entry.leaderboard as Leaderboard;
            leaderboard._character = this.categories['characters'][leaderboard.character].display_name;
        }

        return {
            display_name: key!,
            leaderboard: entries[0].leaderboard!,
            entries: entries
        };
    }

    private compareProduct(leaderboardGroup: LeaderboardGroup) {
        const product = this.categories['products'][leaderboardGroup.leaderboard.product];

        return product ? product.order : -1;
    }

    private compareMode(leaderboardGroup: LeaderboardGroup) {
        const mode = this.categories['modes'][leaderboardGroup.leaderboard.mode];

        return mode ? mode.order : -1;
    }

    private compareRun(leaderboardGroup: LeaderboardGroup) {
        const run = this.categories['runs'][leaderboardGroup.leaderboard.run];

        return run ? run.order : -1;
    }

    $onDestroy() {
        this.pageTitle.unset();
    }
}

interface LeaderboardGroup {
    display_name: string;
    leaderboard: toofz.Leaderboard;
    entries: toofz.Entry[];
}

interface Leaderboard extends toofz.Leaderboard {
    _character: string;
}