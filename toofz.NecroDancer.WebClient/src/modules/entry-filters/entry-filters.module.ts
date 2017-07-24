import * as moment from 'moment';
import 'moment-duration-format';

import * as angular from 'angular';

angular
    .module('necrodancer.entry-filters', [])
    .filter('time', (): TimeFilter => {
        return score => {
            return 100000000 - score;
        };
    })
    .filter('duration', (): DurationFilter => {
        return time => {
            const duration = moment.duration(time);
            if (duration.hours() < 1) {
                return duration.format('mm:ss', 2);
            }
            return duration.format('h:mm:ss', 3);
        };
    })
    .filter('wins', (): WinsFilter => {
        return score => {
            return Math.floor(score / 100);
        };
    })
    .filter('end', (): EndFilter => {
        return end => {
            const { zone, level } = end;
            if ((zone === 3 && level === 5) ||
                (zone === 4 && level === 6) ||
                (zone === 5 && level === 6)) {
                return 'Win!';
            }
            return `${zone}-${level}`;
        };
    })
    .filter('killedBy', (): KilledByFilter => {
        return killedBy => {
            switch (killedBy) {
                case 'CROWNOFTHORNS':
                    return 'CROWN OF THORNS';
                case 'HOTCOAL':
                    return 'HOT COAL';
                case 'MISSEDBEAT':
                    return 'MISSED BEAT';
                case 'SHOPKEEPER_GHOST':
                    return 'SHOPKEEPER GHOST';
                case 'SPIKETRAP':
                    return 'SPIKE TRAP';
                default:
                    return killedBy || '--';
            }
        };
    });

export interface TimeFilter {
    (score: number): number;
}

export interface DurationFilter {
    (time: number): string;
}

export interface WinsFilter {
    (score: number): number;
}

export interface EndFilter {
    (end: toofz.End): string;
}

export interface KilledByFilter {
    (killedBy: string | null): string;
}
