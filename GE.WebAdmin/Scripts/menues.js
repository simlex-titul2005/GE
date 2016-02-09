//delete menu item
function deleteMenuItem(button, menuId, menuItemId, url)
{
    var btn = $(button);
    var li = btn.closest('li');
    $.ajax({
        method: 'post',
        url: url,
        data: { menuId: menuId, id: menuItemId },
        beforeSend:function(){
            $('<i class="fa fa-spin fa-spinner"></i>').prependTo(li);
        },
        success:function(html){
            $('#menu-' + menuId).html(html);
        }
    });
}