
//保存
function btnSaveClick(sender, gameType) {
    if (gameType == 3 || gameType == 4 || gameType == 5) {
        if ($('.result0').html() == '' || ($('.result1').html() == '' && $('.result2').html() == '' && $('.result3').html() == '' && $('.result4').html() == '' && $('.result5').html() == '')) {
            parent.$.sticky('请选择开牌结果', { type: "st-error" });
            return;
        }
    }

    sender.disabled = true;
    ETrip.Util.ajaxSubmit("#mainForm", function (data, status) {
        sender.disabled = false;
        if (data.Success) {
            parent.$.sticky("修改成功", { type: "st-success" });
            parent.query();
            parent.$("#dialog").dialog("close");
        } else {
            parent.$.sticky(data.Message, { type: "st-error" });
        }
    });
}

var selectdClass = "selected";

function iniData(gameType, result) {
    if (gameType == 1 || gameType == 2) {
        if (result == 1 || result == 4 || result == 7 || result == 10) {
            $('button[data-value="1"]').addClass(selectdClass);
        }
        if (result == 2 || result == 5 || result == 8 || result == 11) {
            $('button[data-value="2"]').addClass(selectdClass);
        }
        if (result == 3 || result == 6 || result == 9 || result == 12) {
            $('button[data-value="3"]').addClass(selectdClass);
        }
        if (result == 4 || result == 5 || result == 6 || result == 10 || result == 11 || result == 12) {
            $('button[data-value="4"]').addClass(selectdClass);
        }
        if (result == 7 || result == 8 || result == 9 || result == 10 || result == 11 || result == 12) {
            $('button[data-value="5"]').addClass(selectdClass);
        }
    } else if (gameType == 3 || gameType == 4 || gameType == 5) {
        var bits = [0xf, 0xf0, 0xF00, 0xF000, 0xF0000, 0xF00000];
        for (var i = 0; i <= 6; i++) {
            var r = result & bits[i];
            $('.result' + i).html($('tr[data-row="' + i + '"] button[data-value="' + (r >> (i * 4)) + '"]').addClass(selectdClass).html());
            if (i == 0) {
                resultButtonLinkChange(gameType, r);
            }
        }
    }
}

function resultButtonLinkChange(gameType, result0) {
    if (gameType == 3) {
        for (var i = 1; i <= 5; i++) {
            $('.result' + i).html('');
            for (var j = 1; j <= 7; j++) {
                var btn = $('tr[data-row="' + i + '"] button[data-value="' + j + '"]').removeClass(selectdClass);
                //输(4),和(7)一直显示
                if (j == 1)                                                                                                //庄家结果不是平倍时隐藏
                    btn.prop('disabled', result0 != 1);
                if (j == 2 || j == 3)                                                                                      //庄家结果不是平倍，翻倍时隐藏
                    btn.prop('disabled', result0 != 1 && result0 != 2);
                if (j == 5 || j == 6)                                                                                      //庄家结果不是豹子时隐藏
                    btn.prop('disabled', result0 != 3);
            }
        }
    } else if (gameType == 4) {
        for (var i = 1; i <= 5; i++) {
            $('.result' + i).html('');
            for (var j = 1; j <= 7; j++) {
                var btn = $('tr[data-row="' + i + '"] button[data-value="' + j + '"]').removeClass(selectdClass);           //隐藏赔率比庄家小的按钮
                btn.prop('disabled', result0 > j || (j == 7 && result0 > 1));
            }
        }
    }
}

function changeResult(sender, gameType, position) {
    var result = 0;
    if (gameType == 1 || gameType == 2) {
        var selectValue = parseInt($(sender).attr('data-value'));
        if (selectValue >= 1 && selectValue <= 3) {
            $('button[data-value="1"],button[data-value="2"],button[data-value="3"]').removeClass(selectdClass);
            $(sender).addClass(selectdClass);
        } else {
            $(sender).toggleClass(selectdClass);
        }

        var result1 = $('button[data-value="1"]').hasClass(selectdClass);
        var result2 = $('button[data-value="2"]').hasClass(selectdClass);
        var result3 = $('button[data-value="3"]').hasClass(selectdClass);
        var result4 = $('button[data-value="4"]').hasClass(selectdClass);
        var result5 = $('button[data-value="5"]').hasClass(selectdClass);
        if (result1) {
            if (result4 && result5) {
                result = 10;
            } else if (result4) {
                result = 4;
            } else if (result5) {
                result = 7;
            } else {
                result = 1;
            }
        } else if (result2) {
            if (result4 && result5) {
                result = 11;
            } else if (result4) {
                result = 5;
            } else if (result5) {
                result = 8;
            } else {
                result = 2;
            }
        } else if (result3) {
            if (result4 && result5) {
                result = 12;
            } else if (result4) {
                result = 6;
            } else if (result5) {
                result = 9;
            } else {
                result = 3;
            }
        }
        $("#imgResult").attr('src', '/Assets/images/gameresult/' + (gameType == 1 ? 'baccarat' : 'longhu') + '/' + result + '.png');
    } else if (gameType == 3 || gameType == 4 || gameType == 5) {
        $(sender).toggleClass(selectdClass);
        if ($(sender).hasClass(selectdClass)) {
            $('.result' + position).html($(sender).html());
        } else {
            $('.result' + position).html('');
        }
        var forStart = gameType == 5 ? 13 : 0;
        var forEnd = gameType == 5 ? 14 : 6;
        var clickDataValue = parseInt($(sender).attr('data-value'));

        for (var i = forStart; i <= forEnd; i++) {
            if (i != clickDataValue)
                $('tr[data-row="' + position + '"] button[data-value="' + i + '"]').removeClass(selectdClass);
        }
        var result0 = parseInt($('tr[data-row="0"] button.' + selectdClass).attr("data-value"));

        if (position == 0) {
            resultButtonLinkChange(gameType, result0);
        }

        for (var i = 0; i <= 5; i++) {
            result += parseInt($('tr[data-row="' + i + '"] button.' + selectdClass).attr("data-value")) << (i * 4);
        }
    }

    $("#Result").val(result);
}