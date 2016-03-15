/// <reference path="D:\GE\GE.WebUI\bower_components/jquery/dist/jquery.min.js" />
$(function () {
    var width = (window.innerWidth > 0) ? window.innerWidth : screen.width;
    $('#seach-block button').first().click(function () {
        $btn = $(this);
        $btn.toggleClass('active');
        $('#seach-block .lg-block').toggle();
    });

    $('#seach-block').submit(function (event) {
        var key = width >= 780 ? $(this).find('#lg-input').val() : $(this).find('#xs-input').val();
        siteFind(key);
        return false;
    });
});

function siteFind(key) {
    $.ajax({
        method: 'get',
        url: '/search/list?key=' + key,
        beforeSend: function () {
            $('#lg-input').next('.fa-spinner').show();
        },
        success: function (data) {
            $('#search-res').html(data);
        },
        complete: function () {
            $('#lg-input').next('.fa-spinner').hide();
        }
    });
}