import * as angular from 'angular';
import '../titlecase/titlecase.module';

import { PageTitle } from './page-title';

angular
    .module('necrodancer.page-title', [
        'necrodancer.titlecase'
    ])
    .service('pageTitle', PageTitle);