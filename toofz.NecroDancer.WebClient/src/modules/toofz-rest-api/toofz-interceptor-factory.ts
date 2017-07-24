import * as URI from 'urijs';
import * as util from '../../util';

import * as angular from 'angular';

export function ToofzInterceptorFactory(apiBaseUrl: string): angular.IHttpInterceptor {
    'ngInject';
    const apiUrl = new URI(apiBaseUrl);
    return {
        request: (config) => {
            const destUrl = new URI(config.url);
            if (destUrl.hostname() === apiUrl.hostname()) {
                angular.forEach(config.params, (value, key) => {
                    config.params[key] = util.toCommaSeparatedValues(value);
                });
            }

            return config;
        }
    };
}