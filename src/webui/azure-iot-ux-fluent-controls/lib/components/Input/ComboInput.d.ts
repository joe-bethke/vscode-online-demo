/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps, ButtonProps, SpanProps, InputProps, OptionAttr } from '../../Attributes';
import { DropdownAttributes } from '../Dropdown';
import { MethodNode, FormOption } from '../../Common';
export interface ComboInputType {
}
export interface ComboInputAttributes extends DropdownAttributes {
    textbox?: DivProps;
    input?: InputProps;
    clearButton?: ButtonProps;
    chevron?: SpanProps;
    option?: ButtonProps;
}
export interface ComboInputProps extends React.Props<ComboInputType> {
    /** HTML form element name */
    name: string;
    /** Current value of HTML input element */
    value: string | any;
    /** HTML input element placeholder */
    placeholder?: string;
    /**
     * List of HTML select element options in the format:
     *
     * `{
     *     label: string,
     *     value: any,
     *     disabled: boolean,
     *     hidden: boolean
     * }`
     */
    options: (FormOption & OptionAttr<ButtonProps>)[];
    /**
     * Callback used to map FormOption to strings to be used by default
     * optionFilter and optionSelect callbacks
     *
     * See examples for how to use these callbacks
     */
    optionMap?: (option: FormOption) => string;
    /**
     * Callback used to filter list of FormOptions for display in the dropdown
     *
     * This function can, for example, implement autocomplete by hiding
     * any option that does not contain the value in the text input
     *
     * See examples for how to use these callbacks
     */
    optionFilter?: (newValue: string, option: FormOption) => boolean;
    /**
     * Callback used to decide whether a FormOption is selected or not
     *
     * See examples for how to use these callbacks
     */
    optionSelect?: (newValue: string, option: FormOption) => boolean;
    /**
     * Callback used to generate a React node to use as the label in dropdown
     *
     * This function can, for example, bold any relevant fragments of text for
     * highlighting in autocomplete
     *
     * See examples for how to use these callbacks
     */
    optionLabel?: (newValue: string, option: FormOption) => MethodNode;
    /** Apply error styling to input element */
    error?: boolean;
    /** Add required attribute to HTML input element */
    required?: boolean;
    /** Disable HTML input element and apply disabled styling */
    disabled?: boolean;
    /** Read only HTML input element */
    readOnly?: boolean;
    /** Autofocus */
    autoFocus?: boolean;
    /**
     * Show label instead of FormOption value in ComboInput text box when a
     * value from the FormOptions is selected
     *
     * Since the ComboInput has a text input, it cannot draw an arbitrary
     * MethodNode as the textbox value. If props.optionLabel returns a string,
     * then you can show the label text in the textbox instead of the option
     * value itself.
     *
     * Note: If the label and value are different and showLabel is true,
     * when the user starts typing after making a selection in the dropdown,
     * it will not reselect the option unless optionSelect checks the label
     * string as well as the value.
     *
     * For example:
     * ```js
     * optionSelect = (newValue, option) => {
     *     return newValue === option.value || newValue === option.label.toString();
     * }
     * ```
     *
     * Default: true
     */
    showLabel?: boolean;
    /** Callback for HTML input element `onChange` events */
    onChange: (newValue: string | FormOption) => void;
    /** Class to append to top level element */
    className?: string;
    /** Class to append to HTML Input element */
    inputClassName?: string;
    /** Class to append to top level dropdown element */
    dropdownClassName?: string;
    attr?: ComboInputAttributes;
}
export interface ComboInputState {
    visible: boolean;
    hovered: FormOption;
}
/**
 * Low level combo input control
 *
 * `ComboInput` is a hybrid of the SelectInput and TextInput controls. It
 * functions as a 'new or existing' text field where the user can type in a
 * custom value or pick from a list of values provided by the control.
 *
 * `ComboInput` consumes the property `options: FormOption[]` which specify
 * each option's `value` and `label`. The former can be any object while the
 * latter can be any React node (or a string). `ComboInput` also consumes a
 * `value: string | FormOption` property that sets the current value of the
 * `ComboInput` text field. If `value` is a `string`, the user is typing in a
 * custom value and if it is an object, the user has either typed in a value
 * equal to one of the options or has selected an option from the dropdown list.
 *
 * In this example of a default `ComboInput`, `FormOption.value` must be a
 * string, which allows you to use `ComboInput` with only the properties `name`,
 * `value`, `onChange`, and `options`. When the user types in 'Option 1', that
 * option will be considered selected instead of a custom object.
 *
 * *Reffer to the other examples on how to use `ComboInput`'s callbacks to
 * further modify what options display in the dropdown.*
 *
 * (Use the `ComboField` control for forms with standard styling)
 */
export declare class ComboInput extends React.Component<ComboInputProps, Partial<ComboInputState>> {
    static defaultProps: {
        optionMap: (option: FormOption) => string;
        optionLabel: (newValue: string, option: FormOption) => MethodNode;
        showLabel: boolean;
        attr: {
            container: {};
            textbox: {};
            input: {};
            clearButton: {};
            chevron: {};
            dropdown: {};
            option: {};
        };
    };
    inputElement: HTMLInputElement;
    optionFilter: (newValue: string, option: FormOption) => boolean;
    optionSelect: (newValue: string, option: FormOption) => boolean;
    private optionElements;
    private currentOption;
    constructor(props: ComboInputProps);
    inputRef: (input: HTMLInputElement) => void;
    onFocus(event: any): void;
    onKeyDown(event: any): void;
    getValue(): string;
    getVisibleOptions(getDisabled?: boolean): (FormOption & OptionAttr<ButtonProps>)[];
    onInputChange(event: any): void;
    showDropdown(): void;
    hideDropdown(): void;
    clearSelection(option: string): void;
    setSelection(option: string): void;
    render(): JSX.Element;
}
export default ComboInput;
