var gulp = require('gulp');
var concat = require('gulp-concat');
var minifyCSS = require('gulp-minify-css');
var autoprefixer = require('gulp-autoprefixer');
var rename = require('gulp-rename');
var del = require('del');

gulp.task('css:clean', function () {
    return del(['content/css/**/*.css', 'fonts/**/*']);
});

gulp.task('css', ['css:clean'], function () {
    gulp.src([
        'bower_components/bootstrap/dist/fonts/**/',
        'bower_components/opensans-condensed-notosans/regular/**/*.{eot,ttf,woff}'
    ])
    .pipe(gulp.dest('fonts'))

    gulp.src([
        'content/less/**/*.css'
    ])
        .pipe(minifyCSS())
        .pipe(rename('site.min.css'))
        .pipe(autoprefixer('last 2 version', 'safari 5', 'ie 8', 'ie 9'))
        .pipe(gulp.dest('content/css'))

    gulp.watch('content/css', ['css']);
});