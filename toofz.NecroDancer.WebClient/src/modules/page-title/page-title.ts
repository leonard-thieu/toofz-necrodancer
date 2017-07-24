import * as angular from 'angular';

import { TitlecaseFilter } from '../titlecase/titlecase.module';

export class PageTitle {
    constructor(private readonly $rootScope: angular.IRootScopeService,
                private readonly titlecaseFilter: TitlecaseFilter) {
        'ngInject';
    }

    set(title: string, titlecase: boolean = true): string {
        if (titlecase) {
            title = this.titlecaseFilter(title);
        }
        this.$rootScope.title = title;
        return title;
    }

    unset(): void {
        this.$rootScope.title = undefined;
    }
}

declare module 'angular' {
    interface IRootScopeService {
        title?: string;
    }
}
