/// <binding Clean='clean' />
"use strict";

var gulp = require('gulp'),
    less = require('gulp-less');

var paths = {
    webroot: "./wwwroot/"
};

gulp.task("less", function () {
    //return gulp.src([paths.webroot + 'less/fonts.less'])
    //        .pipe(less())
    //        .pipe(concat('less.less'))
    //        .pipe(gulp.dest(paths.webroot + 'dist/css/app.min.css'))
});
