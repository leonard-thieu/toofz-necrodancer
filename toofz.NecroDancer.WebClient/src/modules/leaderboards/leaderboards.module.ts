import * as angular from 'angular';
import 'angular-ui-router';
import '../page-title/page-title.module';
import '..//toofz-rest-api/toofz-rest-api.module';

import { LeaderboardsController } from './leaderboards-controller';

/**
 * @ngdoc directive
 * @name ndLeaderboards
 * @restrict E
 *
 * @param {expression} categories
 * @param {expression} leaderboards
 */

angular
    .module('necrodancer.leaderboards', [
        'ui.router',
        'necrodancer.page-title',
        'toofz.rest-api'
    ])
    .component('ndLeaderboards', {
        templateUrl: __dirname + '/leaderboards.html',
        controller: LeaderboardsController,
        bindings: {
            categories: '<',
            leaderboards: '<'
        }
    });