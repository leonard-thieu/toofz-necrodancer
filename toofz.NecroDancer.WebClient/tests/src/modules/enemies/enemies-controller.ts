import * as sinon from 'sinon';

import * as angular from 'angular';
import 'angular-mocks';

import '../../../../src/modules/enemies/enemies.module';
import { EnemiesController } from '../../../../src/modules/enemies/enemies-controller';

describe('EnemiesController', function () {
    let $componentController: angular.IComponentControllerService;
    let $stateParams: angular.ui.IStateParamsService;

    beforeEach(function () {
        angular.mock.module('necrodancer.enemies');

        inject((_$componentController_: any,
                _$stateParams_: any) => {
            $componentController = _$componentController_;
            $stateParams = _$stateParams_;
        });
    });

    describe('constructor', function () {
        it(`should set 'title' to 'Enemies' if '$stateParams.attribute' is not defined`, function () {
            const ctrl = $componentController('ndEnemies', {}) as EnemiesController;

            ctrl.title.should.equal('Enemies');
        });

        it(`should set 'title' to '{attribute} Enemies' if '$stateParams.attribute' is defined`, function () {
            $stateParams.attribute = 'floating';
            const ctrl = $componentController('ndEnemies', {}) as EnemiesController;

            ctrl.title.should.equal('Floating Enemies');
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
            const ctrl = $componentController('ndEnemies', {}) as EnemiesController;

            ctrl.$onDestroy();

            pageTitle_unset.should.have.been.called;
        });
    });
});