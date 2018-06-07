
$(function () {
    //重置分页
    ETrip.QueryUtil.initPager();

    //绑定每页显示N条记录的Select框事件
    ETrip.QueryUtil.bindPageSizeChangeEvent();

    //显示列表状态
    ETrip.QueryUtil.resetRecordsRange(parseInt($('#hdfRecordCount').val()));

    //绑定排序
    $('.btn-sort-order').bind('click', ETrip.QueryUtil.sortOrder);

    //绑定下拉框change事件
    ETrip.QueryUtil.bindChangeEvent($("#ddlIsLocked,#ddlUserType,#ddlVague,#ddlDateType,#ddlIsPartner"));

    //绑定输入框回车事件
    ETrip.QueryUtil.bindKeyEvent($("input.input-sm"));

    //绑定搜索事件
    ETrip.QueryUtil.bindSearchEvent('#btnSearch');

    query();
});

function query() {
    var requestData = {
        pageIndex: $('#hdfPageIndex').val(),
        pageSize: $('#hdfPageSize').val(),
        orderBy: $('#hdfOrderBy').val(),
        isAsc: $('#hdfIsAsc').val(),
        IsLocked: $("#ddlIsLocked").val(),
        dtStart: $("#dtStart").val(),
        dtEnd: $("#dtEnd").val(),
        Tel: $("#txtTel").val(),
        IsVague: $("#ddlVague").val(),
        UserType: $("#ddlUserType").val(),
        DateType: $("#ddlDateType").val(),
        LoginName: $("#txtLoginName").val(),
        ParentId: $("#hdfParentId").val(),
        PartnerId: $("#txtPartnerId").val(),
        IsPartner: $("#ddlIsPartner").val()
    };

    ETrip.Util.showLoading();
    ETrip.Util.ajax({
        url: "/Home/GetUsers",
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

//合作者ID
function showPartnerIdAndKey(id) {
    $("#dialog").show();
    $("#dialog").dialog({
        title: "现金网接口合作者Id和秘钥",
        autoOpen: false,
        modal: true,
        height: 260,
        width: 520,
        resizable: false,
        draggable: false,
        open: function () {
            $("#frameDialog").attr("src", "/Agent/ShowPartnerIdAndKey?id=" + id);
        },
        close: function (event, ui) {
            $("#frameDialog").attr("src", "/Login/LoadingDialog");
        }
    });
    $("#dialog").dialog("open");
}


function userExtraPeiLvTreeView(sender, id) {
    $("#dialog").show();
    $("#dialog").dialog({
        title: "查看额外打水率",
        autoOpen: false,
        modal: true,
        height: 800,
        width: 1020,
        resizable: false,
        draggable: false,
        open: function () {
            $("#frameDialog").attr("src", "/Home/UserExtraPeiLvTreeView?id=" + id);
        },
        close: function (event, ui) {
            $("#frameDialog").attr("src", "/Login/LoadingDialog");
        }
    });
    $("#dialog").dialog("open");
}