/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps, SpanProps, SelectProps, OptionProps, OptionAttr } from '../../Attributes';
import { FormOption } from '../../Common';
export interface SelectInputType {
}
export interface SelectInputState {
    cancelFocused: boolean;
}
export interface SelectInputAttributes {
    container?: DivProps;
    select?: SelectProps;
    option?: OptionProps;
    chevron?: SpanProps;
}
export interface SelectInputProps extends React.Props<SelectInputType> {
    /** HTML form element name */
    name: string;
    /**
     * Current value of HTML select element
     *
     * This must be an Object that is in `SelectInputProps.options`
     */
    value: any;
    /**
     * List of HTML select element options in the format:
     *
     * `{
     *     label: string,
     *     value: any
     * }`
     */
    options: (FormOption & OptionAttr<OptionProps>)[];
    /** Apply error styling to input element */
    error?: boolean;
    /** Add required attribute to HTML input element */
    required?: boolean;
    /** Disable HTML input element and apply disabled styling */
    disabled?: boolean;
    /** Autofocus */
    autoFocus?: boolean;
    /** Callback for HTML select element onChange events */
    onChange: (newValue: any) => void;
    /** Classname to append to top level element */
    className?: string;
    attr?: SelectInputAttributes;
}
/**
 * Low level select combo box control
 *
 * IMPORTANT: The options provided to this control must all be UNIQUE. The
 * `value` property of option tags is the numerical index of the option in
 * `SelectInput.options` so `SelectInput.value` is compared to each value in
 * `options` (===) to decide which option is the one currently selected.
 *
 * (Use the `SelectField` control instead when making a form with standard styling)
 *
 * @param props Control properties (defined in `SelectInputProps` interface)
 */
export declare const SelectInput: React.StatelessComponent<SelectInputProps>;
export default SelectInput;
