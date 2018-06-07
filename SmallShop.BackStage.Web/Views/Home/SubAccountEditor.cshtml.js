$(function () {
    $("#divAvailableTime").dialog({
        title: "有效期",
        modal: true,
        autoOpen: false,
        resizable: false,
        draggable: false
    });
});

//保存
function btnSaveClick(sender) {    
    if ($("[name='LoginName']").val() == "") {
        parent.$.sticky("帐号不能为空", { type: "st-error" });
        return false;
    }
    if ($("[name='Password']").val() == "") {
        parent.$.sticky("密码不能为空", { type: "st-error" });
        return false;
    }

    sender.disabled = true;
    ETrip.Util.ajaxSubmit("#mainForm", function (data, status) {
        sender.disabled = false;
        if (data.Success) {
            parent.$.sticky("保存子帐号成功", { type: "st-success" });
            parent.query();
            parent.$("#dialog").dialog("close");
        } else {
            parent.$.sticky(data.Message, { type: "st-error" });
        }
    });
}


