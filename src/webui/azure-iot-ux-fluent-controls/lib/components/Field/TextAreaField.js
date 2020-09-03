Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const TextArea_1 = require("../Input/TextArea"),
    FormField_1 = require("./FormField");
/**
 * High level form text field
 *
 * @param props Control properties (defined in `TextAreaFieldProps` interface)
 */
exports.TextAreaField = (props) => {
    const tooltipId = props.tooltip ? `${props.name}-tt` : undefined,
        textAreaAttr = {
            container: props.attr.container,
            textarea: Object.assign(
                {
                    "aria-label": props.label,
                    "aria-describedby": tooltipId,
                },
                props.attr.textarea
            ),
            pre: props.attr.pre,
        },
        fieldAttr = {
            fieldLabel: Object.assign(
                {
                    balloon: {
                        balloonContent: {
                            id: tooltipId,
                        },
                    },
                },
                props.attr.fieldLabel
            ),
            fieldError: props.attr.fieldError,
            fieldContent: props.attr.fieldContent,
            fieldContainer: props.attr.fieldContainer,
        };
    return React.createElement(
        FormField_1.FormField,
        {
            name: props.name,
            label: props.label,
            error: props.error,
            errorTitle: props.errorTitle,
            loading: props.loading,
            required: props.required,
            tooltip: props.tooltip,
            className: props.className,
            attr: fieldAttr,
            labelFarSide: props.labelFarSide,
        },
        React.createElement(TextArea_1.TextArea, {
            name: props.name,
            value: props.value,
            placeholder: props.placeholder,
            error: !!props.error,
            disabled: props.disabled,
            readOnly: props.readOnly,
            onChange: props.onChange,
            className: props.inputClassName,
            autogrow: props.autogrow,
            autoFocus: props.autoFocus,
            required: props.required,
            attr: textAreaAttr,
        })
    );
};
exports.TextAreaField.defaultProps = {
    name: undefined,
    value: undefined,
    label: undefined,
    onChange: undefined,
    attr: {
        fieldContainer: {},
        fieldLabel: {},
        fieldContent: {},
        fieldError: {},
        container: {},
        textarea: {},
        pre: {},
    },
};
exports.default = exports.TextAreaField;
