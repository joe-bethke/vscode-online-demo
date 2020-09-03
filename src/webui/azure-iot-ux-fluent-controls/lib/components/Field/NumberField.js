Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    NumberInput_1 = require("../Input/NumberInput"),
    FormField_1 = require("./FormField");
/**
 * High level form text field
 *
 * @param props Control properties (defined in `NumberFieldProps` interface)
 */
exports.NumberField = (props) => {
    const tooltipId = props.tooltip ? `${props.name}-tt` : undefined,
        numberAttr = {
            container: props.attr.container,
            input: Object.assign(
                {
                    "aria-label": props.label,
                    "aria-describedby": tooltipId,
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
        React.createElement(NumberInput_1.NumberInput, {
            name: props.name,
            initialValue: props.initialValue,
            placeholder: props.placeholder,
            prefix: props.prefix,
            postfix: props.postfix,
            error: !!props.error,
            disabled: props.disabled,
            readOnly: props.readOnly,
            onChange: props.onChange,
            className: props.inputClassName,
            autoFocus: props.autoFocus,
            step: props.step,
            min: props.min,
            max: props.max,
            required: props.required,
            attr: numberAttr,
        })
    );
};
exports.NumberField.defaultProps = {
    name: undefined,
    label: undefined,
    onChange: undefined,
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
    },
};
exports.default = exports.NumberField;
