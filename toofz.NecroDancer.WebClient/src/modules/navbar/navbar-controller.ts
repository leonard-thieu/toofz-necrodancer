export class NavbarController {
    constructor(public readonly apiBaseUrl: string) { 
        'ngInject';
    }

    readonly areas: toofzSite.Category[];
}