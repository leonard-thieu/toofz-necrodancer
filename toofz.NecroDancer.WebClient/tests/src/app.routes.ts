import * as sinon from 'sinon';

import * as angular from 'angular';
import 'angular-mocks';

import '../../src/app.module';
import { ToofzRestApi } from '../../src/modules/toofz-rest-api/toofz-rest-api';
import { ToofzSiteApi } from '../../src/modules/toofz-site-api/toofz-site-api';
import { BackendDefinition } from '../shared';

const toofzSite_definitions = require('./modules/toofz-site-api/toofz-site-api-definitions.json').definitions;

describe('necrodancer (Routes)', function () {
    // http://nikas.praninskas.com/angular/2014/09/27/unit-testing-ui-router-configuration/
    function mockTemplate<T>(templateRoute: string, template?: T) {
        $templateCache.put(templateRoute, template || templateRoute);
    }

    function goTo(url: string) {
        $location.url(url);
        $rootScope.$digest();
    }

    function goFrom(url: string) {
        return {
            toState: function (state: string, params?: any) {
                $location.replace().url(url); // Don't actually trigger a reload
                $state.go(state, params);
                $rootScope.$digest();
            }
        };
    }

    function resolve(value: string) {
        return {
            forStateAndView: function (state: string, view?: string, locals?: any) {
                const viewDefinition = view ?
                    $state.get(state).views![view] :
                    $state.get(state);

                return $injector.invoke(viewDefinition.resolve![value], undefined, locals);
            },
            forState: function (this: any, state: string, locals?: any) {
                return this.forStateAndView(state, undefined, locals);
            }
        };
    }

    let $templateCache: angular.ITemplateCacheService;
    let $location: angular.ILocationService;
    let $state: angular.ui.IStateService;
    let $rootScope: angular.IRootScopeService;
    let $injector: angular.auto.IInjectorService;
    let $stateParams: angular.ui.IStateParamsService;
    let toofzSiteApi: ToofzSiteApi;
    let toofzRestApi: ToofzRestApi;

    beforeEach(function () {
        angular.mock.module('necrodancer.app');

        inject((_$templateCache_: any,
                _$location_: any,
                _$state_: any,
                _$rootScope_: any,
                _$injector_: any,
                _$stateParams_: any,
                _toofzSiteApi_: any,
                _toofzRestApi_: any) => {
            $templateCache = _$templateCache_;
            $location = _$location_;
            $state = _$state_;
            $rootScope = _$rootScope_;
            $injector = _$injector_;
            $stateParams = _$stateParams_;
            toofzSiteApi = _toofzSiteApi_;
            toofzRestApi = _toofzRestApi_;
        });
    });

    describe('root', function () {
        const state = 'root';

        describe('url', function () {
            it(`should return ''`, function () {
                const url = $state.href(state);

                url.should.equal('');
            });
        });

        describe('resolve', function () {
            let toofzSiteApi_getAreas: sinon.SinonStub;

            beforeEach(function () {
                toofzSiteApi_getAreas = sinon.stub(toofzSiteApi, 'getAreas');
            });

            it(`should call 'toofzSiteApi.getAreas()'`, function () {
                toofzSiteApi_getAreas.returns(Promise.resolve({ data: {} }));

                return resolve('areas').forState(state).should.be.fulfilled;
            });
        });
    });

    describe('root.landing', function () {
        const state = 'root.landing';

        describe('url', function () {
            it(`should return ''`, function () {
                const url = $state.href(state);

                url.should.equal('');
            });
        });
    });

    describe('root.items', function () {
        const state = 'root.items';

        describe('url', function () {
            it(`should return '/items' if no params supplied`, function () {
                const url = $state.href(state);

                url.should.equal('/items');
            });

            it(`should return '/items/{category}' if 'category' is defined`, function () {
                const url = $state.href(state, { category: 'weapons' });

                url.should.equal('/items/weapons');
            });

            it(`should return '/items/{category}/{subcategory}' if 'subcategory' is defined`, function () {
                const url = $state.href(state, { category: 'weapons', subcategory: 'bows' });

                url.should.equal('/items/weapons/bows');
            });

            it(`should return '/items?page={page}' if 'page' is defined`, function () {
                const url = $state.href(state, { page: 2 });

                url.should.equal('/items?page=2');
            });
        });

        describe('resolve', function () {
            describe('items', function () {
                let toofzRestApi_getItems: sinon.SinonStub;
                let toofzRestApi_getItemsByCategory: sinon.SinonStub;
                let toofzRestApi_getItemsBySubcategory: sinon.SinonStub;

                beforeEach(function () {
                    toofzRestApi_getItems = sinon.stub(toofzRestApi, 'getItems');
                    toofzRestApi_getItemsByCategory = sinon.stub(toofzRestApi, 'getItemsByCategory');
                    toofzRestApi_getItemsBySubcategory = sinon.stub(toofzRestApi, 'getItemsBySubcategory');
                });

                it(`should call 'toofzRestApi.getItems()' if 'category' is not defined`, function () {
                    toofzRestApi_getItems.returns(Promise.resolve());

                    const items = resolve('items').forState(state).should.be.fulfilled;
                    toofzRestApi_getItems.should.have.been.called;

                    return items;
                });

                it(`should call 'toofzRestApi.getItemsByCategory()' if 'category' is defined and 'subcategory' is not defined`, function () {
                    $stateParams.category = 'weapons';

                    toofzRestApi_getItemsByCategory.returns(Promise.resolve());

                    const items = resolve('items').forState(state).should.be.fulfilled;
                    toofzRestApi_getItemsByCategory.should.have.been.called;

                    return items;
                });

                it(`should call 'toofzRestApi.getItemsBySubcategory()' if 'category' is defined and 'subcategory' is defined`, function () {
                    $stateParams.category = 'weapons';
                    $stateParams.subcategory = 'bows';

                    toofzRestApi_getItemsBySubcategory.returns(Promise.resolve());

                    const items = resolve('items').forState(state).should.be.fulfilled;
                    toofzRestApi_getItemsBySubcategory.should.have.been.called;

                    return items;
                });
            });
        });
    });

    describe('root.enemies', function () {
        const state = 'root.enemies';

        describe('url', function () {
            it(`should return '/enemies' if no params supplied`, function () {
                const url = $state.href(state);

                url.should.equal('/enemies');
            });

            it(`should return '/enemies?page={page}' if page is defined`, function () {
                const url = $state.href(state, { page: 2 });

                url.should.equal('/enemies?page=2');
            });

            it(`should return '/enemies/{attribute}' if 'attribute' is defined`, function () {
                const url = $state.href(state, { attribute: 'floating' });

                url.should.equal('/enemies/floating');
            });
        });

        describe('resolve', function () {
            describe('enemies', function () {
                let toofzRestApi_getEnemies: sinon.SinonStub;
                let toofzRestApi_getEnemiesByAttribute: sinon.SinonStub;

                beforeEach(function () {
                    toofzRestApi_getEnemies = sinon.stub(toofzRestApi, 'getEnemies');
                    toofzRestApi_getEnemiesByAttribute = sinon.stub(toofzRestApi, 'getEnemiesByAttribute');
                });

                it(`should call 'toofzRestApi.getEnemies()' if 'attribute' is not defined`, function () {
                    toofzRestApi_getEnemies.returns(Promise.resolve());

                    const enemies = resolve('enemies').forState(state).should.be.fulfilled;
                    toofzRestApi_getEnemies.should.have.been.called;

                    return enemies;
                });

                it(`should call 'toofzRestApi.getEnemiesByAttribute()' if 'attribute' is defined`, function () {
                    $stateParams.attribute = 'floating';
                    toofzRestApi_getEnemiesByAttribute.returns(Promise.resolve());

                    const enemies = resolve('enemies').forState(state).should.be.fulfilled;
                    toofzRestApi_getEnemiesByAttribute.should.have.been.called;

                    return enemies;
                });
            });
        });
    });

    describe('root.leaderboards', function () {
        const state = 'root.leaderboards';

        describe('url', function () {
            it(`should return '/leaderboards'`, function () {
                const url = $state.href(state);

                url.should.equal('/leaderboards');
            });
        });

        describe('resolve', function () {
            describe('categories', function () {
                let toofzSiteApi_getLeaderboardCategories: sinon.SinonStub;

                beforeEach(function () {
                    toofzSiteApi_getLeaderboardCategories = sinon.stub(toofzSiteApi, 'getLeaderboardCategories');
                });

                it(`should return '$stateParams.categories' if it exists`, function () {
                    $stateParams.categories = {};

                    return resolve('categories').forState(state).should.equal($stateParams.categories);
                });

                it(`should return a default categories object if '$stateParams.categories' doesn't exist`, function () {
                    const categories = toofzSite_definitions.find((value: BackendDefinition) => value.description === 'getLeaderboardCategories');
                    toofzSiteApi_getLeaderboardCategories.returns(Promise.resolve(categories.response));

                    return resolve('categories').forState(state).should.eventually.be.an('object');
                });
            });

            describe('leaderboards', function () {
                let toofzRestApi_getLeaderboards: sinon.SinonStub;

                beforeEach(function () {
                    toofzRestApi_getLeaderboards = sinon.stub(toofzRestApi, 'getLeaderboards');
                });

                it(`should resolve data`, function () {
                    const categories = toofzSite_definitions.find((value: BackendDefinition) => value.description === 'getLeaderboardCategories');
                    toofzRestApi_getLeaderboards.returns(Promise.resolve({
                        leaderboards: [],
                    }));

                    return resolve('leaderboards').forState(state, {
                        categories: categories.response.data.categories
                    }).should.eventually.be.an('array');
                });
            });
        });
    });

    describe('root.leaderboard', function () {
        const state = 'root.leaderboard';

        describe('url', function () {

        });

        describe('resolve', function () {
            describe('headers', function () {
                let toofzSiteApi_getLeaderboardHeaders: sinon.SinonStub;

                beforeEach(function () {
                    toofzSiteApi_getLeaderboardHeaders = sinon.stub(toofzSiteApi, 'getLeaderboardHeaders');
                });

                it(`should resolve data`, function () {
                    toofzSiteApi_getLeaderboardHeaders.returns(Promise.resolve({
                        data: {}
                    }));

                    return resolve('headers').forState(state).should.be.fulfilled;
                });
            });

            describe('categories', function () {
                let toofzSiteApi_getLeaderboardCategories: sinon.SinonStub;

                beforeEach(function () {
                    toofzSiteApi_getLeaderboardCategories = sinon.stub(toofzSiteApi, 'getLeaderboardCategories');
                });

                it(`should resolve data`, function () {
                    toofzSiteApi_getLeaderboardCategories.returns(Promise.resolve({
                        data: {}
                    }));

                    return resolve('categories').forState(state).should.be.fulfilled;
                });
            });

            describe('header', function () {
                it(`should resolve data`, function () {
                    $stateParams.product = 'amplified';
                    $stateParams.character = 'cadence';
                    $stateParams.mode = 'standard';
                    $stateParams.run = 'speed';

                    const headers = toofzSite_definitions.find((value: BackendDefinition) => value.description === 'getLeaderboardHeaders');
                    const categories = toofzSite_definitions.find((value: BackendDefinition) => value.description === 'getLeaderboardCategories');

                    resolve('header').forState(state, {
                        headers: headers.response.data.leaderboards,
                        categories: categories.response.data.categories
                    }).should.be.an('object');
                });
            });

            describe('player', function () {
                let toofzRestApi_getPlayerLeaderboardEntry: sinon.SinonStub;

                beforeEach(function () {
                    toofzRestApi_getPlayerLeaderboardEntry = sinon.stub(toofzRestApi, 'getPlayerLeaderboardEntry');
                });

                it(`should return undefined if 'id' doesn't exist`, function () {
                    const header = {};

                    const player = resolve('player').forState(state, { header: header });

                    should.not.exist(player);
                });

                it(`should return an 'Entry' object if 'id' exists`, function () {
                    $stateParams.id = '76561197960481221';
                    const header = { id: 1694062 };
                    toofzRestApi_getPlayerLeaderboardEntry.returns(Promise.resolve({}));

                    const player = resolve('player').forState(state, { header: header });

                    return player.should.be.fulfilled;
                });

                it(`should return undefined if an entry for the player could not be found`, function () {
                    $stateParams.id = '76561197960481221';
                    const header = { id: 1694062 };
                    toofzRestApi_getPlayerLeaderboardEntry.returns(Promise.reject({}));

                    const player = resolve('player').forState(state, { header: header });

                    return player.should.eventually.not.exist;
                });
            });

            describe('leaderboard', function () {
                let toofzRestApi_getLeaderboardEntries: sinon.SinonStub;

                beforeEach(function () {
                    toofzRestApi_getLeaderboardEntries = sinon.stub(toofzRestApi, 'getLeaderboardEntries');
                });

                it(`should resolve data`, function () {
                    const header = {};
                    const player = undefined;
                    toofzRestApi_getLeaderboardEntries.returns(Promise.resolve({}));

                    return resolve('leaderboard').forState(state, {
                        header: header,
                        player: player
                    }).should.eventually.be.fulfilled;
                });
            });
        });
    });

    describe('root.player', function () {
        const state = 'root.player';

        describe('url', function () {
            it(`should return '/p/{id}' if 'slug' is not defined`, function () {
                const url = $state.href(state, { id: '76561197960481221' });

                url.should.equal('/p/76561197960481221');
            });

            it(`should return '/p/{id}/{slug}' if 'slug' is defined`, function () {
                const url = $state.href(state, { id: '76561197960481221', slug: 'Mendayen' });

                url.should.equal('/p/76561197960481221/Mendayen');
            });
        });

        describe('resolve', function () {
            describe('player', function () {
                let toofzRestApi_getPlayerEntries: sinon.SinonStub;

                beforeEach(function () {
                    toofzRestApi_getPlayerEntries = sinon.stub(toofzRestApi, 'getPlayerEntries');
                });

                it(`should resolve data`, function () {
                    toofzRestApi_getPlayerEntries.returns(Promise.resolve({ data: {} }));

                    return resolve('player').forState(state).should.be.fulfilled;
                });
            });

            describe('categories', function () {
                let toofzSiteApi_getLeaderboardCategories: sinon.SinonStub;

                beforeEach(function () {
                    toofzSiteApi_getLeaderboardCategories = sinon.stub(toofzSiteApi, 'getLeaderboardCategories');
                });

                it(`should resolve data`, function () {
                    toofzSiteApi_getLeaderboardCategories.returns(Promise.resolve({ data: {} }));

                    return resolve('categories').forState(state).should.be.fulfilled;
                });
            });
        });
    });
});
