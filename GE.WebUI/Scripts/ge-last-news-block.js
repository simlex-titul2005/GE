/// <reference path="D:\GE\GE.WebUI\bower_components/jquery/dist/jquery.min.js" />
(function ($) {
    jQuery.fn.swap = function (b) {
        b = jQuery(b)[0];
        var a = this[0];
        if (a === b) return;

        var a2 = a.cloneNode(true),
            b2 = b.cloneNode(true),
            stack = this;

        a.parentNode.replaceChild(b2, a);
        b.parentNode.replaceChild(a2, b);

        stack[0] = a2;
        return this.pushStack(stack);
    };

    $.fn.geLastNewsBlock = function () {
        var width = (window.innerWidth > 0) ? window.innerWidth : screen.width;

        this.each(function () {
            var $this = $(this);
            var $menu = $this.find('.menu');
            var $container = $this.find('.games-wrapper');
            var padTop = parseInt($container.css('padding-top').replace('px', ''));
            var padBottom = parseInt($container.css('padding-bottom').replace('px', ''));
            var H = $menu.height() - padTop - padBottom;

            $this.find('.menu a').mouseenter(function () {
                if (width < 768) return;

                $this.find('.menu a').removeClass('hover');
                var $a = $(this);
                $a.addClass('hover');
                var gameId = $a.data('game-id');
                var $firstFigure = $this.find('.games figure');
                var $figure = $container.find('figure[data-game-id="' + gameId + '"]');
                $figure.swap($firstFigure);
            });

            $this.find('.games img').one('load', function () {
                if (width < 768) return;
                var $img = $(this);

                var W = $img.closest('figure').width();

                var realW = this.height;
                var realH = this.width;

                var w = $(this).width();
                var h = $(this).height();

                if (w < W)
                    $img.width(W).css('height', 'auto');
                if (w > W)
                    $img.addClass('centerHorizontal');
            }).each(function () {
                if (width < 768) return;
                if (this.complete) $(this).load();
            });
        });
    };
})(jQuery);