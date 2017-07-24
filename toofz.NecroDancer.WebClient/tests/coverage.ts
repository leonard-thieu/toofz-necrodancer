// https://github.com/tropy/tropy/blob/master/test/support/coverage.js
import * as glob from 'glob';
import { readFileSync } from 'fs';
import {
    Reporter,
    Instrumenter,
    Collector,
    hook
} from 'istanbul';

function match() {
    const map: { [file: string]: boolean } = {};
    const fn: any = function (file: string) {
        return map[file];
    };

    fn.files = glob.sync(pattern, {
        root: root,
        realpath: true,
        ignore: ignores
    });
    for (let file of fn.files) {
        map[file] = true;
    }

    return fn;
}

function report() {
    for (let file of matched.files) {
        if (!cov[file]) {
            // Files that are not touched by code ran by the test runner is
            // manually instrumented, to illustrate the missing coverage.
            transformer(readFileSync(file, 'utf-8'), file);

            // When instrumenting the code, istanbul will give each
            // FunctionDeclaration a value of 1 in coverState.s,
            // presumably to compensate for function hoisting.
            // We need to reset this, as the function was not hoisted,
            // as it was never loaded.
            for (let key of Object.keys(instrumenter.coverState.s)) {
                instrumenter.coverState.s[key] = 0;
            }

            cov[file] = instrumenter.coverState;
        }
    }

    const collector = new Collector();
    collector.add(cov);

    const reporter = new Reporter();
    reporter.addAll(['text-summary', 'json']);
    reporter.write(collector, true, () => {});
}

const instrumenter = new Instrumenter();
const transformer = instrumenter.instrumentSync.bind(instrumenter);
const cov: any = global.__coverage__ = {};

const root = __dirname;
const pattern = (process.type === 'browser') ?
    '' :
    'src/**/*.js';
const ignores = (process.type === 'browser') ?
    [] :
    [
        'src/app.config.debug.js'
    ];

const matched = match();
hook.hookRequire(matched, transformer, {});

if (process.type === 'browser') {
    process.on('exit', report);
} else {
    window.addEventListener('unload', report);
}
