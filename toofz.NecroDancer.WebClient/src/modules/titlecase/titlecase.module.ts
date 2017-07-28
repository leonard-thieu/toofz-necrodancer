import * as angular from 'angular';

angular
    .module('necrodancer.titlecase', [])
    .filter('titlecase', (): TitlecaseFilter => {
        /* 
         * To Title Case 2.1 – http://individed.com/code/to-title-case/
         * Copyright © 2008–2013 David Gouch. Licensed under the MIT License.
         */

        /* istanbul ignore next */
        function toTitleCase(text: string) {
            const smallWords = /^(a|an|and|as|at|but|by|en|for|if|in|nor|of|on|or|per|the|to|vs?\.?|via)$/i;

            return text.replace(/[A-Za-z0-9\u00C0-\u00FF]+[^\s-]*/g, function (match, index, title) {
                if ((index > 0) &&
                    (index + match.length !== title.length) &&
                    (match.search(smallWords) > -1) &&
                    (title.charAt(index - 2) !== ':') &&
                    (title.charAt(index + match.length) !== '-' || title.charAt(index - 1) === '-') &&
                    (title.charAt(index - 1).search(/[^\s-]/) < 0)) {
                    return match.toLowerCase();
                }

                if (match.substr(1).search(/[A-Z]|\../) > -1) {
                    return match;
                }

                return match.charAt(0).toUpperCase() + match.substr(1);
            });
        }

        return text => {
            if (typeof text === 'string') {
                return toTitleCase(text.toLowerCase()).replace(/-/g, ' ');
            }
            return text;
        };
    });

export interface TitlecaseFilter {
    (text: string): string;
}
