(function ($) {

    $.fn.geLastCategoryBlock = function () {
        var width = (window.innerWidth > 0) ? window.innerWidth : screen.width;

        this.each(function () {
            var $this = $(this);
            var $menu = $this.find('.menu');
            var $container = $this.find('.category-wrapper');
            var padTop = parseInt($container.css('padding-top').replace('px', ''));
            var padBottom = parseInt($container.css('padding-bottom').replace('px', ''));
            var H = $menu.height() - padTop - padBottom;

            $this.find('.menu a').mouseenter(function () {
                if (width < 768) return;

                $this.find('.menu a').removeClass('hover');
                var $a = $(this);
                $a.addClass('hover');
                var cateoryId = $a.data('category-id');
                var $firstFigure = $container.find('figure').closest('li');
                var $figure = $container.find('figure[data-category-id="' + cateoryId + '"]').closest('li');
                $figure.swap($firstFigure);
                setFigures($container.find('.sub-gategories > li'));
                $figure.trigger('mouseenter');
            });

            $this.mouseleave(function () {
                $(this).find('.lgnb-tags').hide();
            });

            setFigures($this.find('.sub-gategories > li'));
        });

        function setFigures(e) {
            $(e).mouseenter(function () {
                if (width < 768) return;

                var $wrapper = $(this).closest('.category-wrapper');
                var $tags = $(this).children('.tags').html();
                var $t = $wrapper.find('.lgnb-tags');
                $t.html($tags);
                $t.show();
                $t.find('.tags-block').show();
            });
        }
    };
})(jQuery);