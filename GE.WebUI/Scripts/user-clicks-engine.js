/// <reference path="../bower_components/jquery/dist/jquery.min.js" />

//send like
function sendLike(element) {
    $btn = $(element);
    var url = $btn.attr('data-url');
    $badge = $btn.find('.share-buttons__counter');
    var aft = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        method: 'post',
        url: url,
        data: { __RequestVerificationToken: aft },
        success: function (data) {
            $badge.html(data);
        }
    });
}