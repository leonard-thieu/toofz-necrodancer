import * as angular from 'angular';
import 'angular-mocks';

import '../../../../src/modules/pagination/pagination.module';
import { PaginationController } from '../../../../src/modules/pagination/pagination-controller';
const _PaginationController = PaginationController as any;

describe('PaginationController', function () {
    let $componentController: angular.IComponentControllerService;

    beforeEach(function () {
        angular.mock.module('necrodancer.pagination');

        inject((_$componentController_: any) => {
            $componentController = _$componentController_;
        });
    });

    describe('getRecords', function () {
        it(`should return a 'Records' object`, function () {
            const records = _PaginationController.getRecords(0, 20, 35);

            records.should.deep.equal({
                start: 1,
                end: 20,
                total: 35
            });
        });

        it(`should return a 'Records' object with 'end' limited by 'total'`, function () {
            const records = _PaginationController.getRecords(20, 20, 35);

            records.should.deep.equal({
                start: 21,
                end: 35,
                total: 35
            });
        });
    });

    describe('getPages', function () {
        it(`should return 'Pages' object`, function () {
            const pages = _PaginationController.getPages(0, 20, 40);

            pages.should.deep.equal({
                isStartVisible: false,
                isEndVisible: false,
                start: 1,
                middle: [1, 2],
                end: 2,
                current: 1
            });
        });
    });

    describe('getMiddlePages', function () {
        it(`should return a middle pages object`, function () {
            const middle = _PaginationController.getMiddlePages(1, 108, 57);

            middle.should.deep.equal({
                midStart: 56,
                midEnd: 58,
                mid: [56, 57, 58]
            });
        });
    });

    describe('getIsStartVisible', function () {

    });

    describe('getIsEndVisible', function () {

    });

    describe('$onInit', function () {
        it(`should initialize ndPagination`, function () {
            const ctrl = $componentController('ndPagination', {}, {
                data: {
                    offset: 0,
                    limit: 20,
                    total: 40
                }
            }) as PaginationController;

            ctrl.$onInit();

            ctrl.records.should.be.an('object');
            ctrl.pages.should.be.an('object');
        });
    });
});