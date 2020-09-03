Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    Attributes_1 = require("../../Attributes"),
    classNames = require("classnames/bind"),
    css = classNames.bind(require("./Icon.module.scss"));
/**
 * Background circle for Icons
 *
 * @param props Control properties (Defined by `IconBackgroundProps` interface)
 */
exports.IconBackground = (props) => {
    let cls = css(
            {
                "icon-background": true,
                centered: props.centered,
            },
            props.className
        ),
        style = {
            backgroundColor: props.backgroundColor,
        };
    if (props.diameter) {
        style.width = `${props.diameter}px`;
        style.height = `${props.diameter}px`;
        style.borderRadius = `${props.diameter / 2}px`;
    }
    return React.createElement(Attributes_1.Elements.div, {
        className: cls,
        style: style,
        attr: props.attr.container,
    });
};
exports.IconBackground.defaultProps = {
    backgroundColor: undefined,
    diameter: undefined,
    attr: {
        container: {},
    },
};
exports.default = exports.IconBackground;
