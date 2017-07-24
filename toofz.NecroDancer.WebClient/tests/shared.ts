export interface BackendDefinition {
    description?: string;
    request: {
        method: string;
        url: string;
        data?: any;
        headers?: any;
        keys?: any[];
    };
    response: {
        data: string | any;
        headers?: any;
        text?: string;
    };
}