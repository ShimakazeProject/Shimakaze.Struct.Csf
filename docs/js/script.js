// 去他的 JavaScript, 那是给人看的东西嘛!?
// Cookie管理
var Cookie = /** @class */ (function () {
    function Cookie() {
    }
    // 获取cookie
    Cookie.get = function (key) {
        key = key + "=";
        var cookieArray = document.cookie.split(';');
        for (var i = 0; i < cookieArray.length; i++) {
            var cookie = cookieArray[i].trim();
            if (cookie.indexOf(key) == 0)
                return cookie.substring(key.length, cookie.length);
        }
        return "";
    };
    // 写入cookie
    Cookie.set = function (key, value, exdays) {
        if (exdays === void 0) { exdays = 30; }
        var date = new Date();
        date.setTime(date.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + date.toUTCString();
        document.cookie = key + "=" + value + "; " + expires;
    };
    return Cookie;
}());
// 主题管理
var Theme = /** @class */ (function () {
    function Theme() {
    }
    // 从cookie中读取主题
    Theme.loadThemeFromCookie = function () {
        var theme = Cookie.get("theme"); // 获取cookie中主题相关内容
        if (theme == "" || theme == null) { // 找不到主题设置
            Cookie.set("theme", "themeable-dark"); // 添加主题到cookie
            Theme.change("themeable-dark"); // 设置主题
        }
        else if (theme != "")
            Theme.change(theme); // 从cookie中设置主题
    };
    // 修改主题
    Theme.change = function (theme) {
        var links = document.getElementsByTagName("link"); // 获取全部link标签
        for (var i = 0; i < links.length; i++) {
            var link = links[i]; // 遍历link标签
            if (link.className == "theme") { // 是theme类标签
                if (link.title == theme) // 是目标主题
                    link.disabled = false; // 解除禁用
                else
                    link.disabled = true; // 否则设置禁用
            }
        }
        Theme.instance = theme; // 修改当前实例的主题名
    };
    return Theme;
}());
// 加载主题
Theme.loadThemeFromCookie();
