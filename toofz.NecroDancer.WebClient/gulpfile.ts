import * as del from 'del';
import * as browserify from 'browserify';

import merge = require('merge-stream');
import * as source from 'vinyl-source-stream';
import * as buffer from 'vinyl-buffer';

import * as gulp from 'gulp';
import * as autoprefixer from 'gulp-autoprefixer';
import * as babel from 'gulp-babel';
import * as concat from 'gulp-concat';
import * as minifyCSS from 'gulp-minify-css';
import * as sass from 'gulp-sass';
import * as sourcemaps from 'gulp-sourcemaps';
import * as tsc from 'gulp-typescript';
import * as uglify from 'gulp-uglify';

const tsProject = tsc.createProject('tsconfig.json');

gulp.task('default', ['build']);

gulp.task('build', ['build:js', 'build:css']);

gulp.task('build:js', ['build:js:test'], () => {
    const opts: browserify.Options = {
        entries: 'src/app.module.ts',
        debug: true
    };
    const bundle = browserify(opts)
        .ignore('angular-ui-router')
        .ignore('moment-duration-format')
        .plugin('tsify')
        .bundle()
        .pipe(source('site.js'))
        .pipe(buffer())
        .pipe(sourcemaps.init({
            loadMaps: true
        }))
        .pipe(babel({
            plugins: ['angularjs-annotate']
        }));

    const site = bundle
        .pipe(concat('site.js'))
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest('.'));

    const siteMin = bundle
        .pipe(concat('site.min.js'))
        .pipe(uglify())
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest('.'));

    return merge(site, siteMin);
});

// This is still needed for tests.
gulp.task('build:js:test', () => {
    return tsProject.src()
        .pipe(sourcemaps.init())
        .pipe(tsProject()).js
        .pipe(sourcemaps.write())
        .pipe(gulp.dest('.'));
});

gulp.task('build:css', () => {
    const sassResult = gulp.src('css/site.scss')
        .pipe(sourcemaps.init())
        .pipe(sass().on('error', sass.logError))
        .pipe(autoprefixer({
            browsers: ['last 2 versions']
        }));

    const site = sassResult
        .pipe(concat('site.css'))
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest('css'));

    const siteMin = sassResult
        .pipe(minifyCSS())
        .pipe(concat('site.min.css'))
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest('css'));

    return merge(site, siteMin);
});

gulp.task('clean', ['clean:js', 'clean:css']);

gulp.task('clean:js', ['clean:js:test'], () => {
    return del([
        'src/**/*.js',
        'src/**/*.js.map',
        '*.js',
        '*.js.map'
    ]);
});

gulp.task('clean:js:test', () => {
    return del([
        'tests/**/*.js',
        'tests/**/*.js.map',
        'coverage/'
    ]);
});

gulp.task('clean:css', () => {
    return del([
        'css/**/*.css',
        'css/**/*.css.map'
    ]);
});
