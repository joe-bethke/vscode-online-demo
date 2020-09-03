Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    Attributes_1 = require("../../Attributes"),
    RadioInput_1 = require("../Input/RadioInput"),
    FormField_1 = require("./FormField");
/**
 * High level form select box control
 *
 * IMPORTANT: The options provided to this control must all be UNIQUE. The
 * `value` property of radio buttons is the numerical index of the option in
 * `RadioField.options` so `RadioField.value` is compared to each value in
 * `options` (===) to decide which option is the one currently selected.
 *
 * @param props: Object fulfilling `RadioFieldProps` interface
 */
exports.RadioField = (props) => {
    const onChange = (newValue) => {
            const index = parseInt(newValue);
            props.onChange(props.options[index].value);
        },
        tooltipId = props.tooltip ? `${props.name}-tt` : undefined,
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
        },
        options = props.options.map((option, index) => {
            const radioAttr = {
                container: props.attr.container,
                label: props.attr.label,
                input: Object.assign(
                    {
                        "aria-label": option.label,
                        "aria-describedby": tooltipId,
                    },
                    props.attr.input
                ),
                radio: props.attr.radio,
                text: props.attr.text,
                fill: props.attr.fill,
                border: props.attr.border,
            };
            return React.createElement(RadioInput_1.RadioInput, {
                name: props.name,
                value: `${index}`,
                label: option.label,
                columns: props.columns,
                checked: props.value === option.value,
                disabled: props.disabled || option.disabled,
                hidden: option.hidden,
                onChange: onChange,
                className: props.inputClassName,
                key: `${props.name}-${index}`,
                autoFocus: props.autoFocus,
                required: props.required,
                attr: Attributes_1.mergeAttributeObjects(
                    radioAttr,
                    option.attr,
                    [
                        "container",
                        "label",
                        "input",
                        "radio",
                        "text",
                        "fill",
                        "border",
                    ]
                ),
            });
        });
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
        React.createElement("div", null, options)
    );
};
exports.RadioField.defaultProps = {
    name: undefined,
    value: undefined,
    label: undefined,
    onChange: undefined,
    options: undefined,
    attr: {
        fieldContainer: {},
        fieldLabel: {},
        fieldContent: {},
        fieldError: {},
        container: {},
        label: {},
        input: {},
        radio: {},
        text: {},
        fill: {},
        border: {},
    },
};
exports.default = exports.RadioField;
