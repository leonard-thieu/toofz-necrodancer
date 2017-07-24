import * as angular from 'angular';
import 'angular-mocks';

import '../../../../src/modules/toofz-rest-api/toofz-rest-api.module';
import { ToofzRestApi } from '../../../../src/modules/toofz-rest-api/toofz-rest-api';

const toofz_definitions = require('./toofz-rest-api.definitions.json').definitions;

describe('ToofzRestApi', function () {
    let $httpBackend: angular.IHttpBackendService;
    let toofzRestApi: ToofzRestApi;

    beforeEach(function () {
        angular.mock.module('toofz.rest-api', {
            apiBaseUrl: ''
        });

        inject((_$httpBackend_: any) => {
            $httpBackend = _$httpBackend_;

            for (const definition of toofz_definitions) {
                const { request, response } = definition;
                $httpBackend.when(request.method, request.url)
                    .respond(response.data);
            }
        });

        inject((_toofzRestApi_: any) => {
            toofzRestApi = _toofzRestApi_;
        });
    });

    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    describe('getItems', function () {
        it(`should return successfully`, function () {
            toofzRestApi.getItems().then(data => {
                data.should.have.interface({
                    total: Number,
                    items: Array
                });
            });
            $httpBackend.flush();
        });
    });

    describe('getItemsByCategory', function () {
        it(`should return successfully`, function () {
            toofzRestApi.getItemsByCategory('weapons').then(data => {
                data.should.have.interface({
                    total: Number,
                    items: Array
                });
            });
            $httpBackend.flush();
        });
    });

    describe('getItemsBySubcategory', function () {
        it(`should return successfully`, function () {
            toofzRestApi.getItemsBySubcategory('weapons', 'bows').then(data => {
                data.should.have.interface({
                    total: Number,
                    items: Array
                });
            });
            $httpBackend.flush();
        });
    });

    describe('getEnemies', function () {
        it(`should return successfully`, function () {
            toofzRestApi.getEnemies().then(data => {
                data.should.exist;
            });
            $httpBackend.flush();
        });
    });

    describe('getEnemiesByAttribute', function () {
        it(`should return successfully`, function () {
            toofzRestApi.getEnemiesByAttribute('floating').then(data => {
                data.should.exist;
            });
            $httpBackend.flush();
        });
    });

    describe('getLeaderboards', function () {
        it(`should return successfully`, function () {
            toofzRestApi.getLeaderboards({
                products: ['amplified'],
                modes: ['standard'],
                runs: ['speed'],
                characters: ['cadence']
            }).then(data => {
                data.should.exist;
            });
            $httpBackend.flush();
        });
    });

    describe('getDailyLeaderboards', function () {
        it(`should return successfully`, function () {
            toofzRestApi.getDailyLeaderboards().then(data => {
                data.should.exist;
            });
            $httpBackend.flush();
        });
    });

    describe('getLeaderboardEntries', function () {
        it(`should return successfully`, function () {
            toofzRestApi.getLeaderboardEntries(739999).then(data => {
                data.should.exist;
            });
            $httpBackend.flush();
        });
    });

    describe('getPlayers', function () {
        it(`should return successfully`, function () {
            toofzRestApi.getPlayers('Mendayen').then(data => {
                data.should.exist;
            });
            $httpBackend.flush();
        });
    });

    describe('getPlayerEntries', function () {
        it(`should return successfully`, function () {
            toofzRestApi.getPlayerEntries('76561197960481221').then(data => {
                data.should.exist;
            });
            $httpBackend.flush();
        });
    });

    describe('getPlayerLeaderboardEntry', function () {
        it(`should return successfully`, function () {
            toofzRestApi.getPlayerLeaderboardEntry('76561197960481221', 739999).then(data => {
                data.should.exist;
            });
            $httpBackend.flush();
        });
    });
});
