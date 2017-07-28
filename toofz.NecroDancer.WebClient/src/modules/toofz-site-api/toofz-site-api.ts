import { IHttpService } from 'angular';

export class ToofzSiteApi {
    constructor(private readonly $http: IHttpService) {
        'ngInject';
    }

    getAreas() {
        return this.$http.get<toofzSite.Areas>('/data/areas.min.json?v=4', {
            cache: true
        }).then(response => response.data!.areas);
    }

    getLeaderboardCategories() {
        return this.$http.get<toofzSite.Leaderboard.CategoriesResponse>('/data/leaderboard-categories.min.json?v=3', {
            cache: true
        }).then(response => response.data!.categories);
    }

    getLeaderboardHeaders() {
        return this.$http.get<toofzSite.Leaderboard.Headers>('/data/leaderboard-headers.min.json?v=3', {
            cache: true
        }).then(response => response.data!.leaderboards);
    }
}
