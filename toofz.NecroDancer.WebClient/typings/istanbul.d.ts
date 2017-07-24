import 'istanbul';

declare module 'istanbul' {
    interface Instrumenter {
        coverState: CoverState;
    }

    interface CoverState {
        path: string;
        s: any;
        b: any;
        f: any;
        fnMap: any;
        statementMap: any;
        branchMap: any;
    }

    interface Hook {
        hookRequire: (matcher: (filePath: string) => void,
                      transformer: (code: any, filePath: string) => any,
                      options?: HookRequireOptions) => void;
    }

    interface HookRequireOptions {
        verbose?: boolean;
        postLoadHook?: (filename: string) => void;
    }
}