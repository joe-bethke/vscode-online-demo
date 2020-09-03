Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    Attributes_1 = require("../../Attributes"),
    classNames = require("classnames/bind"),
    cx = classNames.bind(require("./Navigation.module.scss"));
function Navigation({ isExpanded, onClick, attr, children }) {
    return React.createElement(
        Attributes_1.Elements.nav,
        {
            className: cx("navigation", { expanded: isExpanded }),
            attr: attr && attr.container,
        },
        React.createElement(
            Attributes_1.Elements.button,
            {
                className: "global-nav-item",
                key: "globalNavButton",
                onClick: onClick,
                attr: attr && attr.navButton,
            },
            React.createElement("span", {
                className: cx(
                    "global-nav-item-icon",
                    "icon",
                    "icon-globalNavButton"
                ),
            })
        ),
        React.createElement("div", { className: cx("scrollable") }, children)
    );
}
exports.Navigation = Navigation;
function NavigationItemSeparator() {
    return React.createElement("div", { className: cx("separator") });
}
exports.NavigationItemSeparator = NavigationItemSeparator;
exports.default = Navigation;
