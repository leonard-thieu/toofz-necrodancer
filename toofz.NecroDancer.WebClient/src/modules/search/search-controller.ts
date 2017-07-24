import * as angular from 'angular';

import { ToofzRestApi } from '../toofz-rest-api/toofz-rest-api';
import { SlugFilter } from '../slug/slug.module';

export class SearchController {
    constructor(private readonly $element: JQuery,
                private readonly $rootScope: angular.IRootScopeService,
                private readonly $compile: angular.ICompileService,
                private readonly $state: angular.ui.IStateService,
                private readonly toofzRestApi: ToofzRestApi,
                private readonly slugFilter: SlugFilter) {
        'ngInject';
    }

    private $input: JQuery;

    $postLink() {
        this.$input = this.$element.find('input');

        this.$input.typeahead(this.getSearchDatasetOptions());
    }

    private getSearchDatasetOptions(): Bootstrap3Typeahead.Options<toofz.Player> {
        return {
            source: (query, process) => {
                this.toofzRestApi.getPlayers(query).then(data => {
                    process(data.players);
                });
            },
            items: 'all',
            displayText: (item) => item.display_name!,
            highlighter: (text, item) => {
                interface SearchResultScope extends angular.IScope {
                    text: string;
                    item: toofz.Player;
                }

                const scope = this.$rootScope.$new(true) as SearchResultScope;
                scope.text = text;
                scope.item = item;

                return this.$compile('<nd-search-result text="::text" item="::item"></nd-search-result>')(scope);
            },
            delay: 200,
            afterSelect: (item) => {
                if (item) {
                    this.$state.go('root.player', {
                        id: item.id,
                        slug: this.slugFilter(item.display_name)
                    }, {
                        reload: true
                    });
                    this.$input.val('');
                }
            },
            fitToElement: true
        };
    }
}
