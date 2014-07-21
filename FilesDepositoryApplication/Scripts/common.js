
/**
*Global spinner for all pages
* @constructor
*/
function LogoAnimation() {
    _this = this;
    var $container = $("#logo");
    this.start = function() {
        $container.switchClass("ajax-spinner-static", "ajax-spinner-animated");
    };
    this.stop = function () {
        $container.switchClass("ajax-spinner-animated", "ajax-spinner-static");
    };
    this.init = function() {
        $(document)
          .ajaxStart(function () {
              _this.start();
            })
          .ajaxStop(function () {
              _this.stop();
        });
    };
}

var globalAnimation = null;
$(document).ready(function () {
    globalAnimation = new LogoAnimation();
    globalAnimation.init();
});
