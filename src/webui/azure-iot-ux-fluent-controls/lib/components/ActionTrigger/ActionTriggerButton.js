Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    styled_components_1 = require("styled-components"),
    ActionTrigger_1 = require("../ActionTrigger"),
    Attributes_1 = require("../../Attributes"),
    classNames = require("classnames/bind"),
    css = classNames.bind(require("./ActionTrigger.module.scss")),
    StyledButton = styled_components_1.default(Attributes_1.Elements.button)`
    &&&&& {
        color: ${(props) => props.theme.colorTextBtnStandardRest};
        background-color: ${(props) => props.theme.colorBgBtnStandardRest};
        &:hover {
            background-color: ${(props) => props.theme.colorBgBtnStandardHover};
        }
        &:disabled {
            color: ${(props) => props.theme.colorTextBtnStandardDisabled};
            background-color: ${(props) =>
                props.theme.colorBgBtnStandardDisabled};
        }
        >*:hover {
            background-color: ${(props) => props.theme.colorBgBtnStandardHover};
        }
    }
`;
exports.ActionTriggerButton = (props) => {
    const {
        onClick,
        className,
        disabled,
        tabIndex,
        label,
        attr,
        icon,
        rightIcon,
    } = props;
    return React.createElement(
        StyledButton,
        {
            type: "button",
            onClick: onClick,
            className: css(
                "action-trigger-button",
                {
                    disabled: disabled,
                },
                className
            ),
            disabled: disabled,
            tabIndex: tabIndex,
            title: label,
            attr: attr && attr.button,
        },
        React.createElement(ActionTrigger_1.ActionTrigger, {
            icon: icon,
            rightIcon: rightIcon,
            label: label,
            disabled: disabled,
            attr: attr,
        })
    );
};
exports.ActionTriggerButton.defaultProps = {
    icon: undefined,
    attr: Object.assign(
        { button: {} },
        { container: {}, icon: {}, suffix: {} }
    ),
};
exports.default = exports.ActionTriggerButton;
