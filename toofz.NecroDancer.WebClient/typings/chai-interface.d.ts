/// <reference types="chai" />
/// <reference types="chai-as-promised" />

declare namespace Chai {
    interface Assertion {
        'interface': Interface;
    }

    interface Interface {
        (definition: any): void;
    }

    interface PromisedAssertion {
        'interface': Interface;
    }
}

declare function chaiInterface(chai: any, utils: any): void;

declare namespace chaiInterface { }

declare module 'chai-interface' {
    export = chaiInterface;
}