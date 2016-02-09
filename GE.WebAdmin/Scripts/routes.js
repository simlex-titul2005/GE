//delete route value
function deleteRouteValue(button, routeId, routeValueId, url)
{
    var btn = $(button);
    var li = btn.closest('li');
    $.ajax({
        method: 'post',
        url: url,
        data: { routeId: routeId, id: routeValueId },
        beforeSend:function(){
            $('<i class="fa fa-spin fa-spinner"></i>').prependTo(li);
        },
        success:function(html){
            $('#route-' + routeId).html(html);
        }
    });
}