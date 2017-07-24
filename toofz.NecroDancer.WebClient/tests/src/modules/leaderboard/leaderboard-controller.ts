import * as sinon from 'sinon';

import * as angular from 'angular';
import 'angular-mocks';

import '../../../../src/modules/leaderboard/leaderboard.module';
import { LeaderboardController } from '../../../../src/modules/leaderboard/leaderboard-controller';

describe('LeaderboardController', function () {
    let $componentController: angular.IComponentControllerService;

    beforeEach(function () {
        angular.mock.module('necrodancer.leaderboard');

        inject((_$componentController_: any) => {
            $componentController = _$componentController_;
        });
    });

    describe('$onInit', function () {
        it(`should set 'title' and create slugs for each player`, function () {
            const player = { display_name: 'Elad Difficult' };
            const ctrl = $componentController('ndLeaderboard', {}, {
                data: {
                    leaderboard: {
                        display_name: 'Cadence Score'
                    },
                    entries: [{
                        player: player
                    }]
                }
            }) as LeaderboardController;

            ctrl.$onInit();

            ctrl.title.should.equal('Cadence Score Leaderboard');

            const p = ctrl.data.entries[0].player as any;
            p.slug.should.equal('Elad-Difficult');
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
            const ctrl = $componentController('ndLeaderboard', {}) as LeaderboardController;

            ctrl.$onDestroy();

            pageTitle_unset.should.have.been.called;
        });
    });
});