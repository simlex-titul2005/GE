var promise = require('es6-promise'),
    gulp = require('gulp'),
    minifyCss = require('gulp-minify-css'),
    autoprefixer = require('gulp-autoprefixer'),
    rename = require('gulp-rename'),
    del = require('del'),
    less = require('gulp-less'),
    watch = require('gulp-watch'),
    concat = require('gulp-concat'),
    uglify = require('gulp-uglify'),
    merge = require('merge-stream');

function clear() {
    del([
        'content/dist/css/**/*.css',
        'content/dist/js/**/*.js',
        'content/fonts',
        'fonts/**/*'
    ]);
}

function createFonts() {
    gulp.src([
        'bower_components/bootstrap/fonts/**/'
    ])
        .pipe(gulp.dest('fonts'));

    gulp.src([
        'bower_components/font-awesome/fonts/**/*.{eot,svg,ttf,woff,woff2,otf}'
    ])
        .pipe(gulp.dest('content/dist/fonts'));
}

function createCss() {
    var lessStream = gulp.src([
        'content/less/article.less',
        'content/less/bootstrap-ext.less',
        'content/less/comments.less',
        'content/less/footer.less',
        'content/less/for-gamers-block.less',
        'content/less/forum.less',
        'content/less/game-list.less',
        'content/less/last-news-block.less',
        'content/less/last-category-news-block.less',
        'content/less/list-article.less',
        'content/less/list-news.less',
        'content/less/material.less',
        'content/less/positioned.less',
        'content/less/seach-block.less',
        'content/less/site.less',
        'content/less/sx-list.less',
        'content/less/sx-pager.less',
        'content/less/sn-btn.less',
        'content/less/tags-cloud.less',
        'content/less/like-mats.less',
        'content/less/by-date-m.less',
        'content/less/share42init.less',
        'content/less/pop-mat.less',
        'content/less/banners.less',
        'content/less/identity-page.less',
        'content/less/video.less',
        'content/less/aphorisms.less',
        'content/less/site-quetions.less',
        'content/less/form-transparent.less',
        'content/less/game-details.less',
        'content/less/employee.less',
        'content/less/th-banner.less'
    ])
        .pipe(less())
        .pipe(autoprefixer('last 2 version', 'safari 5', 'ie 8', 'ie 9'))
        .pipe(minifyCss())
        .pipe(concat('less-files.less'));

    var cssStream = gulp.src([
        'bower_components/bootstrap/dist/css/bootstrap.min.css',
        'bower_components/font-awesome/css/font-awesome.min.css',
    ])
        .pipe(concat('css-files.css'));

    var mergedStream = merge(lessStream, cssStream)
        .pipe(concat('site.min.css'))
        .pipe(gulp.dest('content/dist/css'));

    gulp.src([
        'content/less/error-page.less',
    ])
        .pipe(less())
        .pipe(autoprefixer('last 2 version', 'safari 5', 'ie 8', 'ie 9'))
        .pipe(minifyCss())
        .pipe(rename({
            suffix: '.min'
        }))
        .pipe(gulp.dest('content/dist/css'));
}

function createJs() {
    gulp.src([
        'bower_components/jquery/dist/jquery.min.js',
        'bower_components/jquery-lazy/jquery.lazy.min.js',
        'bower_components/jquery-ajax-unobtrusive/jquery.unobtrusive-ajax.min.js',
        'bower_components/bootstrap/dist/js/bootstrap.min.js',
        'scripts/ge-game-menu.js',
        'scripts/find-engine.js',
        'scripts/user-clicks-engine.js',
        'scripts/site.js'
    ])
        .pipe(concat('site.min.js'))
        .pipe(uglify())
        .pipe(gulp.dest('content/dist/js'));

    gulp.src([
        'scripts/ge-for-gamers-block.js',
        'scripts/ge-last-news-block.js',
        'scripts/ge-last-category-block.js',
        'scripts/currency-provider.js',
        'scripts/ge-aphorisms.js',
        'bower_components/jquery-validation/dist/jquery.validate.js',
        'bower_components/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js'
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