Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    CheckboxInput_1 = require("../Input/CheckboxInput"),
    FormField_1 = require("./FormField");
/**
 * High level form checkbox control
 *
 * IMPORTANT: The options provided to this control must all be UNIQUE. The
 * `value` property of checkboxes is the numerical index of the option in
 * `CheckboxField.options` so `CheckboxField.value` is compared to each value in
 * `options` (===) to decide which option is the one currently selected.
 *
 * @param props: Object fulfilling `CheckboxFieldProps` interface
 */
exports.CheckboxField = (props) => {
    const tooltipId = props.tooltip ? `${props.name}-tt` : undefined,
        checkboxAttr = {
            container: props.attr.container,
            input: Object.assign(
                {
                    "aria-label": props.label,
                    "aria-describedby": tooltipId,
                },
                props.attr.input
            ),
            label: props.attr.label,
            text: props.attr.text,
            checkbox: props.attr.checkbox,
            indeterminateFill: props.attr.indeterminateFill,
            checkmarkIcon: props.attr.checkmarkIcon,
            border: props.attr.border,
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
        React.createElement(
            "div",
            null,
            React.createElement(CheckboxInput_1.CheckboxInput, {
                name: props.name,
                checked: props.value,
                label: props.label,
                disabled: props.disabled,
                onChange: props.onChange,
                className: props.inputClassName,
                autoFocus: props.autoFocus,
                required: props.required,
                attr: checkboxAttr,
            })
        )
    );
};
exports.CheckboxField.defaultProps = {
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
        label: {},
        input: {},
        text: {},
        checkbox: {},
        indeterminateFill: {},
        checkmarkIcon: {},
        border: {},
    },
};
exports.default = exports.CheckboxField;
