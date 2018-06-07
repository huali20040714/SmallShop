
//保存
function btnSaveClick(sender) {
    if ($("[name='Name']").val() == "") {
        parent.$.sticky("名称不能为空", { type: "st-error" });
        return false;
    }

    sender.disabled = true;
    ETrip.Util.ajaxSubmit("#mainForm", function (data, status) {
        sender.disabled = false;
        if (data.Success) {
            parent.$.sticky("保存角色成功", { type: "st-success" });
            parent.query();
            parent.$("#dialog").dialog("close");
        } else {
            parent.$.sticky(data.Message, { type: "st-error" });
        }
    });
}
