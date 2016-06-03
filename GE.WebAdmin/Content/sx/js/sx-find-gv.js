/// <reference path="../../../bower_components/jquery/dist/jquery.min.js" />

$(function () {
    $('.sx-find-gv .input-group [type="text"]').click(function () {
        var $input = $(this);
        var $box = $input.closest('.sx-find-gv');
        var isLoad = $box.attr('data-is-load');
        var $dropdown = $box.children('.dropdown');
        if (isLoad=='false') {
            getFindTableHtml($box);
        }
        $('.sx-find-gv .dropdown').not($dropdown).hide();
        $dropdown.slideToggle('fast');
    });

    $('.sx-find-gv .input-group-btn button:first-child').click(function () {
        $(this).closest('.input-group').find('[type="text"]').trigger('click');
    });

    $('.sx-find-gv .input-group-btn button:last-child').click(function () {
        var $box = $(this).closest('.sx-find-gv');
        var $txt=$(this).closest('.input-group').find('[type="text"]');
        $txt.val(null);
        $box.find('[type="hidden"]').val(null);
        $txt.trigger('change');
        $box.find('.dropdown').hide();
    });
});

function getFindTableHtml(box) {
    var url = $(box).attr('data-url');
    $.ajax({
        method: 'post',
        url: url,
        data: { page: $(box).data('page') },
        beforeSend: function () {
            $(box).find('button').first().prepend('<i class="fa fa-spinner fa-spin"></i> ');
        },
        success: function (data) {
            $(box).find('.dropdown').html(data);
        },
        complete: function () {
            $(box).find('button i.fa-spin').remove();
            $(box).attr('data-is-load', true);
        }
    });
}