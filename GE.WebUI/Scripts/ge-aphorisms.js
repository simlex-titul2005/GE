/// <reference path="../bower_components/jquery/dist/jquery.min.js" />
(function ($) {
    $.fn.geAphorisms = function () {
        var mouseInBlock = false;
        var period;
        var periodCounter=0;

        this.each(function () {
            $this = $(this);
            var width = (window.innerWidth > 0) ? window.innerWidth : screen.width;
            $ap = $this.find('.aphorism');
            var mid = $ap.data('mid');
            var lc = $ap.data('lc');
            period = getAphorismPeriod(lc);

            $this.on('mouseover', function () {
                mouseInBlock = true;
                clearInterval(interval);
            });

            $this.on('mouseout', function () {
                mouseInBlock = false;
                interval = setInterval(function () { recountProgressBar() }, 1000);
            });

            var timeOut = setTimeout(function () { getRandom(mid) }, period);
            var interval = setInterval(function () { recountProgressBar() }, 1000);

            function getRandom(mid) {
                if (!mouseInBlock) {
                    $.ajax({
                        method: 'get',
                        url: '/aphorism/random?id=' + mid,
                        success: function (data) {
                            var html = $(data).find('.aph-html').html();
                            $this.fadeOut('fast', function () {
                                $this.html(data);
                                $this.fadeIn('fast');
                            });
                            var lc = getLettersCount(html);
                            period = getAphorismPeriod(lc);
                            periodCounter = 0;
                            clearTimeout(timeOut);
                            timeOut = setTimeout(function () { getRandom(mid) }, period);
                            clearInterval(interval);
                            interval = setInterval(function () { recountProgressBar() }, 1000);
                        }
                    });
                }
                else {
                    var html = $ap.find('.aph-html').html();
                    var lc = getLettersCount(html);
                    clearTimeout(timeOut);
                    timeOut = setTimeout(function () { getRandom(mid) }, getAphorismPeriod(lc));
                    clearInterval(interval);
                }
            }

            function getLettersCount(html) {
                return html.length;
            }

            function getAphorismPeriod(lettersCount) {
                return lettersCount * 200 / 3;
            }

            function recountProgressBar() {
                periodCounter = periodCounter == 0 ? period - 1000 : periodCounter-1000;
                $p = $this.find('.aph-progress > div')
                    .animate({
                        'width': periodCounter * 100 / period + '%'
                    });
            }
        });
    };
})(jQuery);