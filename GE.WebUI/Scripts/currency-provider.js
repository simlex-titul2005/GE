$(function () {
    $('a[data-currency-cc]').each(function () {
        $(this).tooltip();
        $(this).one('click', function () {
            getCurrency(this);
        });
    });
});

//получить курс валюты
function getCurrency(s, e) {
    var charCode = $(s).data('currency-cc');
    var value = parseFloat($(s).data('value').replace(/ /g,''));
    $.ajax({
        url: '/valutes/getcurcourse',
        data: { cc: charCode },
        method: 'post',
        success: function (data) {
            $(s).html(getCurrencyString(Math.round(data.Value * value, 0)) + ' <i class="fa fa-rub"></i>');
            $(s).tooltip('destroy');
        }
    });
}

function getCurrencyString(n)
{
    return n.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1 ');
}