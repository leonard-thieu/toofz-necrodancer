import * as angular from 'angular';
import 'angular-mocks';

import '../../../../src/modules/navbar/navbar.module';
import { NavbarController } from '../../../../src/modules/navbar/navbar-controller';

describe('NavbarController', function () {
    let $componentController: angular.IComponentControllerService;

    beforeEach(function () {
        angular.mock.module('necrodancer.navbar');

        inject((_$componentController_: any) => {
            $componentController = _$componentController_;
        });
    });

    describe('constructor', function () {
        it(`should assign bindings`, function () {
            const apiBaseUrl = 'http://localhost';
            const areas: any[] = [];
            const ctrl = $componentController('ndNavbar', {
                apiBaseUrl: apiBaseUrl
            }, {
                areas: areas
            }) as NavbarController;

            ctrl.apiBaseUrl.should.equal(apiBaseUrl);
            ctrl.areas.should.equal(areas);
        });
    });
});