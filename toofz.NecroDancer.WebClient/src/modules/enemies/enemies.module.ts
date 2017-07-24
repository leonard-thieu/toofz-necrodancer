import * as angular from 'angular';
import 'angular-ui-router';
import '../page-title/page-title.module';
import '../pagination/pagination.module';

import { EnemiesController } from './enemies-controller';

/**
 * @ngdoc directive
 * @name ndEnemies
 * @restrict E
 *
 * @param {expression} data
 */

angular
    .module('necrodancer.enemies', [
        'ui.router',
        'necrodancer.page-title',
        'necrodancer.pagination'
    ])
    .component('ndEnemies', {
        templateUrl: __dirname + '/enemies.html',
        controller: EnemiesController,
        bindings: {
            data: '<'
        }
    });