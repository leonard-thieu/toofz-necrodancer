import * as angular from 'angular';

angular
    .module('necrodancer.slug', [])
    .filter('slug', (): SlugFilter => {
        return text => {
            if (typeof text === 'string') {
                return text.replace(/[\s#%&:/?+^{}\[\]"\\`|*<>]/g, '-');
            }
            return '';
        };
    });

export interface SlugFilter {
    (text: string | null): string;
}
