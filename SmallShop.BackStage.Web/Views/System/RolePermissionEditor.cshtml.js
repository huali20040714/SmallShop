
$(function () {
    $("#RoleId").on("change", function () {
        roleIdChange(this.value);
    });

    roleIdChange($("#RoleId").val());
});

function roleIdChange(roleId) {

    var requestData = {
        RoleId: roleId
    };

    ETrip.Util.ajax({
        url: "/System/BuildRolePermission",
        data: requestData,
        success: function (data) {
            if (data.Success) {
                $("#permissionPanel").html(data.Value);

                bindCheckBoxEvent();
            } else {
                parent.$.sticky(data.Message, { type: "st-error" });
            }
        }
    });
}

// 动态版定复选框事件
function bindCheckBoxEvent() {
    $(".tabbable input[type='checkbox']").on("click", function () {
        var isGroup = $(this).attr('data-tab') == undefined;
        if (isGroup) {
            var group = $(this).val();
            var isChecked = $(this).is(":checked");
            $(".tabbable .tab-content input[data-tab='" + group + "']").prop("checked", isChecked);
        } else {
            var group = $(this).attr("data-tab");
            var hasChecked = $(".tabbable .tab-content input[data-tab='" + group + "']:checked").length > 0;
            $(".tabbable .nav-tabs input[name='c" + group + "']").prop("checked", hasChecked);
        }
    });
}

//保存
function btnSaveClick(sender) {
    var permissions = "|";
    $("input[data-tab]").each(function () {
        if ($(this).is(":checked")) {
            //如果未包含组，加上组
            var tab = $(this).attr('data-tab') + '|';
            permissions.contains('|' + tab) || (permissions += tab);
            permissions += $(this).val() + "|";
        }
    });

    var requestData = {
        RoleId: $("#RoleId").val(),
        Permissions: permissions
    };
    sender.disabled = true;
    ETrip.Util.ajax({
        url: "/System/SaveRolePermission",
        data: requestData,
        success: function (data) {
            sender.disabled = false;
            if (data.Success) {
                $.sticky("保存成功！", { type: "st-success" });
                roleIdChange(requestData.RoleId);
            } else {
                $.sticky(data.Message, { type: "st-error" });
            }
        }
    });
}
