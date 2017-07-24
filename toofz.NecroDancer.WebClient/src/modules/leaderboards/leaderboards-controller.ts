import * as _ from 'lodash';

import * as angular from 'angular';

import { PageTitle } from '../page-title/page-title';

import Categories = toofzSite.Leaderboard.Categories;
import Leaderboard = toofz.Leaderboard;

export class LeaderboardsController {
    constructor(private readonly pageTitle: PageTitle,
                private readonly $state: angular.ui.IStateService) {
        'ngInject';
        this.title = this.pageTitle.set(this.title);
    }

    title = 'Leaderboards';

    readonly categories: Categories;
    readonly leaderboards: Leaderboard[];

    $onDestroy() {
        this.pageTitle.unset();
    }

    search() {
        this._search_debounced();
    }

    private _search() {
        this.$state.go('.', {
            categories: this.categories
        }, {
            reload: true
        });
    }

    readonly _search_debounced = _.debounce(this._search.bind(this), 350);
}
