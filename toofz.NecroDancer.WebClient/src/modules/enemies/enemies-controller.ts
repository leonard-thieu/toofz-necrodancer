import * as angular from 'angular';

import { PagingControllerBase } from '../pagination/paging-controller-base';
import { PageTitle } from '../page-title/page-title';

export class EnemiesController extends PagingControllerBase<toofz.Enemies> {
    constructor($stateParams: angular.ui.IStateParamsService,
                private readonly pageTitle: PageTitle) {
        'ngInject';
        super($stateParams);

        const { attribute } = $stateParams;

        if (attribute) {
            this.title = `${attribute} ${this.title}`;
        }

        this.title = this.pageTitle.set(this.title);
    }

    limit = 10;
    title = 'Enemies';

    $onDestroy() {
        this.pageTitle.unset();
    }
}