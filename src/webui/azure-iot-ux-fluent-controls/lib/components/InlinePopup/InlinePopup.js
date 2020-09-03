Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    classNames = require("classnames/bind"),
    cx = classNames.bind(require("./InlinePopup.module.scss")),
    inlinePopupDefaultProps = {
        attr: {
            tabIndex: 0,
            dataTestHook: null,
            ariaLabel: null,
            ariaDescribedBy: null,
        },
    };
class Container extends React.PureComponent {
    render() {
        return React.createElement(
            "div",
            {
                "data-test-hook": this.props.attr.dataTestHook,
                className: cx(
                    "inline-popup-container",
                    {
                        expanded: this.props.expanded,
                        disabled: this.props.disabled,
                    },
                    this.props.className
                ),
            },
            React.Children.map(this.props.children, (child) =>
                React.cloneElement(child, {
                    expanded: this.props.expanded,
                    tabIndex: this.props.attr.tabIndex,
                    onClick: this.props.onClick,
                    disabled: this.props.disabled,
                })
            )
        );
    }
}
Container.defaultProps = inlinePopupDefaultProps;
exports.Container = Container;
class Label extends React.Component {
    render() {
        const tabIndex = this.props.attr.tabIndex || 0;
        return React.createElement(
            "button",
            {
                "aria-label": this.props.attr.ariaLabel,
                "aria-expanded": !!this.props.expanded,
                "aria-describedby": this.props.attr.ariaDescribedBy,
                title: this.props.title,
                type: "button",
                className: cx(
                    "inline-popup-label",
                    "inline-btn",
                    {
                        disabled: this.props.disabled,
                    },
                    this.props.className
                ),
                disabled: this.props.disabled,
                tabIndex: tabIndex,
                onClick: this.props.onClick,
            },
            this.props.children
        );
    }
}
Label.defaultProps = inlinePopupDefaultProps;
exports.Label = Label;
class Panel extends React.Component {
    componentDidUpdate() {
        if (this.props.expanded && this.props.scrollIntoView) {
            this.rawElement.scrollIntoView();
        }
    }
    render() {
        if (!this.props.expanded) {
            return null;
        }
        return React.createElement(
            "div",
            {
                className: cx(
                    "inline-popup-panel",
                    this.props.alignment,
                    {
                        disabled: this.props.disabled,
                    },
                    this.props.className
                ),
                onClick: (e) => e.stopPropagation(),
                ref: (rawElement) => (this.rawElement = rawElement),
            },
            this.props.children
        );
    }
}
Panel.defaultProps = inlinePopupDefaultProps;
exports.Panel = Panel;
