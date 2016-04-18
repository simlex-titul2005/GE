/// <reference path="../bower_components/jquery/dist/jquery.min.js" />

function sendUserClick(mid, mct, uct) {
    $.ajax({
        method: 'post',
        url: '/userclicks/click',
        data: { mid: mid, mct: mct, uct: uct },
        success: function (data) {
            return data;
        }
    });
}