export function pageToOffset(page: number | undefined, limit: number): number | undefined {
    if (typeof page !== 'number') {
        return undefined;
    }
    return (page - 1) * limit;
}

export function roundDownToMultiple(value: number, multiple: number): number {
    return Math.floor(value / multiple) * multiple;
}

export function toCommaSeparatedValues(value: any) {
    return Array.isArray(value) ?
        value.join(',') :
        value;
}