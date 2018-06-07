
function btnSaveClick(sender) {
    var reqData = {
        Id : $("#Id").val(),
        Password: $("#Password").val(),
        RePassword: $("#RePassword").val()
    };

    if (reqData.Password == '') {
        parent.$.sticky("密码不能为空", { type: "st-error" });
        return;
    }
    if (reqData.Password.length < 6) {
        parent.$.sticky("新密码长度不能小于6位", { type: "st-error" });
        return;
    }
    if (reqData.Password != reqData.RePassword) {
        parent.$.sticky("两次输入的密码不一致", { type: "st-error" });
        return;
    }

    sender.disabled = true;
    ETrip.Util.showLoading();       //显示遮罩
    ETrip.Util.ajax({
        url: "/Home/SavePassword",
        data: reqData,
        async: true,
        success: function (data) {
            sender.disabled = false;
            parent.ETrip.Util.removeLoading();//隐藏遮罩
            if (data.Success) {
                parent.$.sticky("修改密码成功", { type: "st-success" });
                parent.$("#dialog").dialog("close");
            } else {
                parent.$.sticky(data.Message, { type: "st-error" });
            }
        }
    });
}