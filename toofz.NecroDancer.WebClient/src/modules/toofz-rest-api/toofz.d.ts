declare namespace toofz {
    interface PagedResults {
        total: number;
    }

    interface Item {
        name: string;
        display_name: string;
        slot: string | null;
        unlock: number | null;
        cost: number | null;
    }

    interface Items {
        total: number;
        items: Item[];
    }

    interface Enemy {
        name: string;
        type: number;
        display_name: string;
        health: number;
        damage: number;
        beats_per_move: number;
        drops: number;
    }

    interface Enemies {
        total: number;
        enemies: Enemy[];
    }

    interface Entry {
        leaderboard?: Leaderboard;
        player?: Player;
        rank: number;
        score: number;
        end: End;
        killed_by: string | null;
        version: number | null;
    }

    interface End {
        zone: number;
        level: number;
    }

    interface Leaderboard {
        id: number;
        product: Products;
        mode: Modes;
        run: Runs;
        character: Characters;
        display_name: string;
        updated_at: string;
        total: number;
    }

    interface Leaderboards {
        total: number;
        leaderboards: Leaderboard[];
    }

    interface LeaderboardEntries {
        leaderboard: Leaderboard;
        total: number;
        entries: Entry[];
    }

    interface DailyLeaderboard {
        id: number;
        date: string;
        updated_at: string;
        product: Products;
        production: boolean;
    }

    interface DailyLeaderboards {
        total: number;
        leaderboards: DailyLeaderboard[];
    }

    interface DailyLeaderboardEntries {
        leaderboard: DailyLeaderboard;
        total: number;
        entries: Entry[];
    }

    interface Player {
        id: string;
        display_name: string | null;
        updated_at: string | null;
        avatar: string | null;
    }

    interface Players {
        total: number;
        players: Player[];
    }

    interface PlayerEntries {
        player: Player;
        total: number;
        entries: Entry[];
    }

    interface PaginationParams {
        offset?: number;
        limit?: number;
    }

    interface GetLeaderboardsParams {
        products?: Products[];
        modes?: Modes[];
        runs?: Runs[];
        characters?: Characters[];
    }

    interface GetDailyLeaderboardsParams extends PaginationParams {
        products?: Products[];
    }

    type Products = 'classic' | 'amplified';
    type Modes = 'standard' | 'no-return' | 'hard' | 'phasing' | 'randomizer' | 'mystery';
    type Runs = 'score' | 'speed' | 'seeded-score' | 'seeded-speed' | 'deathless';
    type Characters = 'all-characters' | 'all-characters-amplified' | 'aria' | 'bard' | 'bolt' | 'cadence' | 'coda' |
        'diamond' | 'dorian' | 'dove' | 'eli' | 'mary' | 'melody' | 'monk' | 'nocturna' | 'story-mode' | 'tempo';
}
