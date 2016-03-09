/// <reference path="D:\sites\GE\GE.WebUI\bower_components/jquery/dist/jquery.min.js" />

function sendClickStat(element, rawUrl, clickTypeId) {
    $.ajax({
        method: 'post',
        url: '/clicks/click',
        data: { rawUrl: rawUrl, target: null, clickTypeId: clickTypeId }
    });
    return true;
}