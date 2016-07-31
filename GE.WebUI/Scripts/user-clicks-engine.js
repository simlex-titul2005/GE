/// <reference path="../bower_components/jquery/dist/jquery.min.js" />

function sendUserClick(mid, mct, uct) {
    var aft = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        method: 'post',
        url: '/userclicks/click',
        data: { __RequestVerificationToken: aft, mid: mid, mct: mct, uct: uct },
        success: function (data) {
            return data;
        }
    });
}

function sendLikeButtonClick(element, mid, mct, uct, ldir) {
    $btn = $(element);
    $badge = $btn.find('.share-buttons__counter');
    var aft = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        method: 'post',
        url: '/userclicks/click',
        data: { __RequestVerificationToken: aft, mid: mid, mct: mct, uct: uct, ldir: ldir },
        success: function (data) {
            $badge.html(data);
        }
    });
}