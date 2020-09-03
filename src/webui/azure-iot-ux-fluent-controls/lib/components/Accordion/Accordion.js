Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    classNames = require("classnames/bind"),
    cx = classNames.bind(require("./Accordion.module.scss"));
class Accordion extends React.PureComponent {
    render() {
        return React.createElement(
            "div",
            null,
            React.createElement(
                "button",
                {
                    role: this.props.attr.ariaRole,
                    id: `${this.props.id}-label`,
                    "aria-expanded": !!this.props.expanded,
                    "aria-controls": `${this.props.id}-content`,
                    "data-test-hook":
                        this.props.attr.dataTestHook &&
                        `${this.props.attr.dataTestHook}-label`,
                    onClick: this.props.onToggle,
                    className: cx("accordion-label"),
                },
                React.createElement(
                    "div",
                    { className: cx("inline-text-overflow") },
                    this.props.label
                ),
                React.createElement("i", {
                    className: cx("icon", {
                        "icon-chevronDown": !this.props.expanded,
                        "icon-chevronUp": !!this.props.expanded,
                    }),
                })
            ),
            React.createElement(
                "div",
                {
                    id: `${this.props.id}-content`,
                    role: "region",
                    "aria-labelledby": `${this.props.id}-label`,
                    "data-test-hook":
                        this.props.attr.dataTestHook &&
                        `${this.props.attr.dataTestHook}-content`,
                    className: cx("accordion-content", {
                        open: !!this.props.expanded,
                    }),
                },
                this.props.children
            )
        );
    }
}
Accordion.defaultProps = {
    attr: {
        ariaRole: null,
        dataTestHook: null,
    },
};
exports.Accordion = Accordion;
