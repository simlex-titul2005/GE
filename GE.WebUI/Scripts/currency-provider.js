$(function () {
    $('a[data-currency-cc]').click(function () {
        getCurrency(this);
    });
});

//получить курс валюты
function getCurrency(s, e)
{
    var charCode = $(s).data('currency-cc');
    $.ajax({
        url: '/valutes/getcurcourse',
        data: { cc: charCode },
        method: 'post',
        success: function (data) {
            $(s).html(data.Value + ' <i class="fa fa-rub"></i>');
        }
    });
}