Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    FormField_1 = require("./FormField"),
    Toggle_1 = require("../Toggle");
/**
 * High level form toggle switch control
 *
 * @param props: Object fulfilling `ToggleFieldProps` interface
 */
exports.ToggleField = (props) => {
    const tooltipId = props.tooltip ? `${props.name}-tt` : undefined,
        toggleAttr = {
            container: props.attr.container,
            button: Object.assign(
                {
                    "aria-label": props.label,
                    "aria-describedby": tooltipId,
                },
                props.attr.button
            ),
            border: props.attr.border,
            switch: props.attr.switch,
            text: props.attr.text,
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
        React.createElement(Toggle_1.Toggle, {
            on: props.value,
            name: props.name,
            disabled: props.disabled,
            onChange: props.onChange,
            onLabel: props.onLabel,
            offLabel: props.offLabel,
            className: props.inputClassName,
            autoFocus: props.autoFocus,
            attr: toggleAttr,
        })
    );
};
exports.ToggleField.defaultProps = {
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
        button: {},
        border: {},
        switch: {},
        text: {},
    },
};
exports.default = exports.ToggleField;
