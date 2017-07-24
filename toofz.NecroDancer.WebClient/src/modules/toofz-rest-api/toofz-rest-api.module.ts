import * as angular from 'angular';

import { ToofzInterceptorFactory } from './toofz-interceptor-factory';
import { ToofzRestApi } from './toofz-rest-api';

angular
    .module('toofz.rest-api', [])
    .config(($httpProvider: angular.IHttpProvider) => {
        'ngInject';
        $httpProvider.interceptors.push(ToofzInterceptorFactory);
    })
    .service('toofzRestApi', ToofzRestApi);