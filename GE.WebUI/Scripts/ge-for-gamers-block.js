/// <reference path="D:\GE\GE.WebUI\bower_components/jquery/dist/jquery.min.js" />
(function ($) {
    $.fn.geFGB = function (lettersCount) {
        this.each(function () {
            var $this = $(this);
            var $previewBox = $('#materials-preview');
            var width = (window.innerWidth > 0) ? window.innerWidth : screen.width;
            
            $this.find('.games > li > a[rel="nofollow"]').click(function () {
                var $a = $(this);
                var $li = $a.closest('li');
                $this.find('.games > li').not($li).removeClass('selected');
                $li.toggleClass('selected');

                var gameTitle = $a.data('game-title');
                var url = '/articles/preview?gameTitle=' + gameTitle + '&lettersCount=' + lettersCount;
                $.ajax({
                    method: 'get',
                    url: url,
                    beforeSend: function () {
                        
                    },
                    success: function (data) {
                        $previewBox.html(data);
                    },
                    complete: function () {
                        
                    }
                });
                return false;
            });

            $this.find('.dropdown > li > a').click(function () {
                var $a = $(this);
                var gameTitle = $a.data('game-title');
                var articleType = $a.data('article-type');
                var url = '/articles/preview?gameTitle=' + gameTitle + '&articleType=' + articleType + '&lettersCount=' + lettersCount;
                $.ajax({
                    method: 'get',
                    url: url,
                    beforeSend: function () {
                        $a.prepend('<i class="fa fa-spin fa-spinner" style="margin-right:15px;"></i>');
                    },
                    success: function (data) {
                        $previewBox.html(data);
                    },
                    complete: function () {
                        $a.find('.fa-spin').remove();
                        $this.find('.dropdown > li > a').removeClass('active');
                        $a.toggleClass('active');
                        if (width < 768) {
                            var st = $previewBox.closest('div').offset().top-120;
                            console.log(st);
                            $('html, body').animate({
                                scrollTop: st
                            }, 100);
                        }
                    }
                });
                return false;
            });
        });
    };
})(jQuery);