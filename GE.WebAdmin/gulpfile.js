var gulp = require('gulp');
var minifyCSS = require('gulp-minify-css');
var autoprefixer = require('gulp-autoprefixer');
var rename = require('gulp-rename');
var del = require('del');
var less = require('gulp-less');
var watch = require('gulp-watch');

function clear() {
    del([
        'content/css/**/*.css',
        'content/js/**/*.js',
        //'content/fonts',
        'fonts/**/*'
    ]);
}

function createFonts() {
    gulp.src([
        'bower_components/bootstrap/dist/fonts/**/'
    ])
        .pipe(gulp.dest('fonts'));

    gulp.src([
        'bower_components/font-awesome/fonts/**/*.{eot,svg,ttf,woff,woff2,otf}'
    ])
        .pipe(gulp.dest('content/fonts'));
}

function createCss() {
    gulp.src([
        'bower_components/bootstrap/dist/css/bootstrap.min.css',
        'bower_components/metisMenu/dist/metisMenu.min.css',
        'bower_components/font-awesome/css/font-awesome.min.css'
    ])
        .pipe(gulp.dest('content/css'));

    gulp.src('content/less/**/*.less')
            .pipe(less())
            .pipe(minifyCSS())
            .pipe(rename('site.min.css'))
            .pipe(autoprefixer('last 2 version', 'safari 5', 'ie 8', 'ie 9'))
            .pipe(gulp.dest('content/css'));
}

function createJs() {
    gulp.src([
        'bower_components/jquery/dist/jquery.min.js',
        'bower_components/bootstrap/dist/js/bootstrap.min.js',
        'bower_components/metisMenu/dist/metisMenu.min.js'
    ])
        .pipe(gulp.dest('content/js'));

}

gulp.task('watch', function (cb) {
    watch('content/less/**/*.less', function () {
        clear();
        createFonts();
        createCss();
        createJs();
    });
});