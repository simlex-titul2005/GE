/// <reference path="D:\GE\GE.WebUI\bower_components/jquery/dist/jquery.min.js" />
(function ($) {
    $.fn.geGameMenu = function () {
        this.each(function () {
            var $this = $(this);

            $this.find('[data-toggle="tooltip"]').tooltip({
                container: $this
            });
        });
    };
})(jQuery);