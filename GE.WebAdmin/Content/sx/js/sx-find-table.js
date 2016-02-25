/// <reference path="D:\sites\GE\GE.WebAdmin\bower_components/jquery/dist/jquery.min.js" />

$(function () {
    $('.sx-find-table .input-group [type="text"]').click(function () {
        var $input = $(this);
        var $box = $input.closest('.sx-find-table');
        var isLoad = $box.data('is-load');
        var $dropdown = $box.children('.dropdown');
        if (!isLoad) {
            getFindTableHtml($box);
        }
        $('.sx-find-table .dropdown').not($dropdown).hide();
        $dropdown.slideToggle('fast');
    });

    $('.sx-find-table .input-group-btn button:first-child').click(function () {
        $(this).closest('.input-group').find('[type="text"]').trigger('click');
    });

    $('.sx-find-table .input-group-btn button:last-child').click(function () {
        var $box = $(this).closest('.sx-find-table');
        var $txt=$(this).closest('.input-group').find('[type="text"]');
        $txt.val(null);
        $box.find('[type="hidden"]').val(null);
        $txt.trigger('change');
    });
});

function findTableRowClick(e) {
    var $box = $(e).closest('.sx-find-table');
    var $hidden = $box.children('input[type="hidden"]');
    var $input = $box.find('.input-group [type="text"]');
    var $dropdown = $box.children('.dropdown');
    var id = $(e).data('id');
    var textField=$(e).find('td[data-text-field]');
    $input.val(textField.text());
    $hidden.val(id);
    $dropdown.hide();
    $input.trigger('change');
}

function findTablePagerClick(e) {
    var $a = $(e);
    var $box = $a.closest('.sx-find-table');
    $box.data('is-load', false);
    var page = $a.data('page');
    $box.data('page', page);
    getFindTableHtml($box);
    return false;
}

function getFindTableHtml(box) {
    $.ajax({
        method: 'post',
        url: $(box).data('url'),
        data: { page: $(box).data('page') },
        beforeSend: function () {
            $(box).find('button').first().prepend('<i class="fa fa-spinner fa-spin"></i> ');
        },
        success: function (data) {
            $(box).find('.dropdown').html(data);
        },
        complete: function () {
            $(box).find('button i.fa-spin').remove();
            $(box).data('is-load', true);
            var $dropdown = $(box).find('.dropdown');
            $dropdown.find('tbody tr').click(function () {
                findTableRowClick(this);
            });
            $dropdown.find('tfoot a').click(function () {
                findTablePagerClick(this);
            });
        }
    });
}