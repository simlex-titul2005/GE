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
                var $firstFigure = $this.find('.games figure').closest('li');
                var $figure = $container.find('figure[data-game-id="' + gameId + '"]').closest('li');
                $figure.swap($firstFigure);
                setFigures($this.find('.games > li'));
                $figure.trigger('mouseenter');
            });

            $this.mouseleave(function () {
                $(this).find('.lgnb-tags').hide();
            });

            setFigures($this.find('.games > li'));
        });

        function setFigures(e) {
            $(e).mouseenter(function () {
                if (width < 768) return;

                var $wrapper = $(this).closest('.games-wrapper');
                var $tags = $(this).children('.tags').html();
                var $t = $wrapper.find('.lgnb-tags');
                $t.html($tags);
                $t.show();
                $t.find('.tags-block').show();
            });
        }
    };
})(jQuery);