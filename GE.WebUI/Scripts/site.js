
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


function changeLocation(element) {
    var href = $(element).find('a').attr('href');
    window.location = href;
}

function playVideo(s)
{
    $a = $(s);
    $container = $a.closest('.video');
    $wrapper = $container.children('.wrapper');
    var guid = $container.attr("id");
    var src = $a.data('src');
    $frame = '<div class="embed-responsive embed-responsive-16by9"><iframe id="video-' + guid + '" class="embed-responsive-item"  src="' + src + '?autoplay=1" /></div>';
    removeOtherVideo();
    $wrapper.hide();
    $container.append($frame);
    addVideoView(guid);
}

function addVideoView(guid)
{
    $.ajax({
        method: 'post',
        url: '/videos/addview',
        data: { videoId: guid },
        success:function()
        {

        }
    });
}

function removeOtherVideo()
{
    $('.video .embed-responsive').remove();
    $('.video .wrapper').show();
}