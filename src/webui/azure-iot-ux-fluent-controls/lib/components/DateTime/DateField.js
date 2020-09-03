Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    Common_1 = require("../../Common"),
    DatePicker_1 = require("./DatePicker"),
    FormField_1 = require("../Field/FormField");
/**
 * High level form text field
 *
 * @param props Control properties (defined in `DateFieldProps` interface)
 */
exports.DateField = (props) => {
    const tooltipId = props.tooltip ? `${props.name}-tt` : undefined,
        errorId = `${props.name}-error`;
    let describedby = errorId;
    if (tooltipId) {
        describedby += " " + tooltipId;
    }
    const dateAttr = {
            input: Object.assign(
                {
                    "aria-label": props.label,
                    "aria-describedby": describedby,
                },
                props.attr.input
            ),
            inputContainer: props.attr.inputContainer,
            inputIcon: props.attr.inputIcon,
            calendar: props.attr.calendar,
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
            className: props.className,
            attr: fieldAttr,
            tooltip: props.tooltip,
            labelFarSide: props.labelFarSide,
        },
        React.createElement(DatePicker_1.DatePicker, {
            name: props.name,
            initialValue: props.initialValue,
            locale: props.locale,
            localTimezone: props.localTimezone,
            tabIndex: props.tabIndex,
            showAbove: props.showAbove,
            format: props.format,
            error: !!props.error,
            disabled: props.disabled,
            required: props.required,
            onChange: props.onChange,
            onExpand: props.onExpand,
            className: props.inputClassName,
            attr: dateAttr,
        })
    );
};
exports.DateField.defaultProps = {
    name: undefined,
    label: undefined,
    onChange: undefined,
    format: Common_1.DateFormat.MMDDYYYY,
    tabIndex: -1,
    localTimezone: true,
    showAbove: false,
    attr: {
        fieldContainer: {},
        fieldLabel: {},
        fieldContent: {},
        fieldError: {},
        inputContainer: {},
        input: {},
        inputIcon: {},
        calendar: {},
    },
};
exports.default = exports.DateField;
