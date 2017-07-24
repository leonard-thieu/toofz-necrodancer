declare namespace toofzSite {
    interface Areas {
        areas: Category[];
    }

    interface Category {
        name: string;
        path?: string;
        icon?: string;
        categories?: Category[];
    }

    namespace Leaderboard {
        interface CategoriesResponse {
            categories: Categories;
        }

        interface Categories {
            [name: string]: CategoryItems;
        }

        interface CategoryItems {
            [name: string]: CategoryItem;
        }

        interface CategoryItem {
            display_name: string;
            order: number;
            value?: boolean;
            icon?: string;
        }

        interface Headers {
            leaderboards: Header[];
        }

        interface Header {
            id: number;
            display_name: string;
            product: string;
            mode: string;
            run: string;
            character: string;
        }
    }
}