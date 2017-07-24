import * as angular from 'angular';
import 'angular-ui-router';
import '../entry-filters/entry-filters.module';
import '../slug/slug.module';
import '../titlecase/titlecase.module';
import '../page-title/page-title.module';
import '../pagination/pagination.module';

import { LeaderboardController } from './leaderboard-controller';

/**
 * @ngdoc directive
 * @name ndLeaderboard
 * @restrict E
 *
 * @param {expression} playerEntry
 * @param {expression} data
 */

angular
    .module('necrodancer.leaderboard', [
        'ui.router',
        'necrodancer.page-title',
        'necrodancer.entry-filters',
        'necrodancer.pagination',
        'necrodancer.titlecase',
        'necrodancer.slug'
    ])
    .component('ndLeaderboard', {
        templateUrl: __dirname + '/leaderboard.html',
        controller: LeaderboardController,
        bindings: {
            playerEntry: '<',
            data: '<'
        }
    });