/// <reference types="@types/react" />
import * as React from 'react';
import { MethodNode } from '../../Common';
import { TextInputAttributes } from '../Input/TextInput';
import { FormFieldAttributes } from './FormField';
export interface NumberFieldType {
}
export interface NumberFieldProps extends React.Props<NumberFieldType> {
    /** HTML form element name */
    name: string;
    /** Current value of HTML input element */
    initialValue?: string | number;
    /** HTML input element placeholder */
    placeholder?: string;
    /** Step to give the number input */
    step?: number | 'any';
    /** Minimum value of HTML Input element */
    min?: number;
    /** Maximum value of HTML Input element */
    max?: number;
    /** Label to display above input element */
    label: MethodNode;
    /** Error to display below input element */
    error?: MethodNode;
    /** Error HTML title in case of overflow */
    errorTitle?: string;
    /** Node to draw to the left of the input box */
    prefix?: MethodNode;
    /** Node to draw to the right of the input box */
    postfix?: MethodNode;
    /** Disable HTML input element */
    disabled?: boolean;
    /** Read only HTML input element */
    readOnly?: boolean;
    /** Form field is required (appends a red asterisk to the label) */
    required?: boolean;
    /** Display horizontal loading animation instead of error */
    loading?: boolean;
    /** Autofocus */
    autoFocus?: boolean;
    /** Tooltip text to display in info icon bubble */
    tooltip?: MethodNode;
    /** Callback for HTML input element `onChange` events */
    onChange: (newValue: number | 'invalid') => void;
    /** Classname to append to top level element */
    className?: string;
    /** Classname to append to top level element of TextInput */
    inputClassName?: string;
    /** React node to render at the far side of the label. */
    labelFarSide?: React.ReactNode;
    attr?: TextInputAttributes & FormFieldAttributes;
}
/**
 * High level form text field
 *
 * @param props Control properties (defined in `NumberFieldProps` interface)
 */
export declare const NumberField: React.StatelessComponent<NumberFieldProps>;
export default NumberField;
