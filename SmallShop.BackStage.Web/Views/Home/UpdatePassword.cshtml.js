
function btnSaveClick(sender) {
    var reqData = {
        OldPassword: $("#OldPassword").val(),
        NewPassword: $("#NewPassword").val(),
        RePassword: $("#RePassword").val()
    };
    
    if (reqData.OldPassword == '') {
        $.sticky("原始密码不能为空", { type: "st-error" });
        return;
    }
    if (reqData.NewPassword == '') {
        $.sticky("新密码不能为空", { type: "st-error" });
        return;
    }
    if (reqData.NewPassword.length < 6) {
        $.sticky("新密码长度不能小于6位", { type: "st-error" });
        return;
    }
    if (reqData.NewPassword != reqData.RePassword) {
        $.sticky("两次输入的密码不一致", { type: "st-error" });
        return;
    }

    sender.disabled = true;
    ETrip.Util.showLoading();       //显示遮罩
    ETrip.Util.ajax({
        url: "/Home/SaveUpdatePassword",
        data: reqData,
        async: true,
        success: function (data) {
            sender.disabled = false;
            ETrip.Util.removeLoading();//隐藏遮罩
            if (data.Success) {
                $.sticky("修改密码成功", { type: "st-success" });
            } else {
                $.sticky(data.Message, { type: "st-error" });
            }
        }
    });    
}