var gulp = require('gulp');
var typescript = require('gulp-tsc');
var uglify = require('gulp-uglify');
var less = require('gulp-less');
var csso = require('gulp-csso');
var changed = require('gulp-changed');
var sourcemaps = require('gulp-sourcemaps');
var concat = require('gulp-concat');
var gulpFilter = require('gulp-filter');
var del = require('del');
var runSequence = require('run-sequence');
var download = require('gulp-download');
var inject = require("gulp-inject");
var rev = require('gulp-rev');

var filePath = {
    less: [
        "../Content/dashboard/*.less",
        "../Content/font-awesome/font-awesome.less",
        "../Content/tools/*.less",
        "../Content/ui/*.less",
        "../Content/users/*.less",
        "../Content/*.less",
        "../Areas/**/*.less",
        '../temp/zenburn.min.css'
    ],
    scripts: [
        "../temp/highlight.min.js",
        "../temp/recaptcha_ajax.js",
        "../Scripts/Libs/jquery-2.1.1.js",
        "../Scripts/Libs/jquery.signalR-2.1.1.js",
        "../Scripts/Libs/jquery.cookie.js",
        "../Scripts/Libs/jquery.nanoscroller.js",
        "../Scripts/Libs/jquery.naturalWidth.js",
        "../Scripts/Libs/jquery.hashchange.js",
        "../Scripts/Libs/jquery.autosize.js",
        "../Scripts/Libs/MD5.js",
        "../Scripts/Libs/angular.js",
        "../Scripts/Libs/angular-recaptcha.js",
        "../Scripts/Libs/ng-quick-date.js",
        "../Scripts/Libs/ui-bootstrap-custom-0.5.0.js",
        "../Scripts/Libs/eventEmitter.js",
        "../Scripts/Libs/linq.js",
        "../Scripts/Libs/tinycon.js",
        "../Scripts/Libs/ion.sound.js",
        "../Scripts/Libs/moment.js",
        "../Scripts/Libs/markdown/AutoLinkTransform.ts",
        "../Scripts/Libs/markdown/*.js",
        "../Scripts/Core/Base64.ts",
        "../Scripts/Core/Utils.ts",
        "../Scripts/Core/Config.ts",
        "../Scripts/TypeLiteEnums.ts",
        "../Scripts/Settings.ts",
        "../Scripts/Commands.ts",
        "../Scripts/BasePage.ts",
        "../Scripts/Core/Application.ts",
        "../Scripts/Core/CurrentUser.ts",
        "../Scripts/Core/ApiService.ts",
        "../Scripts/Core/Service.ts",
        "../Scripts/Core/EventBus.ts",
        "../Scripts/Core/CommandBus.ts",
        "../Scripts/Core/SocketService.ts",
        "../Scripts/Account/*.ts",
        "../Scripts/UI/*.ts",
        "../Areas/Chat/Scripts/*.ts",
        "../Areas/Topics/Scripts/*.ts",
        "../Scripts/Tools/*.ts",
        "../Scripts/Users/*.ts",
        "../Areas/Forum/Scripts/*.ts",
        "../Areas/Projects/Scripts/*.ts",
        "../Scripts/Attachment/*.ts",
        "../Scripts/MirGames.ts",
        "../Scripts/ActivationNotificationController.ts",
        "../Scripts/NavigationController.ts",
        "../Scripts/OnlineUsersController.ts",
        "../Scripts/UserNotificationController.ts",
        "../Scripts/RequestNotificationController.ts",
        "../Scripts/SocketNotificationController.ts",
        '../temp/hubs.js',
        '../temp/route.js'
    ],
    external: [
        'http://yandex.st/highlightjs/7.3/highlight.min.js',
        'http://www.google.com/recaptcha/api/js/recaptcha_ajax.js',
        'http://yandex.st/highlightjs/7.3/styles/zenburn.min.css',
        { url: 'https://local.mirgames.ru/routejs.axd', file: 'route.js' },
        { url: 'https://local.mirgames.ru/signalr/hubs', file: 'hubs.js' }
    ],
    inject: [
        '../Views/Shared/_Layout.cshtml'
    ]
};

gulp.task('clean', function (cb) {
    del(['../public'], { force: true }, cb);
});

gulp.task('clean:temp', function (cb) {
    del(['../temp'], { force: true }, cb);
});

gulp.task('download', function () {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';

    return download(filePath.external)
        .pipe(gulp.dest('../temp/'));
});

gulp.task('clean:maps', function(cb) {
    del(['../public/js/**/*.js.map'], { force: true }, cb);
});

gulp.task('compile-ts', function () {
    var tsFilter = gulpFilter('**/*.ts');
    var jsFilter = gulpFilter('**/*.js');

    return gulp.src(filePath.scripts, { base: '../' })
        .pipe(tsFilter)
        .pipe(typescript({ sourcemap: true, noImplicitAny: true, outDir: '../public/js', target: 'ES5' }))
        .pipe(tsFilter.restore())
        .pipe(jsFilter)
        .pipe(sourcemaps.init({ loadMaps: true }))
        .pipe(concat('scripts.js', { newLine: ';\r\n' }))
        .pipe(jsFilter.restore())
        .pipe(sourcemaps.write())
        .pipe(gulp.dest('../public/js'));
});

gulp.task('compile-less', function () {
    return gulp.src(filePath.less, { base: '../' })
        .pipe(sourcemaps.init())
        .pipe(less({ rootpath: '../../../Content/'  }))
        .pipe(concat('default.css'))
        .pipe(sourcemaps.write())
        .pipe(gulp.dest('../public/css'));
});

gulp.task('minimize-js', function() {
    return gulp.src('../public/js/*.js')
        .pipe(uglify())
        .pipe(gulp.dest('../public/js'));
});

gulp.task('minimize-css', function () {
    return gulp.src('../public/css/*.css')
        .pipe(csso())
        .pipe(gulp.dest('../public/css'));
});

gulp.task('dev', function(cb) {
    runSequence('clean:temp', ['download', 'clean'], ['compile-less', 'compile-ts'], ['clean:maps'], cb);
});

gulp.task('publish', function (cb) {
    runSequence('dev', ['minimize-js', 'minimize-css'], cb);
});

if (process.env.NODE_ENV === 'Release') {
    gulp.task('default', ['publish']);
} else {
    gulp.task('default', ['dev']);
}