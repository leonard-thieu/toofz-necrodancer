import * as sinon from 'sinon';

import * as angular from 'angular';
import 'angular-mocks';

import '../../../../src/modules/toofz-rest-api/toofz-rest-api.module';

describe('ToofzInterceptorFactory', function () {
    let toofzInterceptorFactory: angular.IHttpInterceptorFactory;
    let toofzInterceptor: angular.IHttpInterceptor;

    beforeEach(function () {
        angular.mock.module('toofz.rest-api', {
            apiBaseUrl: 'https://api.toofz.com'
        }, ($httpProvider: angular.IHttpProvider) => {
            toofzInterceptorFactory = $httpProvider.interceptors[0] as angular.IHttpInterceptorFactory;
        });

        inject(($injector: angular.auto.IInjectorService) => {
            toofzInterceptor = $injector.invoke(toofzInterceptorFactory);
        });
    });

    describe('request', function () {
        let angular_forEach: sinon.SinonSpy;

        beforeEach(function () {
            angular_forEach = sinon.spy(angular, 'forEach');
        });

        afterEach(function () {
            angular_forEach.restore();
        });

        it(`should not modify the request if 'config.url' has a hostname that's not equal to the hostname of 'apiBaseUrl'`, function () {
            const request = toofzInterceptor.request!;
            const config = {
                url: 'http://necrodancer.com/'
            } as any;

            request(config);

            angular_forEach.should.not.have.been.called;
        });

        it(`should convert arrays to comma separated values if 'config.url' has a hostname that's equal to the hostname of 'apiBaseUrl'`, function () {
            const request = toofzInterceptor.request!;
            const config = {
                url: 'https://api.toofz.com',
                params: {
                    products: ['amplified', 'classic']
                }
            } as any;

            request(config);

            config.params.products.should.equal('amplified,classic');
        });
    });
});