function resizeModal(s)
{
    var mode = $(s).data('mode');
    $modal = $(s).closest('.modal-dialog');
    $modal.attr('class', 'modal-dialog modal-' + mode);
}

//показать форму привязки видео к материалу
function showNotMaterialVideos() {
    $('#not-mat-videos').modal('show');
}

//привязать видео к материалу
function linkedVideoSelectedChange(s) {
    $cbx = $(s);
    $form = $('#not-mat-videos').find('.modal-footer').find('form');
    var id = $cbx.val();
    var name = $cbx.attr('name');
    var checked = $cbx.prop('checked');
    if (checked) {
        $form.append('<input type="hidden" name="' + name + '" value="' + id + '" />');
    }
    else {
        $hidden = $form.find('input[value="' + id + '"]');
        if ($hidden.length >= 1)
            $hidden.remove();
    }
}