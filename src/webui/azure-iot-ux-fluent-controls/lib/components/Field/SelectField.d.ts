/// <reference types="@types/react" />
import * as React from 'react';
import { OptionAttr, OptionProps } from '../../Attributes';
import { MethodNode, FormOption } from '../../Common';
import { SelectInputAttributes } from '../Input/SelectInput';
import { FormFieldAttributes } from './FormField';
export interface SelectFieldType {
}
export interface SelectFieldProps extends React.Props<SelectFieldType> {
    /** HTML form element name */
    name: string;
    /**
     * Current value of HTML select element
     *
     * This must be an `Object` that is in `SelectInputProps.options`
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
    /** Label to display above input element */
    label: MethodNode;
    /** Error to display below input element */
    error?: MethodNode;
    /** Error HTML title in case of overflow */
    errorTitle?: string;
    /** Disable HTML input element */
    disabled?: boolean;
    /** Form field is required (appends a red asterisk to the label) */
    required?: boolean;
    /** Display horizontal loading animation instead of error */
    loading?: boolean;
    /** Autofocus */
    autoFocus?: boolean;
    /** Tooltip text to display in info icon bubble */
    tooltip?: MethodNode;
    /** Callback for HTML select element `onChange` events */
    onChange: (newValue: any) => void;
    /** Classname to append to top level element */
    className?: string;
    /** Classname to append to top level element of SelectInput */
    inputClassName?: string;
    /** React node to render at the far side of the label. */
    labelFarSide?: React.ReactNode;
    attr?: SelectInputAttributes & FormFieldAttributes;
}
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
export declare const SelectField: React.StatelessComponent<SelectFieldProps>;
export default SelectField;
