import * as angular from 'angular';
import 'angular-mocks';

import '../../../../src/modules/entry-filters/entry-filters.module';
import {
    DurationFilter,
    EndFilter,
    KilledByFilter,
    TimeFilter,
    WinsFilter
} from '../../../../src/modules/entry-filters/entry-filters.module';

describe('necrodancer.entry-filters', function () {
    beforeEach(function () {
        angular.mock.module('necrodancer.entry-filters');
    });

    describe('time', function () {
        let timeFilter: TimeFilter;

        beforeEach(function () {
            inject((_timeFilter_: any) => {
                timeFilter = _timeFilter_;
            });
        });

        it(`should return 'score' as a length of time in milliseconds`, function () {
            const result = timeFilter(93456723);

            result.should.equal(6543277);
        });
    });

    describe('duration', function () {
        let durationFilter: DurationFilter;

        beforeEach(function () {
            inject((_durationFilter_: any) => {
                durationFilter = _durationFilter_;
            });
        });

        it(`should return 'mm:ss' if 'time' is less than 1 hour`, function () {
            const result = durationFilter(543277);

            result.should.equal('09:03.27');
        });

        it(`should return 'h:mm:ss' if 'time' is at least 1 hour`, function () {
            const result = durationFilter(6543277);

            result.should.equal('1:49:03.277');
        });
    });

    describe('wins', function () {
        let winsFilter: WinsFilter;

        beforeEach(function () {
            inject((_winsFilter_: any) => {
                winsFilter = _winsFilter_;
            });
        });

        it(`should return 'score' as the number of wins`, function () {
            const result = winsFilter(350);

            result.should.equal(3);
        });
    });

    describe('end', function () {
        let endFilter: EndFilter;

        beforeEach(function () {
            inject((_endFilter_: any) => {
                endFilter = _endFilter_;
            });
        });

        it(`should return 'Win!' if 'end.zone' is 3 and 'end.level' is 5`, function () {
            const result = endFilter({ zone: 3, level: 5 });

            result.should.equal('Win!');
        });

        it(`should return 'Win!' if 'end.zone' is 4 and 'end.level' is 6`, function () {
            const result = endFilter({ zone: 4, level: 6 });

            result.should.equal('Win!');
        });

        it(`should return 'Win!' if 'end.zone' is 5 and 'end.level' is 6`, function () {
            const result = endFilter({ zone: 5, level: 6 });

            result.should.equal('Win!');
        });

        it(`should return '{zone}-{level}'`, function () {
            const result = endFilter({ zone: 2, level: 3 });

            result.should.equal('2-3');
        });
    });

    describe('killedBy', function () {
        let killedByFilter: KilledByFilter;

        beforeEach(function () {
            inject((_killedByFilter_: any) => {
                killedByFilter = _killedByFilter_;
            });
        });

        it(`should return 'CROWN OF THORNS' if 'killedBy' is 'CROWNOFTHORNS'`, function () {
            const result = killedByFilter('CROWNOFTHORNS');

            result.should.equal('CROWN OF THORNS');
        });

        it(`should return 'HOT COAL' if 'killedBy' is 'HOTCOAL'`, function () {
            const result = killedByFilter('HOTCOAL');

            result.should.equal('HOT COAL');
        });

        it(`should return 'MISSED BEAT' if 'killedBy' is 'MISSEDBEAT'`, function () {
            const result = killedByFilter('MISSEDBEAT');

            result.should.equal('MISSED BEAT');
        });

        it(`should return 'SHOPKEEPER GHOST' if 'killedBy' is 'SHOPKEEPER_GHOST'`, function () {
            const result = killedByFilter('SHOPKEEPER_GHOST');

            result.should.equal('SHOPKEEPER GHOST');
        });

        it(`should return 'SPIKE TRAP' if 'killedBy' is 'SPIKETRAP'`, function () {
            const result = killedByFilter('SPIKETRAP');

            result.should.equal('SPIKE TRAP');
        });

        it(`should return 'killedBy' if 'killedBy' exists`, function () {
            const result = killedByFilter('SLIME');

            result.should.equal('SLIME');
        });

        it(`should return '--' if 'killedBy' doesn't exist`, function () {
            const result = killedByFilter(null);

            result.should.equal('--');
        });
    });
});
