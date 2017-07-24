import * as angular from 'angular';

angular
    .module('necrodancer.app')
    .constant('apiBaseUrl', 'https://api.toofz.com')
    .config(($compileProvider: angular.ICompileProvider) => {
        'ngInject';
        $compileProvider.debugInfoEnabled(false);
        $compileProvider.commentDirectivesEnabled(false);
        $compileProvider.cssClassDirectivesEnabled(false);
    });