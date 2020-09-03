Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    Attributes_1 = require("../../Attributes"),
    classNames = require("classnames/bind"),
    css = classNames.bind(require("./Field.module.scss"));
/**
 * Form Error
 *
 * @param props Control properties (defined in `FormErrorProps` interface)
 */
exports.FormError = (props) => {
    return React.createElement(
        Attributes_1.Elements.div,
        {
            className: css(
                "field-error",
                {
                    hidden: props.hidden,
                },
                props.className
            ),
            title: props.title,
            attr: props.attr.container,
        },
        props.children
    );
};
exports.FormError.defaultProps = {
    attr: {
        container: {},
    },
};
exports.default = exports.FormError;
