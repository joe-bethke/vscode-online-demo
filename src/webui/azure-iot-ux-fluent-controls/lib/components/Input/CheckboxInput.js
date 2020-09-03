Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    styled_components_1 = require("styled-components"),
    Common_1 = require("../../Common"),
    Attributes_1 = require("../../Attributes"),
    Icon_1 = require("../Icon"),
    classNames = require("classnames/bind"),
    css = classNames.bind(require("./CheckboxInput.module.scss")),
    StyledActiveCheckboxButton = styled_components_1.default(
        Attributes_1.Elements.span
    )`
    &&&& {
        color: ${(props) => props.theme.colorTextBtnPrimaryRest};
        background-color: ${(props) => props.theme.colorBgBtnPrimaryRest};
    }
`;
/**
 * Low level checkbox control
 *
 * (Use the `CheckboxField` control instead when making a form with standard styling)
 *
 * @param props Control properties (defined in `CheckboxInputProps` interface)
 */
exports.CheckboxInput = (props) => {
    const containerClass = css(
            "checkbox-container",
            {
                columns: props.columns,
                disabled: props.disabled,
                selected: props.checked,
                indeterminate: props.indeterminate,
            },
            props.className
        ),
        id = `${props.name}_checkbox`,
        onChange = (event) => {
            // Stop propagation and call the onChange handler with the new value.
            event.stopPropagation();
            props.onChange(!props.checked);
        },
        CheckboxInputProxy = props.checked
            ? StyledActiveCheckboxButton
            : Attributes_1.Elements.span;
    return React.createElement(
        Attributes_1.Elements.div,
        {
            className: containerClass,
            hidden: props.hidden,
            attr: props.attr.container,
        },
        React.createElement(
            Attributes_1.Elements.label,
            {
                className: css("checkbox-label"),
                htmlFor: id,
                onClick: stopPropagation,
                attr: props.attr.label,
            },
            React.createElement(Attributes_1.Elements.input, {
                id: id,
                type: "checkbox",
                name: props.name,
                disabled: props.disabled,
                hidden: props.hidden,
                checked: props.checked,
                required: props.required,
                onChange: onChange,
                autoFocus: props.autoFocus,
                methodRef: props.autoFocus && Common_1.autoFocusRef,
                attr: props.attr.input,
            }),
            React.createElement(CheckboxInputProxy, {
                className: css("checkbox-button"),
                attr: props.attr.checkbox,
            }),
            React.createElement(
                Attributes_1.Elements.span,
                { className: css("checkbox-text"), attr: props.attr.text },
                props.label
            ),
            React.createElement(Attributes_1.Elements.span, {
                className: css("checkbox-fill"),
                attr: props.attr.indeterminateFill,
            }),
            React.createElement(
                styled_components_1.ThemeConsumer,
                null,
                (theme) =>
                    React.createElement(Icon_1.Icon, {
                        icon: "checkMark",
                        size: Icon_1.IconSize.xsmall,
                        className: css("checkbox-checkmark"),
                        attr: props.attr.checkmarkIcon,
                        color: theme && theme.colorTextBtnPrimaryRest,
                    })
            )
        )
    );
};
function stopPropagation(e) {
    // HACK! If we don't add this click event handler to the label, React never
    // fires the input onChange handler in IoT Central.
    e.stopPropagation();
}
exports.CheckboxInput.defaultProps = {
    name: undefined,
    label: undefined,
    onChange: undefined,
    attr: {
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
exports.default = exports.CheckboxInput;
