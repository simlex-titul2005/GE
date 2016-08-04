(function ($) {
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
                //$(this).find('.lgnb-dropdown').hide();
            });

            setFigures($this.find('.games > li'));
        });

        function setFigures(e) {
            $(e).mouseenter(function () {
                if (width < 768) return;

                $wrapper = $(this).closest('.games-wrapper');
                $videos = $(this).children('.videos').html();
                $tags = $(this).children('.tags').html();

                $t = $wrapper.find('.lgnb-dropdown');
                $t.html($videos + $tags);
                $t.show();
                $t.find('.tags-block').show();
            });
        }
    };
})(jQuery);