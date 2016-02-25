/// <reference path="D:\GE\GE.WebUI\bower_components/jquery/dist/jquery.min.js" />
(function ($) {
    $.fn.geFGB = function () {
        this.each(function () {
            var $this = $(this);
            
            $this.find('.games > li > a[rel="noreferrer"]').click(function () {
                var $a = $(this);
                var $li = $a.closest('li');
                $this.find('.games > li').not($li).removeClass('selected');
                $li.toggleClass('selected');
                var $dropdown = $li.children('.dropdown').toggle();
                $this.find('.games > li > .dropdown').not($dropdown).hide();
                return false;
            });
        });
    };
})(jQuery);