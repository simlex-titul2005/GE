/// <binding Clean='clean' />
"use strict";

var gulp = require('gulp'),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    less = require("gulp-less");

var paths = {
    webroot: "./wwwroot/"
};

paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";

/*clean*/
gulp.task("clean:css", function (cb) {
    rimraf(paths.webroot + 'css/**/app*.css', cb);
});

gulp.task("clean", ["clean:css"]);

/*less fiels to css*/
gulp.task("less", function () {
    gulp.src([
        paths.webroot + 'less/bootstrap-reset.less',
        paths.webroot + 'less/app.less',
        paths.webroot + 'less/ge-games-menu.less',
    ])
        .pipe(concat('app.less'))
        .pipe(less())
        .pipe(gulp.dest(paths.webroot + 'css'))
});

/*min files*/
gulp.task("min:css", function () {
    return gulp.src([paths.webroot + 'css/app.css', paths.webroot + 'css/fonts.css'])
        .pipe(concat(paths.webroot+'css/app.min.css'))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min", ["min:css"]);

/*run*/
gulp.task("min", ["clean", "less", "min:css"]);
