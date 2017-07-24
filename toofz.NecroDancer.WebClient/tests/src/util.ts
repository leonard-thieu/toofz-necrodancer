import * as util from '../../src/util';

describe('util', function () {
    describe('pageToOffset', function () {
        it(`should return undefined if 'page' is not a number`, function () {
            const offset = util.pageToOffset(undefined, 20);

            should.not.exist(offset);
        });

        it(`should return an offset if 'page' is a number`, function () {
            const offset = util.pageToOffset(3, 20);

            offset!.should.equal(40);
        });
    });

    describe('roundDownToMultiple', function () {
        it(`should return 'value' rounded down to its closest multiple`, function () {
            const rounded = util.roundDownToMultiple(43, 20);

            rounded.should.equal(40);
        });
    });
});