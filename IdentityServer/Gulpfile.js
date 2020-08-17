/// <binding BeforeBuild='build-css' />
const gulp = require('gulp'),
    sass = require("gulp-sass"); 

const buildSass = () => gulp.src('./wwwroot/css/*.scss')
    .pipe(sass())
    .pipe(gulp.dest('./wwwroot/css'))

gulp.task('build-css', buildSass); 

gulp.task('default', gulp.series('build-css'))

exports.default = function () {
    // You can use a single task
    gulp.watch('./wwwroot/css/*.scss', buildSass);
};

