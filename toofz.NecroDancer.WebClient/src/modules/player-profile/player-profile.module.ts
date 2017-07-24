import * as angular from 'angular';
import '../entry-filters/entry-filters.module';
import '../ordinal/ordinal.module';
import '../page-title/page-title.module';

import { PlayerProfileController } from './player-profile-controller';

/**
 * @ngdoc directive
 * @name ndPlayerProfile
 * @restrict E
 *
 * @param {expression} data
 * @param {expression} categories
 */

angular
    .module('necrodancer.player-profile', [
        'necrodancer.entry-filters',
        'necrodancer.ordinal',
        'necrodancer.page-title'
    ])
    .component('ndPlayerProfile', {
        templateUrl: __dirname + '/player-profile.html',
        controller: PlayerProfileController,
        bindings: {
            data: '<',
            categories: '<'
        }
    });