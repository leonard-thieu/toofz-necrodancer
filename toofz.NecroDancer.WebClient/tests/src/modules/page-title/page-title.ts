import * as angular from 'angular';
import 'angular-mocks';

import '../../../../src/modules/page-title/page-title.module';
import { PageTitle } from '../../../../src/modules/page-title/page-title';

describe('PageTitle', function () {
    let pageTitle: PageTitle;
    let $rootScope: angular.IRootScopeService;

    beforeEach(function () {
        angular.mock.module('necrodancer.page-title');

        inject((_pageTitle_: any,
                _$rootScope_: any) => {
            pageTitle = _pageTitle_;
            $rootScope = _$rootScope_;
        });
    });

    describe('set', function () {
        it(`should set '$rootScope.title' to 'title' and return 'title' if 'titlecase' is false`, function () {
            const title = pageTitle.set('my title', false);

            $rootScope.title!.should.equal('my title');
            title.should.equal('my title');
        });

        it(`should titlecase 'title', set '$rootScope.title' to 'title', and return 'title' if 'titlecase' is true`, function () {
            const title = pageTitle.set('my title', true);

            $rootScope.title!.should.equal('My Title');
            title.should.equal('My Title');
        });
    });

    describe('unset', function () {
        it(`should set '$rootScope.title' to undefined`, function () {
            $rootScope.title = 'my title';

            pageTitle.unset();

            should.not.exist($rootScope.title);
        });
    });
});
