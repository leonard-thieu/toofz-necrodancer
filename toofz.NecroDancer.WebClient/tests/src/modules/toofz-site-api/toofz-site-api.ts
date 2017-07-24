import * as angular from 'angular';
import 'angular-mocks';

import '../../../../src/modules/toofz-site-api/toofz-site-api.module';
import { ToofzSiteApi } from '../../../../src/modules/toofz-site-api/toofz-site-api';

const toofzSite_definitions = require('./toofz-site-api-definitions.json').definitions;

describe('ToofzSiteApi', function () {
    let $httpBackend: angular.IHttpBackendService;
    let toofzSiteApi: ToofzSiteApi;

    beforeEach(function () {
        angular.mock.module('toofz.site-api');

        inject((_$httpBackend_: any) => {
            $httpBackend = _$httpBackend_;

            for (const definition of toofzSite_definitions) {
                const { request, response } = definition;
                $httpBackend.when(request.method, request.url)
                    .respond(response.data);
            }
        });

        inject((_toofzSiteApi_: any) => {
            toofzSiteApi = _toofzSiteApi_;
        });
    });

    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    describe('getAreas', function () {
        it(`should return successfully`, function () {
            toofzSiteApi.getAreas().then(areas => {
                areas.should.exist;
            });
            $httpBackend.flush();
        });
    });

    describe('getLeaderboardCategories', function () {
        it(`should return successfully`, function () {
            toofzSiteApi.getLeaderboardCategories().then(categories => {
                categories.should.exist;
            });
            $httpBackend.flush();
        });
    });

    describe('getLeaderboardHeaders', function () {
        it(`should return successfully`, function () {
            toofzSiteApi.getLeaderboardHeaders().then(leaderboards => {
                leaderboards.should.exist;
            });
            $httpBackend.flush();
        });
    });
});
