var promise = require('es6-promise'),
    gulp = require('gulp'),
    watch = require('gulp-watch'),
    del = require('del'),
    concat = require('gulp-concat'),
    uglify = require('gulp-uglify'),
    merge = require('merge-stream'),
    order = require('gulp-order'),
    less = require('gulp-less'),
    cleanCSS = require('gulp-clean-css'),
    autoprefixer = require('gulp-autoprefixer'),
    rename = require('gulp-rename');

function clear() {
    del([
        'content/dist/css/**/*.css',
        'content/dist/js/**/*.js',
        'content/dist/fonts/**/*'
    ]);
}

function createFonts() {
    gulp.src([
        'bower_components/font-awesome/fonts/**/*'
    ])
        .pipe(gulp.dest('content/dist/fonts'));
}

function createCss() {
    var lessStream = gulp.src([
        'less/article.less',
        'less/bootstrap-ext.less',
        'less/comments.less',
        'less/footer.less',
        'less/for-gamers-block.less',
        'less/forum.less',
        'less/game-list.less',
        'less/last-news-block.less',
        'less/last-category-news-block.less',
        'less/list-article.less',
        'less/list-news.less',
        'less/material.less',
        'less/positioned.less',
        'less/seach-block.less',
        'less/site.less',
        'less/sx-list.less',
        'less/sx-pager.less',
        'less/sn-btn.less',
        'less/tags-cloud.less',
        'less/like-mats.less',
        'less/by-date-m.less',
        'less/pop-mat.less',
        'less/banners.less',
        'less/identity-page.less',
        'less/video.less',
        'less/aphorisms.less',
        'less/site-quetions.less',
        'less/form-transparent.less',
        'less/game-details.less',
        'less/employee.less',
        'less/th-banner.less',
        'less/site-tests.less',
        'less/st-page.less',
        'less/share-buttons.less',
        'less/sx-rating.less'
    ])
        .pipe(less())
        .pipe(cleanCSS({ compatibility: 'ie8' }))
        .pipe(autoprefixer('last 2 version', 'safari 5', 'ie 8', 'ie 9'))
        .pipe(concat('sitecss.css'));

    var cssStream = gulp.src([
        'bower_components/bootstrap/dist/css/bootstrap.min.css',
        'bower_components/font-awesome/css/font-awesome.min.css',
    ])
        .pipe(concat('css.css'));

    merge(cssStream, lessStream)
        .pipe(order([
            'css.css',
            'sitecss.css'
        ]))
            .pipe(concat('site.min.css'))
            .pipe(gulp.dest('content/dist/css'));

    //by one less
    gulp.src([
       'less/error-page.less',
       'less/test-list.less',
       'less/ap-author-page.less',
       'less/humor.less',
       'less/author-page.less'
    ])
        .pipe(less())
        .pipe(cleanCSS({ compatibility: 'ie8' }))
        .pipe(autoprefixer('last 2 version', 'safari 5', 'ie 8', 'ie 9'))
        .pipe(rename({
            suffix: '.min'
        }))
        .pipe(gulp.dest('content/dist/css'));
}

function createJs() {
    var js = gulp.src([
        'bower_components/jquery/dist/jquery.min.js',
        'bower_components/bootstrap/dist/js/bootstrap.min.js'
    ])
        .pipe(concat('js.js'));

    var sitejs = gulp.src([
        'scripts/ge-game-menu.js',
        'scripts/find-engine.js',
        'scripts/user-clicks-engine.js',
        'scripts/site.js'
    ])
        .pipe(uglify())
        .pipe(concat('sitejs.js'));

    merge(js, sitejs)
        .pipe(order([
            'js.js',
            'sitejs.js'
        ]))
            .pipe(concat('site.min.js'))
            .pipe(gulp.dest('content/dist/js'));


    //by one js
    gulp.src([
        'scripts/ge-for-gamers-block.js',
        'scripts/ge-last-news-block.js',
        'scripts/ge-last-category-block.js',
        'scripts/currency-provider.js',
        'scripts/ge-aphorisms.js'
    ])
        .pipe(uglify())
        .pipe(rename({
            suffix: '.min'
        }))
        .pipe(gulp.dest('content/dist/js'));

    gulp.src([
        'bower_components/jquery-ajax-unobtrusive/jquery.unobtrusive-ajax.min.js',
        'bower_components/jquery-validation/dist/jquery.validate.min.js',
        'bower_components/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js',
        'bower_components/goodshare/goodshare.min.js'
    ])
        .pipe(gulp.dest('content/dist/js'));

}

gulp.task('watch', function (cb) {
    watch([
        'less/**/*.less',
        'scripts/**/*.js'
    ], function () {
        clear();
        createCss();
        createFonts();
        createJs();
    });
});