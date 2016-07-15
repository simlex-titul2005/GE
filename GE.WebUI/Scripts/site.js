//поменять элементы местами
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

//javascript переход по ссылке
function changeLocation(element) {
    var href = $(element).find('a').attr('href');
    window.location = href;
}

//once load script
function loadScriptOnce(src) {

    var list = document.getElementsByTagName('script');
    var i = list.length, flag = false;
    while (i--) {
        if (list[i].src == src) {
            flag = true;
        }
    }
    if (!flag) {
        var tag = document.createElement('script');
        tag.src = src;
        var firstScriptTag = document.getElementsByTagName('script')[0];
        firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
    }
}

//video
var players = new Array();
var player;

function onPlayerReady(event) {
    if (players[0]) {
        players[0].stopVideo();
        players.splice(0, 1);
    }
    players.push(event.target);
    players[0].playVideo();
}

function playVideoById(id, videoId) {
    player = new YT.Player(id, {
        videoId: videoId,
        events: {
            'onReady': onPlayerReady,
        }
    });
}

//resize footer
function resizeFooter() {
    var $footer = $('footer');
    var h = $footer.height();
    $footer.css({
        'position': 'absolute'
    });
    $('body').css('margin-bottom', h + 'px');
}