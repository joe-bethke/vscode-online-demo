Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    TextInput_1 = require("../Input/TextInput"),
    FormField_1 = require("./FormField");
/**
 * High level form text field
 *
 * @param props Control properties (defined in `TextFieldProps` interface)
 */
exports.TextField = (props) => {
    const tooltipId = props.tooltip ? `${props.name}-tt` : undefined,
        errorId = `${props.name}-error`;
    let describedby = errorId;
    if (tooltipId) {
        describedby += " " + tooltipId;
    }
    const textAttr = {
            container: props.attr.container,
            input: Object.assign(
                {
                    "aria-label": props.label,
                    "aria-describedby": describedby,
                },
                props.attr.input
            ),
            inputContainer: props.attr.inputContainer,
            prefix: props.attr.prefix,
            postfix: props.attr.postfix,
            clearButton: props.attr.clearButton,
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
            fieldError: Object.assign(
                {
                    id: errorId,
                },
                props.attr.fieldError
            ),
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
        React.createElement(TextInput_1.TextInput, {
            name: props.name,
            value: props.value,
            placeholder: props.placeholder,
            type: props.type,
            prefix: props.prefix,
            prefixClassName: props.prefixClassName,
            postfix: props.postfix,
            postfixClassName: props.postfixClassName,
            error: !!props.error,
            disabled: props.disabled,
            readOnly: props.readOnly,
            onChange: props.onChange,
            className: props.inputClassName,
            autoFocus: props.autoFocus,
            required: props.required,
            attr: textAttr,
        })
    );
};
exports.TextField.defaultProps = {
    name: undefined,
    value: undefined,
    label: undefined,
    onChange: undefined,
    type: "text",
    attr: {
        fieldContainer: {},
        fieldLabel: {},
        fieldContent: {},
        fieldError: {},
        container: {},
        input: {},
        inputContainer: {},
        prefix: {},
        postfix: {},
        clearButton: {},
    },
};
exports.default = exports.TextField;
