//#region 公共通用工具

//去除空格
String.prototype.trim = function () { return this.replace(/(^\s*)|(\s*$)/g, ""); };
String.prototype.lTrim = function () { return this.replace(/(^\s*)/g, ""); };
String.prototype.rTrim = function () { return this.replace(/(\s*$)/g, ""); };
String.prototype.insert = function (text, at) {
    if (at == null || at > this.length)
        at = this.length;
    else if (at < 0)
        at = 0;
    return this.substring(0, at) + text + this.substring(at);
}
String.prototype.contains = function (text) {
    if (text == '') return true;
    else if (text == null) return false;
    else return this.indexOf(text) !== -1;
}
String.prototype.clear = function () {
    return this.replace(/^\s+|\s+$/g, '').replace(/\s+/g, ' ');
}
String.prototype.replaceAll = function (reallyDo, replaceWith, ignoreCase) {
    if (!RegExp.prototype.isPrototypeOf(reallyDo)) {
        return this.replace(new RegExp(reallyDo, (ignoreCase ? "gi" : "g")), replaceWith);
    } else {
        return this.replace(reallyDo, replaceWith);
    }
}

//实现String.format;
String.format = function () {

    if (arguments.length == 0)
        return null;

    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }

    return str;
};

Date.prototype.format = function (format) {
    if (!format) {
        format = "yyyy-MM-dd hh:mm:ss";
    }

    var o = {
        "M+": this.getMonth() + 1, // month
        "d+": this.getDate(), // day
        "h+": this.getHours(), // hour
        "m+": this.getMinutes(), // minute
        "s+": this.getSeconds(), // second
        "q+": Math.floor((this.getMonth() + 3) / 3), // quarter
        "S": this.getMilliseconds()
        // millisecond
    };

    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }

    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
};

//公用静态类。
var ETrip = {};
ETrip.Util = {};

//封装一般的ajax请求
ETrip.Util.ajax = function (para) {
    para = $.extend({ type: 'post' }, para);
    ETrip.Util.ajaxSubmit(null, para);
};

ETrip.Util.ajaxSubmit = function (form, params) {
    var objParams = {
        dataType: "json",
        beforeSend: function (XMLHttpRequest) {
            XMLHttpRequest.setRequestHeader("isAjax", "true");
        }
    };
    if (typeof params == 'function')
        params = { success: params };
    else if (typeof params != 'object')
        params = { success: params };

    if (!params.success)
        params.success = function () { }

    objParams = $.extend(objParams, params, {
        success: function (d, s, jqXHR) {
            if (d.HasException == true && d.Value == "AjaxRequestLoginTimeout") {
                //alert("登录超时，请重新登录");
                window.top.location.href = '/Login/Login?action=LoginOut';
            } else {
                params.success.apply(this, [d, s, jqXHR]);
            }
        }
    });
    if (form == null)
        $.ajax(objParams);
    else
        $(form).ajaxSubmit(objParams);
}

ETrip.Util.startsWith = function (str, prefix, start, end) {
    if (arguments.length < 2) {
        throw new TypeError('ETrip.Util.startsWith() requires at least 2 arguments');
    }

    // check if start and end are null/undefined or a 'number'
    if ((start == null) || (isNaN(new Number(start)))) {
        start = 0;
    }
    if ((end == null) || (isNaN(new Number(end)))) {
        end = Number.MAX_VALUE;
    }

    // if it's an array
    if (typeof prefix == "object") {
        for (var i = 0, j = prefix.length; i < j; i++) {
            var res = ETrip.Util._stringTailMatch(str, prefix[i], start, end, true);
            if (res) {
                return true;
            }
        }
        return false;
    }

    return ETrip.Util._stringTailMatch(str, prefix, start, end, true);
};

