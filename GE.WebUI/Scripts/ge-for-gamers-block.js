/// <reference path="D:\GE\GE.WebUI\bower_components/jquery/dist/jquery.min.js" />
(function ($) {
    $.fn.geFGB = function (lettersCount) {
        this.each(function () {
            var $this = $(this);
            var $previewBox = $('#materials-preview');
            var width = (window.innerWidth > 0) ? window.innerWidth : screen.width;
            
            $this.find('.games > li > a[rel="noreferrer"]').click(function () {
                var $a = $(this);
                var $li = $a.closest('li');
                $this.find('.games > li').not($li).removeClass('selected');
                $li.toggleClass('selected');
                var $dropdown = $li.children('.dropdown').toggle();
                $this.find('.games > li > .dropdown').not($dropdown).hide();
                return false;
            });

            $this.find('.dropdown > li > a').click(function () {
                var $a = $(this);
                var gameId = $a.data('game-id');
                var articleType = $a.data('article-type');
                var url = '/articles/preview?gameId=' + gameId + '&articleType=' + articleType+'&lettersCount='+lettersCount;
                $.ajax({
                    method: 'get',
                    url: url,
                    beforeSend: function () {
                        $a.prepend('<i class="fa fa-spin fa-spinner" style="margin-right:15px;"></i>');
                    },
                    success: function (data) {
                        $previewBox.html(data);
                        if (width < 768) {
                            $('html, body').animate({
                                scrollTop: ($previewBox.offset().top - $(window).height())
                            }, 100);
                        }
                    },
                    complete: function () {
                        $a.find('.fa-spin').remove();
                    }
                });
                return false;
            });
        });
    };
})(jQuery);