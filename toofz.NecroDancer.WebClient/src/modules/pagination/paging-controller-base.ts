import * as util from '../../util';

import * as angular from 'angular';

export abstract class PagingControllerBase<T extends toofz.PagedResults> {
    constructor(protected $stateParams: angular.ui.IStateParamsService) {
        'ngInject';
    }

    protected abstract limit: number;

    readonly data: T;

    records: {
        offset: number | undefined;
        limit: number;
        total: number;
    };

    $onInit() {
        const { page } = this.$stateParams;

        this.records = {
            offset: util.pageToOffset(page, this.limit),
            limit: this.limit,
            total: this.data.total
        };
    }

    $postLink() {
        if (typeof this.records.offset !== 'number') {
            this.records.offset = 0;
        }
    }
}