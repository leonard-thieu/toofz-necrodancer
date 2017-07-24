import * as sinon from 'sinon';

import * as angular from 'angular';
import 'angular-mocks';

import '../../../../src/modules/search/search.module';
import { SearchController } from '../../../../src/modules/search/search-controller';
const toofz_definitions = require('../toofz-rest-api/toofz-rest-api.definitions.json').definitions;

describe('SearchController', function () {
    let $componentController: angular.IComponentControllerService;
    let $httpBackend: angular.IHttpBackendService;

    beforeEach(function () {
        angular.mock.module('necrodancer.search', {
            apiBaseUrl: ''
        });

        inject((_$componentController_: any) => {
            $componentController = _$componentController_;
        });

        inject((_$httpBackend_: any) => {
            $httpBackend = _$httpBackend_;

            for (const definition of toofz_definitions) {
                const { request, response } = definition;
                $httpBackend.when(request.method, request.url)
                    .respond(response.data);
            }
        });
    });

    describe('$postLink', function () {
        it(`should initialize typeahead`, function () {
            const $element = $('<nd-search><input></nd-search>');
            const ctrl = $componentController('ndSearch', {
                $element: $element
            }) as SearchController;

            ctrl.$postLink();
        });
    });

    describe('getSearchDatasetOptions', function () {
        let ctrl: SearchController;
        let _ctrl: any;

        beforeEach(function () {
            const $element = $('<nd-search><input></nd-search>');
            ctrl = $componentController('ndSearch', {
                $element: $element
            }) as SearchController;
            _ctrl = ctrl;
        });

        it(`should return an options object`, function () {
            const options = _ctrl.getSearchDatasetOptions();

            options.should.be.an('object');
        });

        describe('source', function () {
            it(`should update typeahead with player data`, function () {
                const process = sinon.stub();
                const { source } = _ctrl.getSearchDatasetOptions();

                source('Mendayen', process);
                $httpBackend.flush();

                process.should.have.been.called;
            });
        });

        describe('displayText', function () {
            it(`should return 'item.display_name'`, function () {
                const { displayText } = _ctrl.getSearchDatasetOptions();
                const item = { display_name: 'Mendayen' };

                const text = displayText(item);

                text.should.equal('Mendayen');
            });
        });

        describe('highlighter', function () {
            it(`should return a template`, function () {
                const { highlighter } = _ctrl.getSearchDatasetOptions();
                const text = 'Mendayen';
                const item = { avatar: 'https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/be/be8232c6572aa99106a5646149f422b0df6b3112.jpg' };

                const $template = highlighter(text, item);

                const scope = $template.scope();
                scope.text.should.equal('Mendayen');
                scope.item.avatar.should.equal('https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/be/be8232c6572aa99106a5646149f422b0df6b3112.jpg');
            });
        });

        describe('afterSelect', function () {
            let $state: angular.ui.IStateService;
            let $state_go: sinon.SinonSpy;

            beforeEach(function () {
                inject((_$state_: any) => {
                    $state = _$state_;
                    $state_go = sinon.stub($state, 'go');
                });
            });

            it(`should do nothing if 'item' does not exist`, function () {
                const { afterSelect } = _ctrl.getSearchDatasetOptions();

                afterSelect(null);

                $state_go.should.not.have.been.called;
            });

            it(`should go to the 'root.player' state and clear the input if 'item' exists`, function () {
                ctrl.$postLink();

                const { afterSelect } = _ctrl.getSearchDatasetOptions();
                const item = {
                    id: '76561197960481221',
                    slug: 'Mendayen'
                };

                afterSelect(item);

                $state_go.should.have.been.called;
            });
        });
    });
});