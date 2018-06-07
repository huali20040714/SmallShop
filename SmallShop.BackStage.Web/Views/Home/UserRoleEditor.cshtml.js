
$(function () {
    $("#mainForm input[type='checkbox'].RoleId-all").on("click", function () {    
        $("#mainForm :checkbox").prop("checked", this.checked);        
    });
});

//保存
function btnSaveClick(sender) {
    sender.disabled = true;
    ETrip.Util.ajaxSubmit("#mainForm", function (data, status) {
        sender.disabled = false;
        if (data.Success) {
            parent.$.sticky("用户角色设置成功", { type: "st-success" });
            parent.$("#dialog").dialog("close");
        } else {
            parent.$.sticky(data.Message, { type: "st-error" });
        }
    });
}
