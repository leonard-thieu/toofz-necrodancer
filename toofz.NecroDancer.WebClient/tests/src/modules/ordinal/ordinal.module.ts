import * as angular from 'angular';
import 'angular-mocks';

import '../../../../src/modules/ordinal/ordinal.module';
import { OrdinalFilter } from '../../../../src/modules/ordinal/ordinal.module';

describe('necrodancer.ordinal', function () {
    beforeEach(function () {
        angular.mock.module('necrodancer.ordinal');
    });

    describe('ordinal', function () {
        let ordinalFilter: OrdinalFilter;

        beforeEach(function () {
            inject((_ordinalFilter_: any) => {
                ordinalFilter = _ordinalFilter_;
            });
        });

        it(`should return '11th' if 'i' ends with 11`, function () {
            ordinalFilter(11).should.equal('11th');
        });

        it(`should return '12th' if 'i' ends with 12`, function () {
            ordinalFilter(12).should.equal('12th');
        });

        it(`should return '13th' if 'i' ends with 13`, function () {
            ordinalFilter(13).should.equal('13th');
        });

        it(`should return '{i}st' if 'i' ends with 1`, function () {
            ordinalFilter(1).should.equal('1st');
        });

        it(`should return '{i}nd' if 'i' ends with 2`, function () {
            ordinalFilter(2).should.equal('2nd');
        });

        it(`should return '{i}rd' if 'i' ends with 3`, function () {
            ordinalFilter(3).should.equal('3rd');
        });

        it(`should return '{i}th' otherwise`, function () {
            ordinalFilter(75).should.equal('75th');
        });
    });
});