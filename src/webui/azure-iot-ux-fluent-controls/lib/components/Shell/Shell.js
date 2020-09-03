Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    classNames = require("classnames/bind"),
    styled_components_1 = require("styled-components"),
    Masthead_1 = require("../Masthead/Masthead"),
    Navigation_1 = require("../Navigation/Navigation"),
    root_1 = require("../ContextPanel/root");
let Navigation_2 = require("../Navigation/Navigation");
exports.NavigationItemSeparator = Navigation_2.NavigationItemSeparator;
const css = classNames.bind(require("./Shell.module.scss"));
function Shell({ theme, isRtl, masthead, navigation, children, onClick }) {
    // backward compatibility handle string format theme
    const shellTheme =
        typeof theme === "object"
            ? theme
            : {
                  base: theme,
              };
    if (shellTheme.base === undefined) {
        shellTheme.base = "light";
    }
    return React.createElement(
        "div",
        { className: css("theme-" + shellTheme.base) },
        React.createElement(
            styled_components_1.ThemeProvider,
            { theme: shellTheme },
            React.createElement(
                "div",
                { className: css("shell", { rtl: isRtl }), onClick: onClick },
                masthead &&
                    React.createElement(
                        Masthead_1.Masthead,
                        Object.assign({ navigation: navigation }, masthead)
                    ),
                React.createElement(
                    "div",
                    { className: css("nav-and-workspace") },
                    navigation &&
                        React.createElement(
                            Navigation_1.Navigation,
                            Object.assign({}, navigation)
                        ),
                    React.createElement(
                        "div",
                        { className: css("workspace") },
                        children
                    ),
                    React.createElement(root_1.Root, null)
                )
            )
        )
    );
}
exports.Shell = Shell;
exports.default = Shell;
