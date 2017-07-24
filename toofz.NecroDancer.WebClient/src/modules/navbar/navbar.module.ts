import * as angular from 'angular';
import '../dropdown/dropdown.module';
import '../toofz-site-api/toofz-site-api.module';

import { NavbarController } from './navbar-controller';

/**
 * @ngdoc directive
 * @name ndNavbar
 * @restrict E
 *
 * @param {expression} areas
 */

angular
    .module('necrodancer.navbar', [
        'necrodancer.dropdown',
        'toofz.site-api'
    ])
    .component('ndNavbar', {
        templateUrl: __dirname + '/navbar.html',
        controller: NavbarController,
        bindings: {
            areas: '<'
        }
    });