import * as angular from 'angular';
import 'angular-ui-router';
import '../page-title/page-title.module';
import '../pagination/pagination.module';
import '../titlecase/titlecase.module';

import { ItemsController } from './items-controller';

/**
 * @ngdoc directive
 * @name ndItems
 * @restrict E
 *
 * @param {expression} data
 */

angular
    .module('necrodancer.items', [
        'ui.router',
        'necrodancer.page-title',
        'necrodancer.pagination',
        'necrodancer.titlecase'
    ])
    .component('ndItems', {
        templateUrl: __dirname + '/items.html',
        controller: ItemsController,
        bindings: {
            data: '<'
        }
    });