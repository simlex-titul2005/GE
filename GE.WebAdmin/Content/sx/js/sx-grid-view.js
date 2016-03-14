function fillGridViewForm(guid) {
    var gridView = $('.sx-grid-view[id="' + guid + '"]');
    var form = $('#grid-view-form-' + guid);

    var filterRow = gridView.find('.filter-row');
    if (filterRow.length != 0) {
        filterRow.find('input').each(function (i, input) {
            var column = $(input).attr('name');
            var value = $(input).val();
            var formInput = form.find('input[name="' + column + '"]');
            if (value != '')
                formInput.val(value);
            else
                formInput.removeAttr('value');
        });
    }

    var sordedColumns = gridView.find('.ordered-column');
    if (sordedColumns.length != 0) {
        sordedColumns.each(function (i, column) {
            var name = $(column).attr('data-column-name');
            var direction = $(column).attr('data-sort-direction');
            formInput = form.find('input[name="order[' + name + ']"]').val(direction);
        });
    }
}

function pressGridViewColumn(e) {
    var guid = $(e).closest('table').attr('id');
    var gridView = $('.sx-grid-view[id="' + guid + '"]');

    var direction = $(e).attr('data-sort-direction');
    var column = $(e).attr('data-column-name');
    direction = direction == 'Asc' ? 'Desc' : 'Asc';
    gridView.find('th.ordered-column').attr('data-sort-direction', 'Unknown');
    $(e).attr('data-sort-direction', direction);


    var page = $('.sx-grid-view[id="' + guid + '"] .sx-pager li.active a').text();
    $('#grid-view-form-' + guid + ' input[name="page"]').val(page);

    fillGridViewForm(guid);
    $('#grid-view-form-' + guid).submit();
}

function resetGridViewFilter(e) {
    var guid = $(e).closest('table').attr('id');
    var gridView = $('.sx-grid-view[id="' + guid + '"]');
    gridView.find('th.ordered-column').attr('data-sort-direction', 'Unknown');
    $('#grid-view-form-' + guid).submit();
}

function pressGridViewFilter(e) {
    var keyCode = (window.event) ? e.which : e.keyCode;
    if (keyCode == 13) {
        var guid = $(e.target).closest('table').attr('id');
        fillGridViewForm(guid);
        $('#grid-view-form-' + guid).submit();
    }
}

function clickPager(e) {
    var guid = $(e).closest('table').attr('id');
    var page = $(e).attr('data-page');
    var $footer = $(e).closest('tfoot');
    $('<i></i>').addClass('fa fa-spin fa-spinner').appendTo('tfoot td:first-child');
    fillGridViewForm(guid);
    var form = $('#grid-view-form-' + guid);
    form.find('input[name=\"page\"]').val(page);
    form.submit();
}