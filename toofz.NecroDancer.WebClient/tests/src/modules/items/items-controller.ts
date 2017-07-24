import * as sinon from 'sinon';

import * as angular from 'angular';
import 'angular-mocks';

import '../../../../src/modules/items/items.module';
import { ItemsController } from '../../../../src/modules/items/items-controller';

describe('ItemsController', function () {
    let $componentController: angular.IComponentControllerService;
    let $stateParams: angular.ui.IStateParamsService;

    beforeEach(function () {
        angular.mock.module('necrodancer.items');

        inject((_$componentController_: any,
                _$stateParams_: any) => {
            $componentController = _$componentController_;
            $stateParams = _$stateParams_;
        });
    });

    describe('constructor', function () {
        it(`should set 'title' to 'Items' if '$stateParams.category' is not defined`, function () {
            const ctrl = $componentController('ndItems', {}) as ItemsController;

            ctrl.title.should.equal('Items');
        });

        it(`should set 'title' to '{category}' if '$stateParams.category' is defined`, function () {
            $stateParams.category = 'weapons';
            const ctrl = $componentController('ndItems', {}) as ItemsController;

            ctrl.title.should.equal('Weapons');
        });

        it(`should set 'title' to '{category} ({subcategory})' if '$stateParams.category' and '$stateParams.subcategory' is defined`, function () {
            $stateParams.category = 'weapons';
            $stateParams.subcategory = 'bows';
            const ctrl = $componentController('ndItems', {}) as ItemsController;

            ctrl.title.should.equal('Weapons (Bows)');
        });
    });

    describe('$onDestroy', function () {
        let pageTitle_unset: sinon.SinonSpy;

        beforeEach(function () {
            inject((pageTitle: any) => {
                pageTitle_unset = sinon.spy(pageTitle, 'unset');
            });
        });

        it(`should call 'pageTitle.unset()'`, function () {
            const ctrl = $componentController('ndItems', {}) as ItemsController;

            ctrl.$onDestroy();

            pageTitle_unset.should.have.been.called;
        });
    });
});