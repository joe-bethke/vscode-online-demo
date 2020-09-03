Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    styled_components_1 = require("styled-components"),
    InlinePopup = require("../InlinePopup"),
    ActionTrigger_1 = require("../ActionTrigger"),
    Attributes_1 = require("../../Attributes"),
    SearchInput_1 = require("../SearchInput/SearchInput"),
    classNames = require("classnames/bind"),
    cx = classNames.bind(require("./Masthead.module.scss")),
    // Root container
    MastheadContainer = styled_components_1.default(Attributes_1.Elements.div)`
    &&&& {
        color: ${(props) => props.theme.colorTextMastheadRest};
        background-color: ${(props) => props.theme.colorBgMasthead};
    }
`;
// Translates Shell theme to action button theme under current context.
function toolbarButtonTheme(theme) {
    return theme
        ? {
              base: theme.base,
              colorBgBtnStandardRest: theme.colorBgMasthead,
              colorBgBtnStandardHover: theme.colorBgMastheadHover,
              colorBgBtnStandardDisabled: theme.colorBgMastheadDisabled,
              colorTextBtnStandardRest: theme.colorTextMastheadRest,
              colorTextBtnStandardDisabled: theme.colorTextMastheadDisabled,
          }
        : { base: "light" }; // Theme must be an object.
}
class Masthead extends React.PureComponent {
    constructor() {
        super(...arguments);
        this.getToolbarItems = () => {
            if (!this.props.toolbarItems) {
                return null;
            }
            return this.props.toolbarItems.map((item, idx) => {
                const { label, icon, onClick, selected, attr } = item;
                return React.createElement(
                    "li",
                    {
                        key: idx,
                        className: cx("masthead-toolbar-btn-container", {
                            "selected-more": this.props.more.selected,
                        }),
                    },
                    React.createElement(ActionTrigger_1.ActionTriggerButton, {
                        label: label,
                        attr: attr,
                        icon: icon,
                        onClick: onClick,
                        className: cx("masthead-toolbar-btn", {
                            selected: selected,
                        }),
                    })
                );
            });
        };
    }
    render() {
        const { navigation, user, search, more, logo, branding } = this.props,
            items = this.getToolbarItems(),
            expanded = search && search.expanded;
        return React.createElement(
            MastheadContainer,
            { key: "Masthead", role: "banner", className: cx("masthead") },
            navigation &&
                React.createElement(
                    InlinePopup.Container,
                    {
                        expanded: navigation.isExpanded,
                        onClick: navigation.onClick,
                        className: cx("nav-container", {
                            "force-hide-search": expanded,
                        }),
                    },
                    React.createElement(InlinePopup.Label, {
                        className: cx("icon", "icon-chevronRight", {
                            "nav-icon-collapsed": !navigation.isExpanded,
                            "nav-icon-expanded": navigation.isExpanded,
                        }),
                    }),
                    React.createElement(
                        InlinePopup.Panel,
                        { alignment: "left", className: cx("nav-panel") },
                        navigation.children
                    )
                ),
            logo &&
                React.createElement(
                    Attributes_1.Elements.div,
                    {
                        key: "masthead-logo",
                        className: cx("masthead-logo", {
                            "force-hide-search": expanded,
                        }),
                    },
                    logo
                ),
            React.createElement(
                Attributes_1.Elements.span,
                {
                    key: "masthead-branding",
                    className: cx("masthead-branding", "inline-text-overflow", {
                        "force-hide-search": expanded,
                    }),
                },
                branding
            ),
            search &&
                React.createElement(SearchInput_1.SearchInput, {
                    containerClassName: cx("search-input-container", {
                        "force-show-search": expanded,
                    }),
                    inputClassName: cx("masthead-search-input"),
                    onChange: search.onChange,
                    value: search.value,
                    onSubmit: search.onSubmit,
                    label: search.label,
                    attr: search.attr,
                }),
            React.createElement(
                styled_components_1.ThemeProvider,
                { theme: toolbarButtonTheme },
                React.createElement(
                    Attributes_1.Elements.div,
                    {
                        className: cx("masthead-toolbar-container", {
                            "force-hide-search": expanded,
                        }),
                    },
                    React.createElement(
                        "ul",
                        { className: cx("masthead-toolbar") },
                        search &&
                            React.createElement(
                                "li",
                                {
                                    key: "item-search",
                                    className: cx("search-button"),
                                },
                                React.createElement(
                                    ActionTrigger_1.ActionTriggerButton,
                                    {
                                        key: search.label,
                                        attr: {
                                            button: { title: search.label },
                                        },
                                        icon: "search",
                                        onClick: search.onExpand,
                                        className: cx("masthead-toolbar-btn"),
                                    }
                                )
                            ),
                        more && !more.selected && items,
                        more &&
                            React.createElement(
                                "li",
                                {
                                    key: "item-more",
                                    className: cx("more-button"),
                                    title: more.attr && more.attr.ariaLabel,
                                },
                                React.createElement(
                                    InlinePopup.Container,
                                    {
                                        expanded: more.selected,
                                        onClick: more.onClick,
                                        attr: more.attr,
                                    },
                                    React.createElement(
                                        InlinePopup.Label,
                                        {
                                            className: cx(
                                                "masthead-toolbar-btn",
                                                "more-menu-btn",
                                                { selected: more.selected }
                                            ),
                                            onClick: more.onClick,
                                            attr: more.attr,
                                            title: more.title,
                                        },
                                        React.createElement(
                                            Attributes_1.Elements.span,
                                            { className: cx("icon icon-more") }
                                        )
                                    ),
                                    React.createElement(
                                        InlinePopup.Panel,
                                        {
                                            alignment: "right",
                                            className: cx(
                                                "masthead-toolbar-menu"
                                            ),
                                        },
                                        React.createElement(
                                            "ul",
                                            { role: "menu", id: "more-menu" },
                                            items
                                        )
                                    )
                                )
                            ),
                        user &&
                            React.createElement(
                                "li",
                                {
                                    key: "user-menu",
                                    className: cx("user-menu-item"),
                                },
                                user
                            )
                    )
                )
            )
        );
    }
}
exports.Masthead = Masthead;
exports.default = Masthead;
