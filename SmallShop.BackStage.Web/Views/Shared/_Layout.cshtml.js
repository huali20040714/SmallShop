$(function () {

    //启动心跳包-------------------------002----------------------
    function keepAlive() {
        try {
            ETrip.Util.ajax({
                url: "/Login/KeepAlive",
                data: {},
                success: function (data) { }
            });
        } catch (e) { }
        setTimeout(function () { keepAlive(); }, 1000 * 20);
    }
    keepAlive();
    
    //模态提示信息----------------------007----------------------
    $('#dialogAlertMessage').dialog({
        autoOpen: false,
        width: 300,
        modal: true,
        buttons: {
            "取消": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConfirmMessage').dialog({
        autoOpen: false,
        width: 300,
        modal: true,
        buttons: {
            "取消": function () {
                $(this).dialog('close');
                mDialogCallback(false);
            },
            "确定": function () {
                $(this).dialog('close');
                mDialogCallback(true);
            }
        }
    });
});

//加载完面包屑调用，设置当前选中菜单
function initBreadcrumb(breadcrumbPath) {
    $('.breadcrumbs .breadcrumb li:last').addClass('active');

    var breadcrumbs = breadcrumbPath.split('/');
    var idx = breadcrumbs.length - 1;
    for (var i = idx; i >= 0; i--) {
        var curMenu = breadcrumbs[i];
        var $this = $('[menu="' + curMenu + '"]');
        if ($this.length > 0) {
            $this.addClass('active');
            var $parent = $this.parents("li");
            $parent && $parent.addClass("active").addClass("open");
            break;
        }
    }
}

//打开对话框
function openAlertDialog(_title, _content, _width, _height) {
    $("#dialog-html").show();
    $("#dialog-html").dialog({
        title: _title,
        autoOpen: false,
        modal: true,
        width: _width || 560,
        height: _height || 380,
        resizable: false,
        draggable: true,
        open: function () {
            var _html = String.format("<div id='ly-dialog-conent' style='width:100%; height:100%;padding:5px;'>{0}</div>", _content);
            $("#dialog-html").html(_html);
            //$("#ly-dialog-conent").niceScroll({ styler: "fb", cursorcolor: "#65cea7", cursorwidth: '4', cursorborderradius: '0px', background: '#424f63', scrollspeed: 100, spacebarenabled: false, cursorborder: '0', zindex: '9999' });
        },
        close: function (event, ui) {
            $("#dialog-html").html("");
        }
    });
    $("#dialog-html").dialog("open");
}

//【自定义】模态提示信息
var mDialogCallback;
function openShowMsg(msg, callback) {
    if (callback == null) {
        $('#AlertMessageBody').html(msg);
        $('#dialogAlertMessage').dialog('open');
    } else {
        mDialogCallback = callback;
        $('#ConfirmMessageBody').html(msg);
        $('#dialogConfirmMessage').dialog('open');
    }
};

// div resize event
(function ($, h, c) {
    var a = $([]),
        e = $.resize = $.extend($.resize, {}),
        i,
        k = "setTimeout",
        j = "resize",
        d = j + "-special-event",
        b = "delay",
        f = "throttleWindow";
    e[b] = 250;
    e[f] = true;
    $.event.special[j] = {
        setup: function () {
            if (!e[f] && this[k]) {
                return false
            }
            var l = $(this);
            a = a.add(l);
            $.data(this, d, {
                w: l.width(),
                h: l.height()
            });
            if (a.length === 1) {
                g()
            }
        },
        teardown: function () {
            if (!e[f] && this[k]) {
                return false
            }
            var l = $(this);
            a = a.not(l);
            l.removeData(d);
            if (!a.length) {
                clearTimeout(i)
            }
        },
        add: function (l) {
            if (!e[f] && this[k]) {
                return false
            }
            var n;
            function m(s, o, p) {
                var q = $(this),
                    r = $.data(this, d);
                r.w = o !== c ? o : q.width();
                r.h = p !== c ? p : q.height();
                n.apply(this, arguments)
            }
            if ($.isFunction(l)) {
                n = l;
                return m;
            } else {
                n = l.handler;
                l.handler = m;
            }
        }
    };
    function g() {
        i = h[k](function () {
            a.each(function () {
                var n = $(this),
                    m = n.width(),
                    l = n.height(),
                    o = $.data(this, d);
                if (m !== o.w || l !== o.h) {
                    n.trigger(j, [o.w = m, o.h = l])
                }
            });
            g()
        },
            e[b])
    }
})(jQuery, this);

//打开代理列表目录树
function openJstreeAgent() {
    if ($('#jstreeFrameDialog').attr('src') != undefined) {
        $('#dialog-agentjstree').show();
        $('#dialog-agentjstree').dialog("open");
    } else {
        $('#dialog-agentjstree').show();
        $('#dialog-agentjstree').dialog({
            title: '代理列表',
            autoOpen: false,
            modal: true,
            width: 560,
            height: 380,
            resizable: false,
            draggable: false,
            open: function () {
                $("#jstreeFrameDialog").attr("src", "/Home/JstreeAgent");
            }
        });
        $('#dialog-agentjstree').dialog("open");
    }
}

//自动输出执行状态对话框
function autoResponseWriteDialog(title, src) {
    $("#dialog").show();
    $("#dialog").dialog({
        title: title,
        autoOpen: false,
        modal: true,
        width: 1000,
        height: 680,
        resizable: false,
        draggable: true,
        open: function () {
            $("#frameDialog").attr("src", src);
        },
        close: function (event, ui) {
            $("#frameDialog").attr("src", "/Login/LoadingDialog");
            ETrip.Util.removeLoading("#dialog");
        }
    });
    $("#dialog").dialog("open");

}

//结算洗码（代理列表、会员列表中使用到）
function jieSuanXimaDialog(id) {
    $("#dialog").show();
    $("#dialog").dialog({
        title: "结算洗码",
        autoOpen: false,
        modal: true,
        width: 450,
        height: 540,
        resizable: false,
        draggable: true,
        open: function () {
            $("#frameDialog").attr("src", "/Home/JieSuanXiMaEditor?id=" + id);
        },
        close: function (event, ui) {
            $("#frameDialog").attr("src", "/Login/LoadingDialog");
            ETrip.Util.removeLoading("#dialog");
        }
    });
    $("#dialog").dialog("open");
}

//修改密码（代理列表、会员列表中使用到）
function changePassword(id) {
    $("#dialog").show();
    $("#dialog").dialog({
        title: "修改帐号密码",
        autoOpen: false,
        modal: true,
        width: 320,
        height: 220,
        resizable: false,
        draggable: true,
        open: function () {
            $("#frameDialog").attr("src", "/Home/ChangePassword?id=" + id);
        },
        close: function (event, ui) {
            $("#frameDialog").attr("src", "/Login/LoadingDialog");
            ETrip.Util.removeLoading("#dialog");
        }
    });
    $("#dialog").dialog("open");
}

//锁定（代理列表、会员列表、用户列表中使用到）
function changeUserLockStatus(id, isLocked) {
    ETrip.Util.showLoading();
    ETrip.Util.ajax({
        url: "/Home/ChangeUserLockStatus",
        data: {
            Id: id,
            IsLocked: isLocked
        },
        success: function (data) {
            ETrip.Util.removeLoading();
            if (data.Success) {
                $.sticky("操作成功", { type: "st-success" });
                query();
            } else {
                $.sticky(data.Message, { type: "st-error" });
            }
        }
    });
}

//限红设置（代理列表、会员列表中使用到）
function userLimitMoneyEditor(id) {
    $("#dialog").show();
    $("#dialog").dialog({
        title: "帐号限红设置",
        autoOpen: false,
        modal: true,
        width: 580,
        height: 640,
        resizable: false,
        draggable: true,
        open: function () {
            $("#frameDialog").attr("src", "/Home/UserLimitMoneyEditor?id=" + id);
        },
        close: function (event, ui) {
            $("#frameDialog").attr("src", "/Login/LoadingDialog");
            ETrip.Util.removeLoading("#dialog");
        }
    });
    $("#dialog").dialog("open");
}

//额外打水率编辑（代理列表、会员列表、用户管理列表中使用到）
function userExtraPeiLvEditor(sender, id) {
    if ($(sender).hasClass("disabled"))
        return;

    $("#dialog").show();
    $("#dialog").dialog({
        title: "编辑额外打水率",
        autoOpen: false,
        modal: true,
        width: 560,
        height: 380,
        resizable: false,
        draggable: true,
        open: function () {
            $("#frameDialog").attr("src", "/Home/UserExtraPeiLvEditor?id=" + id);
        },
        close: function (event, ui) {
            $("#frameDialog").attr("src", "/Login/LoadingDialog");
            ETrip.Util.removeLoading("#dialog");
        }
    });
    $("#dialog").dialog("open");
}

//充值（代理列表、会员列表中使用到）
function userDepositAmount(id) {
    $("#dialog").show();
    $("#dialog").dialog({
        title: "帐号充值",
        autoOpen: false,
        modal: true,
        width: 420,
        height: 400,
        resizable: false,
        draggable: true,
        open: function () {
            $("#frameDialog").attr("src", "/Home/UserDepositAmountEditor?id=" + id);
        },
        close: function (event, ui) {
            $("#frameDialog").attr("src", "/Login/LoadingDialog");
            ETrip.Util.removeLoading("#dialog");
        }
    });
    $("#dialog").dialog("open");
}

//提现（代理列表、会员列表中使用到）
function userWithDrawAmount(id) {
    $("#dialog").show();
    $("#dialog").dialog({
        title: "帐号提现",
        autoOpen: false,
        modal: true,
        width: 420,
        height: 400,
        resizable: false,
        draggable: true,
        open: function () {
            $("#frameDialog").attr("src", "/Home/UserWithDrawAmountEditor?id=" + id);
        },
        close: function (event, ui) {
            $("#frameDialog").attr("src", "/Login/LoadingDialog");
            ETrip.Util.removeLoading("#dialog");
        }
    });
    $("#dialog").dialog("open");
}

//特殊结算
function setSpecialGameResult(gameDeskId, chang, ci) {
    if (!confirm("确认特殊结算游戏？")) {
        return false;
    }

    ETrip.Util.showLoading();
    ETrip.Util.ajax({
        url: "/Home/SetSpecialGameResult",
        data: {
            GameDeskId: gameDeskId,
            Chang: chang,
            Ci: ci,
        },
        success: function (data) {
            ETrip.Util.removeLoading();
            if (data.Success) {
                $.sticky("结算成功", { type: "st-success" });
                query();
            } else {
                $.sticky(data.Message, { type: "st-error" });
            }
        }
    });
}