import * as sinon from 'sinon';

import * as angular from 'angular';
import 'angular-mocks';

import '../../../../src/modules/leaderboards/leaderboards.module';
import { LeaderboardsController } from '../../../../src/modules/leaderboards/leaderboards-controller';

describe('LeaderboardsController', function () {
    let $componentController: angular.IComponentControllerService;

    beforeEach(function () {
        angular.mock.module('necrodancer.leaderboards', {
            apiBaseUrl: ''
        });

        inject((_$componentController_: any) => {
            $componentController = _$componentController_;
        });
    });

    describe('constructor', function () {
        let pageTitle_set: sinon.SinonSpy;

        beforeEach(function () {
            inject((pageTitle: any) => {
                pageTitle_set = sinon.spy(pageTitle, 'set');
            });
        });

        it(`should set title`, function () {
            const ctrl = $componentController('ndLeaderboards', {}) as LeaderboardsController;

            pageTitle_set.should.have.been.called;
            ctrl.title.should.equal('Leaderboards');
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
            const ctrl = $componentController('ndLeaderboards', {}) as LeaderboardsController;

            ctrl.$onDestroy();

            pageTitle_unset.should.have.been.called;
        });
    });

    describe('search', function () {
        it(`should reload state with 'categories'`, function () {
            const ctrl = $componentController('ndLeaderboards', {}) as LeaderboardsController;
            const _search_debounced = sinon.stub(ctrl, '_search_debounced');

            ctrl.search();

            _search_debounced.should.have.been.called;
        });
    });

    describe('_search', function () {
        let $state_go: sinon.SinonStub;

        beforeEach(function () {
            inject(($state: any) => {
                $state_go = sinon.stub($state, 'go');
            });
        });

        it(`should reload current state with 'categories' object`, function () {
            const ctrl = $componentController('ndLeaderboards', {}) as LeaderboardsController;
            const _ctrl = ctrl as any;

            _ctrl._search();

            $state_go.should.have.been.called;
        });
    });
});