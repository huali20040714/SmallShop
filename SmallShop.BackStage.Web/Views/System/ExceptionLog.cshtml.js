
$(function () {
    //重置分页
    ETrip.QueryUtil.initPager();

    //绑定每页显示N条记录的Select框事件
    ETrip.QueryUtil.bindPageSizeChangeEvent();

    //显示列表状态
    ETrip.QueryUtil.resetRecordsRange(parseInt($('#hdfRecordCount').val()));

    //绑定下拉框change事件
    ETrip.QueryUtil.bindChangeEvent($("#ddlVague"));
    
    //绑定排序
    $('.btn-sort-order').bind('click', ETrip.QueryUtil.sortOrder);

    //绑定搜索事件
    ETrip.QueryUtil.bindSearchEvent('#btnSearch');

    //绑定输入框回车事件
    ETrip.QueryUtil.bindKeyEvent($("input.input-sm"));

    query();
});

function query() {
    var requestData = {
        pageIndex: $('#hdfPageIndex').val(),
        pageSize: $('#hdfPageSize').val(),
        orderBy: $('#hdfOrderBy').val(),
        isAsc: $('#hdfIsAsc').val(),
        IsVague: $("#ddlVague").val(),
        LoginName: $("#txtLoginName").val(),
        Url: $("#txtUrl").val(),
        IP: $("#txtIP").val(),
        Message: $("#txtMessage").val(),
        dtStart: $("#dtStart").val(),
        dtEnd: $("#dtEnd").val()
    };

    ETrip.Util.showLoading();
    ETrip.Util.ajax({
        url: "/System/GetExceptionLogs",
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

//异常日志详情
function showExceptionLogDetail(id) {
    $("#dialog").show();
    $("#dialog").dialog({
        title: "异常日志详情",
        autoOpen: false,
        modal: true,
        width: 1000,
        height: 600,
        resizable: false,
        draggable: true,
        open: function () {
            $("#frameDialog").attr("src", '/System/ExceptionLogDetail?id=' + id);
        },
        close: function (event, ui) {
            $("#frameDialog").attr("src", "/Login/LoadingDialog");
            ETrip.Util.removeLoading("#dialog");
        }
    });
    $("#dialog").dialog("open");
}
