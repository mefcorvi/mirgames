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

var filePath = {
    less: [
        "../Content/dashboard/*.less",
        "../Content/font-awesome/font-awesome.less",
        "../Content/tools/*.less",
        "../Content/ui/*.less",
        "../Content/users/*.less",
        "../Content/*.less",
        "../Areas/**/*.less"
    ],
    scripts: [
        "../Scripts/Libs/jquery-1.7.1.js",
        "../Scripts/Libs/jquery.signalR-2.0.2.js",
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
        "../Scripts/SocketNotificationController.ts"
    ]
};

gulp.task('clean', function (cb) {
    del(['../public'], { force: true }, cb);
});

gulp.task('clean:maps', function(cb) {
    del(['../public/js/**/*.js.map'], { force: true }, cb);
});

gulp.task('compile-ts', ['clean'], function (cb) {
    var tsFilter = gulpFilter('**/*.ts');
    var jsFilter = gulpFilter('**/*.js');

    return gulp.src(filePath.scripts, { base: '../' })
        .pipe(tsFilter)
        .pipe(typescript({ sourcemap: true, noImplicitAny: true, outDir: '../public/js', target: 'ES5' }))
        .pipe(tsFilter.restore())
        .pipe(jsFilter)
        .pipe(sourcemaps.init({ loadMaps: true }))
        .pipe(concat('scripts.js'))
        .pipe(jsFilter.restore())
        .pipe(sourcemaps.write())
        .pipe(gulp.dest('../public/js'));
});

gulp.task('compile-less', ['clean'], function () {
    return gulp.src(filePath.less, { base: '../' })
        .pipe(sourcemaps.init())
        .pipe(less({ rootpath: '../../../Content/'  }))
        .pipe(concat('default.css'))
        .pipe(sourcemaps.write())
        .pipe(gulp.dest('../public/css'));
});

gulp.task('default', function(cb) {
    runSequence(['compile-less', 'compile-ts'], 'clean:maps', cb);
});