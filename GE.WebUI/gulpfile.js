var gulp = require('gulp');
var minifyCss = require('gulp-minify-css');
var autoprefixer = require('gulp-autoprefixer');
var rename = require('gulp-rename');
var del = require('del');
var less = require('gulp-less');
var watch = require('gulp-watch');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var merge = require('merge-stream');

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
    var lessStream = gulp.src([
        'content/less/**/*.less'
    ])
        .pipe(less())
        .pipe(concat('less-files.less'));

    var cssStream = gulp.src([
        'bower_components/bootstrap/dist/css/bootstrap.min.css',
        'bower_components/font-awesome/css/font-awesome.min.css'
    ])
        .pipe(concat('css-files.css'));

    var mergedStream = merge(lessStream, cssStream)
        .pipe(concat('site.min.css'))
        .pipe(autoprefixer('last 2 version', 'safari 5', 'ie 8', 'ie 9'))
        .pipe(minifyCss())
        .pipe(gulp.dest('content/dist/css'));
}

function createJs() {
    gulp.src([
        'bower_components/jquery/dist/jquery.min.js',
        'bower_components/jquery-ajax-unobtrusive/jquery.unobtrusive-ajax.min.js',
        'bower_components/bootstrap/dist/js/bootstrap.min.js',
        'scripts/ge-game-menu.js',
        'bower_components/jquery-hoverIntent/jquery.hoverIntent.js'
    ])
        .pipe(concat('site.min.js'))
        .pipe(uglify())
        .pipe(gulp.dest('content/dist/js'));

    gulp.src([
        'scripts/ge-for-gamers-block.js',
        'scripts/ge-last-news-block.js'
    ])
        .pipe(uglify())
        .pipe(rename({
            suffix: '.min'
        }))
        .pipe(gulp.dest('content/dist/js'));

}

gulp.task('watch', function (cb) {
    watch([
        'content/less/**/*.less',
        'scripts/**/*.js'
    ], function () {
        clear();
        createFonts();
        createCss();
        createJs();
    });
});