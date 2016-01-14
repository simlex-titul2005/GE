/// <binding Clean='clean' />
"use strict";

var gulp = require('gulp'),
    concat = require('gulp-concat'),
    less = require('gulp-less');

var paths = {
    webroot: "./wwwroot/"
};

gulp.task("less", function () {
    //return gulp.src([
    //    paths.webroot + 'less/fonts.less',
    //    paths.webroot + 'less/bootstrap-reset.less',
    //    paths.webroot + 'less/app.less',
    //    paths.webroot + 'less/ge-games-menu.less'
    //])
    //        .pipe(less())
    //        .pipe(concat('less.less'))
    //        .pipe(gulp.dest(paths.webroot + 'dist/css/app.min.css'))
});
