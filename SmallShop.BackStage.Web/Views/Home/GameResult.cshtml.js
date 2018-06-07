
$(function () {
    //重置分页
    ETrip.QueryUtil.initPager();

    //绑定每页显示N条记录的Select框事件
    ETrip.QueryUtil.bindPageSizeChangeEvent();

    //显示列表状态
    ETrip.QueryUtil.resetRecordsRange(parseInt($('#hdfRecordCount').val()));

    //绑定排序
    $('.btn-sort-order').bind('click', ETrip.QueryUtil.sortOrder);

    //绑定输入框回车事件
    ETrip.QueryUtil.bindKeyEvent($("input.input-sm"));

    //绑定下拉框change事件
    ETrip.QueryUtil.bindChangeEvent($("#ddlGameType,#ddlVague"));

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
        GameType: $("#ddlGameType").val(),
        dtStart: $("#dtStart").val(),
        dtEnd: $("#dtEnd").val(),
        GameDeskName: $("#GameDeskName").val(),
        Chang: $("#Chang").val(),
        Ci: $("#Ci").val()
    };

    ETrip.Util.showLoading();
    ETrip.Util.ajax({
        url: "/Home/GetGameResults",
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

//游戏结果修改
function gameResultEditor(sender, id, gameType, chang, ci) {
    var width = 200;
    var height = 300;
    var title = "";
    if (gameType == 1) {
        title = "修改百家乐" + chang + "场" + ci + "次结果";
        width = 400;
        height = 300;
    } else if (gameType == 2) {
        title = "修改龙虎" + chang + "场" + ci + "次结果";
        width = 400;
        height = 300;
    } else if (gameType == 3) {
        title = "修改推筒子" + chang + "场" + ci + "次结果";
        width = 900;
        height = 600;
    } else if (gameType == 4) {
        title = "修改牛牛" + chang + "场" + ci + "次结果";
        width = 900;
        height = 600;
    } else if (gameType == 5) {
        title = "修改三公" + chang + "场" + ci + "次结果";
        width = 1300;
        height = 600;
    }

    $("#dialog").show();
    $("#dialog").dialog({
        title: title,
        autoOpen: false,
        modal: true,
        width: width,
        height: height,
        resizable: false,
        draggable: true,
        open: function () {
            $("#frameDialog").attr("src", "/Home/GameResultEditor?id=" + id);
        },
        close: function (event, ui) {
            $("#frameDialog").attr("src", "/Login/LoadingDialog");
            ETrip.Util.removeLoading("#dialog");
        }
    });
    $("#dialog").dialog("open");
}

