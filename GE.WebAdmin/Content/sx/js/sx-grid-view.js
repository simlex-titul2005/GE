/// <reference path="../../../bower_components/jquery/dist/jquery.min.js" />

function fillGridViewForm(guid) {
    var gridView = $('.sx-gv[id="' + guid + '"]');
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
        sordedColumns.each(function () {
            var $column = $(this);
            var name = $column.attr('data-column-name');
            var direction = $column.attr('data-sort-direction');
            formInput = form.find('input[name="order[' + name + ']"]').val(direction);
        });
    }
}

function fillGridViewFormLookup(guid, page, filter)
{
    var dataUrl = $('#' + guid).data('data-url');
    $.ajax({
        url: dataUrl,
        method: 'post',
        data: { page: page, filterModel: filter },
        success: function (data) {
            $('#' + guid).closest('.dropdown').html(data);
        }
    });
}

function pressGridViewColumn(e) {
    var guid = $(e).closest('table').attr('id');
    var gridView = $('.sx-gv[id="' + guid + '"]');

    var direction = $(e).attr('data-sort-direction');
    var column = $(e).attr('data-column-name');
    direction = direction == 'Asc' ? 'Desc' : 'Asc';
    gridView.find('th.ordered-column').attr('data-sort-direction', 'Unknown');
    $(e).attr('data-sort-direction', direction);

    var page = $('.sx-gv[id="' + guid + '"] .sx-pager li.active a').text();
    $('#grid-view-form-' + guid + ' input[name="page"]').val(page);

    fillGridViewForm(guid);
    $('#grid-view-form-' + guid).submit();
}

//reset filter
function resetGridViewFilter(e) {
    var guid = $(e).closest('table').attr('id');
    var gridView = $('.sx-grid-view[id="' + guid + '"]');
    gridView.find('th.ordered-column').attr('data-sort-direction', 'Unknown');
    $('#grid-view-form-' + guid).submit();
}

function resetGridViewLookupFilter(e) {
    var guid = $(e).closest('table').attr('id');
    fillGridViewFormLookup(guid, 1, null);
}


//press filter
function pressGridViewFilter(e) {
    var keyCode = (window.event) ? e.which : e.keyCode;
    if (keyCode == 13) {
        var guid = $(e.target).closest('table').attr('id');
        fillGridViewForm(guid);
        $('#grid-view-form-' + guid).submit();
        
    }
}

function pressGridViewLookupFilter(e) {
    var keyCode = (window.event) ? e.which : e.keyCode;
    if (keyCode == 13) {
        var guid = $(e.target).closest('table').attr('id');
        var $tr = $(e.target).closest('tr');
        var filter = getLookupFilter($tr);

        fillGridViewFormLookup(guid, 1, filter);
        e.preventDefault();
    }
}

//click pager
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

function clickLookupPager(e) {
    var guid = $(e).closest('table').attr('id');
    var $tr = $(e).closest('table').find('.filter-row');
    var filter = getLookupFilter($tr);
    var page = $(e).attr('data-page');
    var $footer = $(e).closest('tfoot');
    $('<i></i>').addClass('fa fa-spin fa-spinner').appendTo('tfoot td:first-child');
    fillGridViewFormLookup(guid, page, filter);
}

//click row
function clickLookupRow(e) {
    var $tr = $(e);
    var id = $tr.data('id');
    var $txtFieldCell = $tr.children('[data-text-field]');
    var $dropdown = $tr.closest('.dropdown');
    var $input = $dropdown.parent().find('input[type="hidden"]').val(id);
    var $text = $dropdown.parent().find('.input-group input[type="text"]').val($txtFieldCell.text());
    $dropdown.hide();
}

function getLookupFilter(tr)
{
    var $tr = $(tr);
    var filter = {};
    $tr.find('input').each(function () {
        var propName = $(this).attr('name');
        var propValue = $(this).val();
        if (propValue != "") {
            filter[propName] = propValue;
        }
    });

    return filter;
}