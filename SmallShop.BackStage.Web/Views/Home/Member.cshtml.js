
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
    ETrip.QueryUtil.bindChangeEvent($("#ddlIsLocked,#ddlDateType,#ddlVague"));

    //绑定输入框回车事件
    ETrip.QueryUtil.bindKeyEvent($("input.input-sm"));

    //绑定搜索事件
    ETrip.QueryUtil.bindSearchEvent('#btnSearch');

    ChangeButtonAddStatus();

    query();    
});

function ChangeButtonAddStatus() {
    //当前登录帐号对应的代理
    //管理员以及管理子帐号不能直接添加会员
    if ($("#hdfRootId").val() == $("#hdfParentId").val() && $("#hdfParentId").val() != "1001") {
        $("#btn_add").removeClass("disabled");
    } else {
        $("#btn_add").addClass("disabled");
    }
}

function query() {
    var requestData = {
        ParentId: $("#hdfParentId").val(),
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
        url: "/Home/GetMembers",
        data: requestData,
        success: function (data) {
            ETrip.Util.removeLoading();
            if (data.Success) {
                $(".content1").html(data.Value.HeadPartial);
                $(".content2").html(data.Value.BodyPartial);
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
        },
        complete: function () {
            ChangeButtonAddStatus();
        }
    });
}

//新增、编辑用户
function memberEditor(sender, id, type) {
    if ($(sender).hasClass("disabled"))
        return;

    $("#dialog").show();
    $("#dialog").dialog({
        title: id == 0 ? "新增会员" : "编辑会员",
        autoOpen: false,
        modal: true,
        width: 720,
        height: id == 0 ? 640 : 560,
        resizable: false,
        draggable: true,
        open: function () {
            $("#frameDialog").attr("src", "/Home/UserEditor?id=" + id + "&userType=" + type);
        },
        close: function (event, ui) {
            $("#frameDialog").attr("src", "/Login/LoadingDialog");
            ETrip.Util.removeLoading("#dialog");
        }
    });
    $("#dialog").dialog("open");
}