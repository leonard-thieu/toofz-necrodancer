import * as angular from 'angular';
import 'angular-mocks';

import '../../../../src/modules/titlecase/titlecase.module';
import { TitlecaseFilter } from '../../../../src/modules/titlecase/titlecase.module';

describe('TitlecaseFilter', function () {
    let titlecaseFilter: TitlecaseFilter;

    beforeEach(function () {
        angular.mock.module('necrodancer.titlecase');

        inject((_titlecaseFilter_: any) => {
            titlecaseFilter = _titlecaseFilter_;
        });
    });

    it(`should return 'text' if 'text' is not a string`, function () {
        const text = {} as any;

        const result = titlecaseFilter(text);

        result.should.equal(text);
    });

    it(`should return 'text' as titlecase if 'text' is a string`, function () {
        const text = 'diamond hard mode seeded score (amplified) leaderboard';

        const result = titlecaseFilter(text);

        result.should.equal('Diamond Hard Mode Seeded Score (Amplified) Leaderboard');
    });
});