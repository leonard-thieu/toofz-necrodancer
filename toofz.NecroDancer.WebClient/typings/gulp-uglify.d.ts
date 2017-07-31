/// <reference types="gulp-uglify" />

declare module 'gulp-uglify/composer' {
    import * as UglifyJS from 'uglify-js';
    import * as GulpUglify from 'gulp-uglify';

    interface Composer {
        (uglify: Uglify, log: Logger): typeof GulpUglify;
    }

    interface Uglify {
        minify(files: string | string[], options: object): UglifyJS.MinifyOutput;
    }

    interface Logger {
        warn: typeof console.warn;
    }

    const composer: Composer;

    export = composer;
}