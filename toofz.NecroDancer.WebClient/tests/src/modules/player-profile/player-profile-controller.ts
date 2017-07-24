import * as sinon from 'sinon';

import * as angular from 'angular';
import 'angular-mocks';

import '../../../../src/modules/player-profile/player-profile.module';
import { PlayerProfileController } from '../../../../src/modules/player-profile/player-profile-controller';
import { BackendDefinition } from '../../../shared';

const toofzSite_definitions = require('../toofz-site-api/toofz-site-api-definitions.json').definitions;
const toofz_definitions = require('../toofz-rest-api/toofz-rest-api.definitions.json').definitions;

import Categories = toofzSite.Leaderboard.Categories;
import PlayerEntries = toofz.PlayerEntries;

describe('PlayerProfileController', function () {
    let $componentController: angular.IComponentControllerService;
    let data: PlayerEntries;
    let categories: Categories;

    beforeEach(function () {
        angular.mock.module('necrodancer.player-profile');

        inject((_$componentController_: any) => {
            $componentController = _$componentController_;
        });

        data = toofz_definitions.find((value: BackendDefinition) => value.description === 'getPlayerEntries').response.data;
        categories = toofzSite_definitions.find((value: BackendDefinition) => value.description === 'getLeaderboardCategories').response.data.categories;
    });

    describe('$onInit', function () {
        let pageTitle_set: sinon.SinonSpy;

        beforeEach(function () {
            inject((pageTitle: any) => {
                pageTitle_set = sinon.spy(pageTitle, 'set');
            });
        });

        it(`should initialize component`, function () {
            const ctrl = $componentController('ndPlayerProfile', {}, {
                data: data,
                categories: categories
            }) as PlayerProfileController;

            ctrl.$onInit();

            pageTitle_set.should.have.been.called;
            ctrl.leaderboardGroups.should.be.an('array');
            const _leaderboardGroups = ctrl.leaderboardGroups as any;
            _leaderboardGroups.updated_at.should.be.a('string');
        });
    });

    describe('setTitle', function () {
        let pageTitle_set: sinon.SinonSpy;

        beforeEach(function () {
            inject((pageTitle: any) => {
                pageTitle_set = sinon.spy(pageTitle, 'set');
            });
        });

        it(`should set title to 'player.display_name' if 'player.display_name' exists`, function () {
            const data = {
                player: {
                    display_name: 'Mendayen'
                }
            };
            const ctrl = $componentController('ndPlayerProfile', {}, {
                data: data,
                categories: categories
            }) as PlayerProfileController;
            const _ctrl = ctrl as any;

            _ctrl.setTitle();

            pageTitle_set.should.have.been.calledWith('Mendayen');
        });

        it(`should set title to 'player.id' if 'player.display_name' does not exist`, function () {
            const data = {
                player: {
                    id: '76561197960481221'
                }
            };
            const ctrl = $componentController('ndPlayerProfile', {}, {
                data: data,
                categories: categories
            }) as PlayerProfileController;
            const _ctrl = ctrl as any;

            _ctrl.setTitle();

            pageTitle_set.should.have.been.calledWith('76561197960481221');
        });
    });

    describe('getGroupKey', function () {
        it(`should return '{run_name} ({product_name})' if 'entry.leaderboard.mode' is 'standard'`, function () {
            const ctrl = $componentController('ndPlayerProfile', {}, {
                categories: categories
            }) as PlayerProfileController;
            const _ctrl = ctrl as any;

            const entry = {
                leaderboard: {
                    product: 'amplified',
                    mode: 'standard',
                    run: 'score'
                }
            };

            const key = _ctrl.getGroupKey(entry);

            key.should.equal('Score (Amplified)');
        });

        it(`should return '{mode_name} {run_name} ({product_name})' if 'entry.leaderboard.mode' is not 'standard'`, function () {
            const ctrl = $componentController('ndPlayerProfile', {}, {
                categories: categories
            }) as PlayerProfileController;
            const _ctrl = ctrl as any;

            const entry = {
                leaderboard: {
                    product: 'amplified',
                    mode: 'no-return',
                    run: 'score'
                }
            };

            const key = _ctrl.getGroupKey(entry);

            key.should.equal('No Return Score (Amplified)');
        });
    });

    describe('mapLeaderboardGroup', function () {
        it(`should return a 'LeaderboardGroup' object`, function () {
            const ctrl = $componentController('ndPlayerProfile', {}, {
                categories: categories
            }) as PlayerProfileController;
            const _ctrl = ctrl as any;

            const entry = {
                leaderboard: {
                    product: 'amplified',
                    mode: 'no-return',
                    run: 'score',
                    character: 'cadence'
                }
            };
            const entries = [entry];

            const leaderboardGroup = _ctrl.mapLeaderboardGroup(entries, '');

            leaderboardGroup.should.be.an('object');
        });
    });

    describe('compareProduct', function () {
        it(`should return 0 if 'product' is 'classic'`, function () {
            const ctrl = $componentController('ndPlayerProfile', {}, {
                categories: categories
            }) as PlayerProfileController;
            const _ctrl = ctrl as any;

            const leaderboardGroup = {
                leaderboard: {
                    product: 'classic'
                }
            };

            const value = _ctrl.compareProduct(leaderboardGroup);

            value.should.equal(0);
        });

        it(`should return 1 if 'product' is 'amplified'`, function () {
            const ctrl = $componentController('ndPlayerProfile', {}, {
                categories: categories
            }) as PlayerProfileController;
            const _ctrl = ctrl as any;

            const leaderboardGroup = {
                leaderboard: {
                    product: 'amplified'
                }
            };

            const value = _ctrl.compareProduct(leaderboardGroup);

            value.should.equal(1);
        });

        it(`should return -1 otherwise`, function () {
            const ctrl = $componentController('ndPlayerProfile', {}, {
                categories: categories
            }) as PlayerProfileController;
            const _ctrl = ctrl as any;

            const leaderboardGroup = {
                leaderboard: {
                    product: ''
                }
            };

            const value = _ctrl.compareProduct(leaderboardGroup);

            value.should.equal(-1);
        });
    });

    describe('compareMode', function () {
        it(`should return 0 if 'mode' is 'standard'`, function () {
            const ctrl = $componentController('ndPlayerProfile', {}, {
                categories: categories
            }) as PlayerProfileController;
            const _ctrl = ctrl as any;

            const leaderboardGroup = {
                leaderboard: {
                    mode: 'standard'
                }
            };

            const value = _ctrl.compareMode(leaderboardGroup);

            value.should.equal(0);
        });

        it(`should return 1 if 'mode' is 'no-return'`, function () {
            const ctrl = $componentController('ndPlayerProfile', {}, {
                categories: categories
            }) as PlayerProfileController;
            const _ctrl = ctrl as any;

            const leaderboardGroup = {
                leaderboard: {
                    mode: 'no-return'
                }
            };

            const value = _ctrl.compareMode(leaderboardGroup);

            value.should.equal(1);
        });

        it(`should return 2 if 'mode' is 'hard'`, function () {
            const ctrl = $componentController('ndPlayerProfile', {}, {
                categories: categories
            }) as PlayerProfileController;
            const _ctrl = ctrl as any;

            const leaderboardGroup = {
                leaderboard: {
                    mode: 'hard'
                }
            };

            const value = _ctrl.compareMode(leaderboardGroup);

            value.should.equal(2);
        });

        it(`should return -1 otherwise`, function () {
            const ctrl = $componentController('ndPlayerProfile', {}, {
                categories: categories
            }) as PlayerProfileController;
            const _ctrl = ctrl as any;

            const leaderboardGroup = {
                leaderboard: {
                    mode: ''
                }
            };

            const value = _ctrl.compareMode(leaderboardGroup);

            value.should.equal(-1);
        });
    });

    describe('compareRun', function () {
        it(`should return 0 if 'run' is 'score'`, function () {
            const ctrl = $componentController('ndPlayerProfile', {}, {
                categories: categories
            }) as PlayerProfileController;
            const _ctrl = ctrl as any;

            const leaderboardGroup = {
                leaderboard: {
                    run: 'score'
                }
            };

            const value = _ctrl.compareRun(leaderboardGroup);

            value.should.equal(0);
        });

        it(`should return 1 if 'run' is 'speed'`, function () {
            const ctrl = $componentController('ndPlayerProfile', {}, {
                categories: categories
            }) as PlayerProfileController;
            const _ctrl = ctrl as any;

            const leaderboardGroup = {
                leaderboard: {
                    run: 'speed'
                }
            };

            const value = _ctrl.compareRun(leaderboardGroup);

            value.should.equal(1);
        });

        it(`should return 2 if 'run' is 'seeded-score'`, function () {
            const ctrl = $componentController('ndPlayerProfile', {}, {
                categories: categories
            }) as PlayerProfileController;
            const _ctrl = ctrl as any;

            const leaderboardGroup = {
                leaderboard: {
                    run: 'seeded-score'
                }
            };

            const value = _ctrl.compareRun(leaderboardGroup);

            value.should.equal(2);
        });

        it(`should return 3 if 'run' is 'seeded-speed'`, function () {
            const ctrl = $componentController('ndPlayerProfile', {}, {
                categories: categories
            }) as PlayerProfileController;
            const _ctrl = ctrl as any;

            const leaderboardGroup = {
                leaderboard: {
                    run: 'seeded-speed'
                }
            };

            const value = _ctrl.compareRun(leaderboardGroup);

            value.should.equal(3);
        });

        it(`should return 4 if 'run' is 'deathless'`, function () {
            const ctrl = $componentController('ndPlayerProfile', {}, {
                categories: categories
            }) as PlayerProfileController;
            const _ctrl = ctrl as any;

            const leaderboardGroup = {
                leaderboard: {
                    run: 'deathless'
                }
            };

            const value = _ctrl.compareRun(leaderboardGroup);

            value.should.equal(4);
        });

        it(`should return -1 otherwise`, function () {
            const ctrl = $componentController('ndPlayerProfile', {}, {
                categories: categories
            }) as PlayerProfileController;
            const _ctrl = ctrl as any;

            const leaderboardGroup = {
                leaderboard: {
                    run: ''
                }
            };

            const value = _ctrl.compareRun(leaderboardGroup);

            value.should.equal(-1);
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
            const ctrl = $componentController('ndPlayerProfile', {}) as PlayerProfileController;

            ctrl.$onDestroy();

            pageTitle_unset.should.have.been.called;
        });
    });
});
