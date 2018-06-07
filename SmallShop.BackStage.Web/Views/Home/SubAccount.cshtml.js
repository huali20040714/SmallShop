
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
    ETrip.QueryUtil.bindChangeEvent($("#ddlIsLocked,#ddlVague"));

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
        IsVague: $("#ddlVague").val(),
        IsLocked: $("#ddlIsLocked").val(),
        dtStart: $("#dtStart").val(),
        dtEnd: $("#dtEnd").val(),
        LoginName: $("#txtLoginName").val(),
        Tel: $("#txtTel").val()
    };

    ETrip.Util.showLoading();
    ETrip.Util.ajax({
        url: "/Home/GetSubAccounts",
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

//新增、编辑子帐号
function subAccountEditor(id) {
    $("#dialog").show();
    $("#dialog").dialog({
        title: id == 0 ? "新增子帐号" : "编辑子帐号",
        autoOpen: false,
        modal: true,
        width: 580,
        height: id == 0 ? 340 : 280,
        resizable: false,
        draggable: true,
        open: function () {
            $("#frameDialog").attr("src", "/Home/SubAccountEditor?id=" + id);
        },
        close: function (event, ui) {
            $("#frameDialog").attr("src", "/Login/LoadingDialog");
            ETrip.Util.removeLoading("#dialog");
        }
    });
    $("#dialog").dialog("open");
}

//修改密码
function subAccountChangePassword(id) {
    $("#dialog").show();
    $("#dialog").dialog({
        title: "修改子帐号密码",
        autoOpen: false,
        modal: true,
        width: 320,
        height: 220,
        resizable: false,
        draggable: true,
        open: function () {
            $("#frameDialog").attr("src", "/Home/ChangePassword?id=" + id);
        },
        close: function (event, ui) {
            $("#frameDialog").attr("src", "/Login/LoadingDialog");
            ETrip.Util.removeLoading("#dialog");
        }
    });
    $("#dialog").dialog("open");
}

//锁定
function changesubAccountLockStatus(id, isLocked) {
    ETrip.Util.showLoading();
    ETrip.Util.ajax({
        url: "/Home/ChangeUserLockStatus",
        data: {
            Id: id,
            IsLocked: isLocked
        },
        success: function (data) {
            ETrip.Util.removeLoading();
            if (data.Success) {
                $.sticky("操作成功", { type: "st-success" });
                query();
            } else {
                $.sticky(data.Message, { type: "st-error" });
            }
        }
    });
}