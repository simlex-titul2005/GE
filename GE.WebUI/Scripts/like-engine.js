function setMaterialLike(mid, mct, sid, uid, dir, e) {
    $.ajax({
        method: 'post',
        url: '/likes/like',
        data: { MaterialId: mid, ModelCoreType: mct, SessionId: sid, UserId: uid, Direction: dir },
        success: function (data) {
            if (data === -1) {
                $(e).popover('show');
                $(e).removeAttr('onclick');
                return;
            }
            else {
                var count = parseInt($(e).next().text());
                $(e).next().text(count + 1);
            }
        }
    });
}