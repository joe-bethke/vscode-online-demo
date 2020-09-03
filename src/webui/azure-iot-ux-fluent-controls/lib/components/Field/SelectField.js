Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    SelectInput_1 = require("../Input/SelectInput"),
    FormField_1 = require("./FormField");
/**
 * High level form select box control
 *
 * IMPORTANT: The options provided to this control must all be UNIQUE. The
 * `value` property of option tags is the numerical index of the option in
 * `SelectField.options` so `SelectField.value` is compared to each value in
 * `options` (===) to decide which option is the one currently selected.
 *
 * @param props: Object fulfilling `SelectFieldProps` interface
 */
exports.SelectField = (props) => {
    const tooltipId = props.tooltip ? `${props.name}-tt` : undefined,
        selectAttr = {
            container: props.attr.container,
            select: Object.assign(
                {
                    "aria-label": props.label,
                    "aria-describedby": tooltipId,
                },
                props.attr.select
            ),
            option: props.attr.option,
            chevron: props.attr.chevron,
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
        React.createElement(SelectInput_1.SelectInput, {
            name: props.name,
            value: props.value,
            options: props.options,
            error: !!props.error,
            disabled: props.disabled,
            onChange: props.onChange,
            className: props.inputClassName,
            autoFocus: props.autoFocus,
            required: props.required,
            attr: selectAttr,
        })
    );
};
exports.SelectField.defaultProps = {
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
        select: {},
        option: {},
        chevron: {},
    },
};
exports.default = exports.SelectField;
