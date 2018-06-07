
$(function () {
    //重置分页
    ETrip.QueryUtil.initPager();

    //绑定每页显示N条记录的Select框事件
    ETrip.QueryUtil.bindPageSizeChangeEvent();

    //显示列表状态
    ETrip.QueryUtil.resetRecordsRange(parseInt($('#hdfRecordCount').val()));

    //绑定排序
    $('.btn-sort-order').bind('click', ETrip.QueryUtil.sortOrder);

    //绑定搜索事件
    ETrip.QueryUtil.bindSearchEvent('#btnSearch');

    //绑定下拉框change事件
    ETrip.QueryUtil.bindChangeEvent($("#ddlIsInner"));

    query();
});

function query() {
    var requestData = {
        pageIndex: $('#hdfPageIndex').val(),
        pageSize: $('#hdfPageSize').val(),
        orderBy: $('#hdfOrderBy').val(),
        isAsc: $('#hdfIsAsc').val(),
        IsInner: $("#ddlIsInner").val()
    };

    ETrip.Util.showLoading();
    ETrip.Util.ajax({
        url: "/System/GetRoles",
        data: requestData,
        success: function (data) {
            ETrip.Util.removeLoading();
            if (data.Success) {
                $(".content").html(data.Value);
                $('#hdfRecordCount').val(data.Tag);

                //重置分页
                ETrip.QueryUtil.initPager();

                //显示列表状态
                ETrip.QueryUtil.resetRecordsRange(data.Tag);

                //更新排序状态
                ETrip.QueryUtil.updateSortStatus();
            } else {
                $.sticky(data.Message, { type: "st-error" });
            }
        }
    });
}

//新增、编辑角色
function roleEditor(id) {
    $("#dialog").show();
    $("#dialog").dialog({
        title: id == 0 ? "新增角色" : "编辑角色",
        autoOpen: false,
        modal: true,
        height: 180,
        width: 420,
        resizable: false,
        draggable: true,
        open: function () {
            $("#frameDialog").attr("src", "/System/RoleEditor?id=" + id);
        },
        close: function (event, ui) {
            $("#frameDialog").attr("src", "/Login/LoadingDialog");
            ETrip.Util.removeLoading("#dialog");
        }
    });
    $("#dialog").dialog("open");
}

//删除
function roleDelete(id) {
    if (!confirm("确认删除角色？")) {
        return false;
    }

    ETrip.Util.showLoading();
    ETrip.Util.ajax({
        url: "/System/RoleDelete",
        data: { Id: id },
        success: function (data) {
            ETrip.Util.removeLoading();
            if (data.Success) {
                $.sticky("删除成功", { type: "st-success" });
                query();
            } else {
                $.sticky(data.Message, { type: "st-error" });
            }
        }
    });
}