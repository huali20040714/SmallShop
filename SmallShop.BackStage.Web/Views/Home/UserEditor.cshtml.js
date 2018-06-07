$(function () {
    $("#divAvailableTime").dialog({
        title: "有效期",
        modal: true,
        autoOpen: false,
        resizable: false,
        draggable: false
    });

    $('input[name="CsType"]').change(function () {
        ChangeCsType(this.value);
    });

    $('input[name="XiMaType"').change(function () {
        ChangeXiMaType(this.value);
    });
});

function ChangeCsType(csType) {
    if (csType == "1") {
        $("#trCs").show();
        $("#trXiMaTypeExtParams").hide();
        $('#trXiMaTypeExtParams input[value="false"]').prop("checked", true);
    } else if (csType == "2") {
        $("#trCs").hide();
        $("input[name='Cs']").val(0);
        if ($("#Type").val() == "32") {
            $("#trXiMaTypeExtParams").show();
            $("input[name='XiMaLv']").val(0);
        }
    }
}

function ChangeXiMaType(xiMaType) {
    if (xiMaType == "1" || xiMaType == "2") {
        $("#trXiMaLv").show();
        $("#trXiMaTypeExtParams").hide();
        $('#trXiMaTypeExtParams input[value="false"]').prop("checked", true);
    } else if (xiMaType == "4") {
        $("#trXiMaLv").hide();
        $("input[name='XiMaLv']").val(0);
        $("#trXiMaTypeExtParams").show();
    }
}

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
    if ($("input[name='XiMaType']").length == 0) {
        parent.$.sticky("未选择洗码方式", { type: "st-error" });
        return false;
    }

    sender.disabled = true;
    ETrip.Util.ajaxSubmit("#mainForm", function (data, status) {
        sender.disabled = false;
        if (data.Success) {
            var msg = "保存代理成功";
            if ($("#Type").val() == "32") {
                msg = '保存会员成功';
            }
            parent.$.sticky(msg, { type: "st-success" });
            parent.query();
            parent.$("#dialog").dialog("close");
        } else {
            parent.$.sticky(data.Message, { type: "st-error" });
        }
    });
}

function btnCancel(sender) {
    parent.$("#dialog").dialog("close");
}
