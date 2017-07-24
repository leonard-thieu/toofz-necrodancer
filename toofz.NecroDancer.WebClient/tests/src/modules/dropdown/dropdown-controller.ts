import * as angular from 'angular';
import 'angular-mocks';

import '../../../../src/modules/dropdown/dropdown.module';

describe('DropdownController', function () {
    let $componentController: angular.IComponentControllerService;

    beforeEach(function () {
        angular.mock.module('necrodancer.dropdown');

        inject((_$componentController_: any) => {
            $componentController = _$componentController_;
        });
    });

    describe('constructor', function () {
        it(`should assign bindings`, function () {
            const category = {};
            const ctrl = $componentController('ndDropdown', {}, {
                category: category
            }) as any;

            ctrl.category.should.equal(category);
        });
    });
});