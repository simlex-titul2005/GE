var gulp = require('gulp');
var minifyCSS = require('gulp-minify-css');
var autoprefixer = require('gulp-autoprefixer');
var rename = require('gulp-rename');
var del = require('del');
var less = require('gulp-less');
var watch = require('gulp-watch');

gulp.task('watch', function (cb) {
    watch('content/less/**/*.less', function () {
        del(['content/css/**/*.css', 'fonts/**/*']);

        gulp.src([
        'bower_components/bootstrap/dist/fonts/**/',
        'bower_components/opensans-condensed-notosans/regular/**/*.{eot,ttf,woff}'
        ])
            .pipe(gulp.dest('fonts'))

        gulp.src('content/less/**/*.less')
            .pipe(less())
            .pipe(minifyCSS())
            .pipe(rename('site.min.css'))
            .pipe(autoprefixer('last 2 version', 'safari 5', 'ie 8', 'ie 9'))
            .pipe(gulp.dest('content/css'));
    });
});