/// <binding Clean='clean' />
"use strict";

var gulp = require('gulp'),
    concat = require('gulp-concat'),
    less = require('gulp-less'),
    rimraf = require('rimraf'),
    cssmin = require("gulp-cssmin");

var paths = {
    webroot: "./wwwroot/"
};

gulp.task("clear", function (cb) {
    rimraf(paths.webroot + 'dist', cb);
});


gulp.task("min:css", function () {
    return gulp.src([
        paths.webroot + 'less/fonts.less',
        paths.webroot + 'less/bootstrap-reset.less',
        paths.webroot + 'less/app.less',
        paths.webroot + 'less/ge-games-menu.less'
    ])
            .pipe(less())
            .pipe(cssmin())
            .pipe(concat('app.min.css'))
            .pipe(gulp.dest(paths.webroot + 'dist/css'))
});

gulp.task("copy:fonts", function () {
    gulp.src(paths.webroot + 'lib/opensanscondensed-googlefont/OpenSans-CondLight*')
        .pipe(gulp.dest(paths.webroot + 'dist/fonts'));
});

gulp.task("min", ["clear", "min:css", "copy:fonts"]);
