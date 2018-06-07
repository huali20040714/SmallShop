$(function () {    
    //默认帐号选中
    setTimeout(function () {
        $("#loginName").focus();
        $("#loginName").select();
    }, 200);

    //回车提交
    $("input").each(function () {
        $(this).on("keyup", function (e) {
            $(".tips.accounttips").hide();
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) {
                btnLoginClick();
            }
        });
    });

    //判断版本
    function IsSupportHtml5() {
        var isHtml5 = typeof (Worker) !== "undefined";

        return isHtml5;
    }

    if (!IsSupportHtml5()) {
        $("#dialog-html").show();
        $("#dialog-html").dialog({
            title: "温馨提示",
            autoOpen: false,
            modal: true,
            width: 460,
            height: 280,
            resizable: false,
            draggable: true,
            open: function () {
                var _html = String.format("<table width='420' height='220'><tr><td style='padding-left:38px;line-height:32px;font-size:16px;'>{0}<br/>{1}</td></tr><tr></table>",
                    "<font color='red'>您当前使用的浏览器版本过低<br />为了更好的用户体验请下载如下浏览器：</font>",
                    "<br /><a id='download' href='http://down.360safe.com/se/360se_setup.exe' target='_blank'>下载</a><br />"
                );
                $("#dialog-html").html(_html);
                $("#download").focus();
                $("#download").select();
            },
            close: function (event, ui) {
                $("#dialog-html").html("");
            }
        });
        $("#dialog-html").dialog("open");
    }

});

function btnChangeVerifyCodeClick() {
    $("#verifyCodeImage").attr("src", "/Login/VerifyCodeImage?rnd=" + Math.random());
}

function btnLoginClick() {
    if ($("#loginName").val() == '') {
        $(".tips.accounttips").show();
        $(".message").text("用户名不能为空");
        return;
    }

    if ($("#password").val() == '') {
        $(".tips.accounttips").show();
        $(".message").text("密码不能为空");
        return;
    }

    if ($("#verifyCode").val() == '') {
        $(".tips.accounttips").show();
        $(".message").text("验证码不能为空");
        return;
    }
    $("#btnLogin").prop("disabled", true);
    var requestData = {
        loginName: $("#loginName").val(),
        password: $("#password").val(),
        verifyCode: $("#verifyCode").val()
    };    
    ETrip.Util.ajax({
        url: "/Login/CheckLogin",
        data: requestData,
        async: true,
        success: function (data) {           
            if (data.Success) {
                location.href = data.Value;
            } else {
                //加载 新验证码
                btnChangeVerifyCodeClick();
                $("#btnLogin").prop("disabled", false);
                $(".message").text(data.Message);
                $(".tips.accounttips").show();
                if (data.Message == '验证码不正确') {
                    $("#verifyCode").focus();
                    $("#verifyCode").select();
                }
            }
        }
    });
}