ETrip.Util.endsWith = function (str, suffix, start, end) {
    if (arguments.length < 2) {
        throw new TypeError('ETrip.Util.endsWith() requires at least 2 arguments');
    }

    // check if start and end are null/undefined or a 'number'
    if ((start == null) || (isNaN(new Number(start)))) {
        start = 0;
    }
    if ((end == null) || (isNaN(new Number(end)))) {
        end = Number.MAX_VALUE;
    }

    // if it's an array
    if (typeof suffix == "object") {
        for (var i = 0, j = suffix.length; i < j; i++) {
            var res = ETrip.Util._stringTailMatch(str, suffix[i], start, end, false);
            if (res) {
                return true;
            }
        }
        return false;
    }

    return ETrip.Util._stringTailMatch(str, suffix, start, end, false);
};

ETrip.Util.loadCss = function (cssFile) {
    ///	<summary>
    ///	  动态加载CSS文件
    ///	</summary>
    var head = document.getElementsByTagName('HEAD').item(0);
    var style = document.createElement('link');
    style.href = cssFile;
    style.rel = 'stylesheet';
    style.type = 'text/css';
    head.appendChild(style);
};

/**
* 显示Loading遮罩
*/
ETrip.Util.showLoading = function (maskArea) {
    if (maskArea != undefined) {
        $(maskArea).mask("loading");
    } else {
        $("html").mask("loading");
    }
};

/**
* 隐藏Loading遮罩
*/
ETrip.Util.removeLoading = function (maskArea) {
    if (maskArea != undefined) {
        $(maskArea).unmask();
    } else {
        $("html").unmask();
    }
}


/**
* 查询相关功能辅助
*/
ETrip.QueryUtil = {
    lastKeywordPressTime: new Date(),

    /**
    * 初始化分页
    */
    initPager: function () {
        var pageIndex = $("#hdfPageIndex").val();
        var pageSize = $("#hdfPageSize").val();
        var recordCount = $("#hdfRecordCount").val();
        $(".pager").pager({
            pageIndex: pageIndex,
            pageSize: pageSize,
            recordCount: recordCount,
            pageIndexChanged: function (pageIndex, pageSize) {
                $("#hdfPageIndex").val(pageIndex);
                $("#hdfPageSize").val(pageSize);

                query();
            }
        });
    },

    /**
    * 显示列表数据状态
    */
    resetRecordsRange: function (recordCount) {
        var pageRange = $("#hdfPageIndex").val() * $("#hdfPageSize").val();

        if (pageRange > recordCount)
            pageRange = recordCount;

        var rangeStart = ($("#hdfPageIndex").val() - 1) * $("#hdfPageSize").val() + 1;
        if (parseInt(rangeStart) <= 0)
            rangeStart = 1;
        $('.pagerRemark').text(String.format('共{0}条记录', recordCount, rangeStart, pageRange));
    },

    /**
    * 绑定每页显示N条记录的Select框事件
    */
    bindPageSizeChangeEvent: function () {
        $("#pageSizeChange").change(function () {
            $('#hdfPageIndex').val('1');
            $("#hdfPageSize").val($(this).val());
            query();
        });
    },

    /**
    * 排序
    */
    sortOrder: function () {
        var orderBy = $(this).attr('sort-expression');
        var existingOrderBy = $('#hdfOrderBy').val();
        var isAsc = 'True';
        if (orderBy == existingOrderBy) {
            if ($('#hdfIsAsc').val() == 'False')
                isAsc = 'True';
            else
                isAsc = 'False';
        }

        //重置查询参数
        $('#hdfPageIndex').val('1');
        $('#hdfOrderBy').val(orderBy);
        $('#hdfIsAsc').val(isAsc);

        query();
    },

    updateSortStatus: function () {
        $('.sort-up').removeClass('active');
        $('.sort-down').removeClass('active');
        var orderBy = $('#hdfOrderBy').val();
        var isAsc = $('#hdfIsAsc').val();

        if (isAsc == 'True')
            $('a[sort-expression="' + orderBy + '"]').parent().find('.sort-up').addClass('active');
        else
            $('a[sort-expression="' + orderBy + '"]').parent().find('.sort-down').addClass('active');
    },

    /**
    * 绑定搜索框事件
    */
    bindSearchEvent: function (btnSearch) {
        $(btnSearch).bind('click', function () {
            $("#hdfPageIndex").val("1");
            if (typeof (query) == 'function')
                query();
        });
    },

    /**
    * 绑定输入框回车事件
    */
    bindKeyEvent: function (jqObj) {
        jqObj && jqObj.on("keyup", function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if ((code == 13) && (typeof (query) == 'function')) {
                $("#hdfPageIndex").val("1");
                query();
            }
        });
    },

    /**
    * 绑定下拉框change事件
    */
    bindChangeEvent: function (jqObj) {
        jqObj && jqObj.on("change", function () {
            if (typeof (query) == 'function') {
                $("#hdfPageIndex").val("1");
                query();
            }
        });
    }
};

