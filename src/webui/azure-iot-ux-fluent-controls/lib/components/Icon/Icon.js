Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    Attributes_1 = require("../../Attributes"),
    classNames = require("classnames/bind"),
    css = classNames.bind(require("./Icon.module.scss"));
let IconSize;
(function (IconSize) {
    // 16px
    IconSize[(IconSize.xsmall = 1)] = "xsmall";
    // 32px
    IconSize[(IconSize.small = 2)] = "small";
    // 48px
    IconSize[(IconSize.medium = 3)] = "medium";
    // 64px
    IconSize[(IconSize.large = 4)] = "large";
    // 80px
    IconSize[(IconSize.xlarge = 5)] = "xlarge";
    // 96px
    IconSize[(IconSize.xxlarge = 6)] = "xxlarge";
})((IconSize = exports.IconSize || (exports.IconSize = {})));
/**
 * Icon loaded from Segoe UI MDL icons font
 *
 * Renders children so this control can be used with text
 *
 * @param props Control properties (Defined in `IconProps` interface)
 */
exports.Icon = (props) => {
    let iconClassName = `icon-${props.icon}`,
        cls = css(
            {
                "icon-xsmall": props.size === IconSize.xsmall,
                "icon-small": props.size === IconSize.small,
                "icon-medium": props.size === IconSize.medium,
                "icon-large": props.size === IconSize.large,
                "icon-xlarge": props.size === IconSize.xlarge,
                "icon-xxlarge": props.size === IconSize.xxlarge,
                centered: props.centered,
            },
            iconClassName,
            props.className
        ),
        style = { color: props.color };
    if (props.fontSize) {
        style.fontSize = `${props.fontSize}px`;
    }
    let label;
    if (props.children) {
        label = React.createElement(
            Attributes_1.Elements.span,
            { className: props.labelClassName, attr: props.attr.label },
            props.children
        );
    }
    return React.createElement(
        Attributes_1.Elements.span,
        { className: cls, style: style, attr: props.attr.container },
        label
    );
};
exports.Icon.defaultProps = {
    icon: undefined,
    size: IconSize.medium,
    attr: {
        container: {},
        label: {},
    },
};
exports.default = exports.Icon;
