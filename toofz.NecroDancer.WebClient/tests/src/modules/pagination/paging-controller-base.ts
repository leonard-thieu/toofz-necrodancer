import * as angular from 'angular';
import 'angular-mocks';

import '../../../../src/modules/pagination/pagination.module';
import { PagingControllerBase } from '../../../../src/modules/pagination/paging-controller-base';

describe('PagingControllerBase', function () {
    let ctrl: PagingControllerBase<any>;
    let _ctrl: any;

    beforeEach(function () {
        angular.mock.module('necrodancer.pagination');

        angular.module('necrodancer.pagination')
            .service('pagingControllerBase', PagingControllerBase);

        inject((_pagingControllerBase_: any) => {
            ctrl = _pagingControllerBase_;
            _ctrl = ctrl;
        });
    });

    describe('$onInit', function () {
        it(`should initialize controller`, function () {
            _ctrl.data = {};

            ctrl.$onInit();

            ctrl.records.should.be.an('object');
        });
    });

    describe('$postLink', function () {
        it(`should set 'records.offset' to 0 if 'records.offset' is not a number`, function () {
            _ctrl.records = {};

            ctrl.$postLink();

            _ctrl.records.offset.should.equal(0);
        });

        it(`should do nothing if 'records.offset' is a number`, function () {
            _ctrl.records = {
                offset: 20
            };

            ctrl.$postLink();

            _ctrl.records.offset.should.equal(20);
        });
    });
});