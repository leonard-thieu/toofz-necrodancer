import * as util from '../../util';

import * as angular from 'angular';

import { PageTitle } from '../page-title/page-title';
import { PagingControllerBase } from '../pagination/paging-controller-base';
import { SlugFilter } from '../slug/slug.module';

export class LeaderboardController extends PagingControllerBase<toofz.LeaderboardEntries> {
    constructor($stateParams: angular.ui.IStateParamsService,
                private readonly pageTitle: PageTitle,
                private readonly slugFilter: SlugFilter) {
        'ngInject';
        super($stateParams);
    }

    title = 'Leaderboard';

    readonly playerEntry: toofz.Entry;
    limit = 20;

    $onInit() {
        super.$onInit();

        this.title = `${this.data.leaderboard.display_name} ${this.title}`;
        this.title = this.pageTitle.set(this.title);

        if (typeof this.records.offset !== 'number' && this.playerEntry) {
            this.records.offset = util.roundDownToMultiple(this.playerEntry.rank - 1, this.limit);
        }

        for (const entry of this.data.entries) {
            let p = entry.player as any;
            p.slug = this.slugFilter(entry.player!.display_name!);
        }
    }

    $onDestroy() {
        this.pageTitle.unset();
    }
}