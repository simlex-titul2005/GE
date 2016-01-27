var gulp = require('gulp');
var minifyCSS = require('gulp-minify-css');
var autoprefixer = require('gulp-autoprefixer');
var rename = require('gulp-rename');
var del = require('del');
var less = require('gulp-less');
var watch = require('gulp-watch');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');

function clear() {
    del([
        'content/dist/css/**/*.css',
        'content/dist/js/**/*.js',
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
        .pipe(gulp.dest('content/dist/fonts'));
}

function createCss() {
    gulp.src([
        'bower_components/bootstrap/dist/css/bootstrap.min.css',
        'bower_components/metisMenu/dist/metisMenu.min.css',
        'bower_components/font-awesome/css/font-awesome.min.css'
    ])
        .pipe(gulp.dest('content/dist/css'));

    gulp.src('content/less/**/*.less')
            .pipe(less())
            .pipe(minifyCSS())
            .pipe(rename('site.min.css'))
            .pipe(autoprefixer('last 2 version', 'safari 5', 'ie 8', 'ie 9'))
            .pipe(gulp.dest('content/dist/css'));

    gulp.src('content/sx/less/**/*.less')
        .pipe(less())
        .pipe(minifyCSS())
        .pipe(autoprefixer('last 2 version', 'safari 5', 'ie 8', 'ie 9'))
        .pipe(rename('sx.min.css'))
        .pipe(gulp.dest('content/dist/css'));
}

function createJs() {
    gulp.src([
        'bower_components/jquery/dist/jquery.min.js',
        'bower_components/jquery-ajax-unobtrusive/jquery.unobtrusive-ajax.min.js',
        'bower_components/bootstrap/dist/js/bootstrap.min.js',
        'bower_components/metisMenu/dist/metisMenu.min.js'
    ])
        .pipe(gulp.dest('content/dist/js'));

    gulp.src('content/sx/js/**/*.js')
        .pipe(concat('sx.min.js'))
        .pipe(uglify())
        .pipe(gulp.dest('content/dist/js'));

}

gulp.task('watch', function (cb) {
    watch(['content/less/**/*.less', 'content/sx/less/**/*.less', 'content/sx/js/**/*.js'], function () {
        clear();
        createFonts();
        createCss();
        createJs();
    });
});