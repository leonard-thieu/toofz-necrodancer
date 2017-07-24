import * as angular from 'angular';

import { ToofzSiteApi } from './toofz-site-api';

angular
    .module('toofz.site-api', [])
    .service('toofzSiteApi', ToofzSiteApi);