//不能被嵌套在Iframe中
if (top.location.href != location.href) {
    top.location.href = location.href;
}

function getBrowserInfo() {
    var agent = navigator.userAgent.toLowerCase();
    var regStr_ie = /msie [\d.]+;/gi;
    var regStr_ff = /firefox\/[\d.]+/gi
    var regStr_chrome = /chrome\/[\d.]+/gi;
    var regStr_saf = /safari\/[\d.]+/gi;

    //IE
    if (agent.indexOf("msie") > 0) {
        return agent.match(regStr_ie);
    }

    //firefox
    if (agent.indexOf("firefox") > 0) {
        return agent.match(regStr_ff);
    }

    //Chrome
    if (agent.indexOf("chrome") > 0) {
        return agent.match(regStr_chrome);
    }

    //Safari
    if (agent.indexOf("safari") > 0 && agent.indexOf("chrome") < 0) {
        return agent.match(regStr_saf);
    }
}

try {
    var browser = getBrowserInfo();
    if (browser != undefined && browser.length > 0) {
        browser += "";
        var verinfo = (browser).replace(/[^0-9.]/ig, "");
        if (browser.indexOf("msie") >= 0 && parseFloat(verinfo) < 10) {
            top.location.href = "/Login/LowerBrowser";
        }
    }
} catch (e) {
    alert(e);
}