ETrip.Cookies = {};
ETrip.Cookies.setCookie = function (name, value, time) {
    var Days = 30;
    var exp = new Date();
    var t;
    if (time) {
        t = exp.getTime() + time;
    }
    else {
        t = exp.getTime() + Days * 24 * 60 * 60 * 1000;
    }
    exp.setTime(t);
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
};
ETrip.Cookies.getCookie = function (name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg))
        return (arr[2]);
    else
        return null;
};
ETrip.Cookies.delCookie = function (name) {
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = ETrip.Cookies.getCookie(name);
    if (cval != null)
        document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
};

ETrip.Util.getQueryString = function (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return decodeURI(r[2]); return null;
};

//#endregion

//#region start 兼容PlaceHolder

$(function () {
    var doc = document,
        inputs = doc.getElementsByTagName('input'),
        supportPlaceholder = 'placeholder' in doc.createElement('input'),
        placeholder = function (input) {
            var text = input.getAttribute('placeholder'),
                defaultValue = input.defaultValue;
            if (defaultValue == '') {
                input.value = text
            }
            input.onfocus = function () {
                if (input.value === text) {
                    this.value = ''
                }
            };
            input.onblur = function () {
                if (input.value === '') {
                    this.value = text
                }
            }
        };
    if (!supportPlaceholder) {
        for (var i = 0, len = inputs.length; i < len; i++) {
            var input = inputs[i],
                text = input.getAttribute('placeholder');
            if (input.type === 'text' && text) {
                placeholder(input)
            }
        }
    }

});

//#endregion

//#region 本项目相关功能

function parseDecimal(num) {
    var _num = Math.round((parseFloat(num) || 0) * 10000) / 10000;
    return _num;
}

/**
* 格式化jquery dom对象val中的值、格式化为应许最小数位数decimalDigits
*  $element 需要格式的dom对象
*  decimalDigits允许的最大小数位数
*  allowMinus是否应许为负号
* 格式化后的值设置回dom对象val中
*/
function formatInputElementDecimalString($element, decimalDigits, allowMinus) {
    var val = $element.val();
    if ((val || '').length > 0) {
        decimalDigits = parseInt(decimalDigits);
        if (isNaN(decimalDigits))
            decimalDigits = 4;

        var regexp = new RegExp((allowMinus ? "-{0,1}" : "") + "\\d*\\.?\\d{0," + decimalDigits + "}");
        var match = val.match(regexp);
        if (match != null) {
            val = match[0];
        } else {
            val = "";
        }
        $element.val(val);
    }
}

//自动刷新
ETrip.Util.Reflesh = {};
ETrip.Util.Reflesh.autoExecuteTimeoutHandle = 0;
// callback 为时间到了之后需要执行的方法，
// timeout 单位为毫秒数
ETrip.Util.Reflesh.autoExecute = function (callback, timeout) {
    timeout = timeout || 5 * 1000;
    ETrip.Util.Reflesh.autoExecuteTimeoutHandle && clearTimeout(ETrip.Util.Reflesh.autoExecuteTimeoutHandle);
    ETrip.Util.Reflesh.autoExecuteTimeoutHandle = setTimeout(function () {
        if ($("#chkAutoRefresh").length == 1 && $("#chkAutoRefresh").is(":checked"))
            callback && callback();
        else
            ETrip.Util.Reflesh.autoExecute(callback, timeout);
    }, timeout);
}

//#endregion