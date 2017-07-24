import * as angular from 'angular';

import { PagingControllerBase } from '../pagination/paging-controller-base';
import { PageTitle } from '../page-title/page-title';

export class ItemsController extends PagingControllerBase<toofz.Items> {
    constructor($stateParams: angular.ui.IStateParamsService,
                private readonly pageTitle: PageTitle) {
        'ngInject';
        super($stateParams);

        const { category, subcategory } = $stateParams;

        if (category) {
            if (subcategory) {
                this.title = `${category} (${subcategory})`;
            } else {
                this.title = category;
            }
        }

        this.title = this.pageTitle.set(this.title);
    }

    limit = 10;
    title = 'Items';

    $onDestroy() {
        this.pageTitle.unset();
    }
}