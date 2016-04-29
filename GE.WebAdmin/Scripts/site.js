function resizeModal(s)
{
    var mode = $(s).data('mode');
    $modal = $(s).closest('.modal-dialog');
    $modal.attr('class', 'modal-dialog modal-' + mode);
}