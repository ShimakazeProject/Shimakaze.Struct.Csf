window.$docsify = {
    loadNavbar: true,
    loadSidebar: true,
    coverpage: false,
    auto2top: true,
    maxLevel: 3,
    name: 'Shimakaze Docs',
    repo: 'frg2089/Shimakaze.Struct.Ini',
    homepage: "index.md",
    plugins: [
        function (hook, vm) {
            hook.beforeEach(function (html) {
                return html
                    + '\n\n----  \n'
                    + '`Shimakaze Docs` Powered by [`docsify`](//docsify.js.org)  \n'
                    + '`JavaScript` Build by [`TypeScript`](//www.typescriptlang.org)  \n'
                    + '----';
            }),
                hook.afterEach(function (html) {
                    if (vm.route.path === '/') {
                        return html;
                    }
                    return html;
                });
        }
    ]
};
