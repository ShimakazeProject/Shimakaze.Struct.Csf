// 去他的 JavaScript, 那是给人看的东西嘛!?
// Cookie管理
abstract class Cookie {
  // 获取cookie
  static get(key: string): string {
    key = key + "=";
    let cookieArray = document.cookie.split(';');
    for (let i = 0; i < cookieArray.length; i++) {
      let cookie = cookieArray[i].trim();
      if (cookie.indexOf(key) == 0) return cookie.substring(key.length, cookie.length);
    }
    return "";
  }
  // 写入cookie
  static set(key: string, value: string, exdays = 30): void {
    let date = new Date();
    date.setTime(date.getTime() + (exdays * 24 * 60 * 60 * 1000));
    let expires = "expires=" + date.toUTCString();
    document.cookie = key + "=" + value + "; " + expires;
  }
}
// 主题管理
abstract class Theme {
  static instance: string;// 表示当前主题的字段  
  // 从cookie中读取主题
  static loadThemeFromCookie(): void {
    let theme = Cookie.get("theme");// 获取cookie中主题相关内容
    if (theme == "" || theme == null) {// 找不到主题设置
      Cookie.set("theme", "themeable-dark");// 添加主题到cookie
      Theme.change("themeable-dark");// 设置主题
    } else if (theme != "") Theme.change(theme);// 从cookie中设置主题
  }
  // 修改主题
  static change(theme: string): void {
    let links = document.getElementsByTagName("link");// 获取全部link标签
    for (let i = 0; i < links.length; i++) {
      let link = links[i];// 遍历link标签
      if (link.className == "theme") {// 是theme类标签
        if (link.title == theme)// 是目标主题
          link.disabled = false;// 解除禁用
        else link.disabled = true;// 否则设置禁用
      }
    }
    Theme.instance = theme;// 修改当前实例的主题名
  }
}
// 加载主题
Theme.loadThemeFromCookie();