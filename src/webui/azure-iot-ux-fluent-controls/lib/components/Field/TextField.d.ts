/// <reference types="@types/react" />
import * as React from 'react';
import { MethodNode } from '../../Common';
import { TextInputAttributes } from '../Input/TextInput';
import { FormFieldAttributes } from './FormField';
export interface TextFieldType {
}
export interface TextFieldProps extends React.Props<TextFieldType> {
    /** HTML form element name */
    name: string;
    /** Current value of HTML input element */
    value: string;
    /** HTML input element placeholder */
    placeholder?: string;
    /**
     * HTML input element type
     *
     * Default: text
     */
    type?: string;
    /** Label to display above input element */
    label: MethodNode;
    /** Error to display below input element */
    error?: MethodNode;
    /** Error HTML title in case of overflow */
    errorTitle?: string;
    /** Node to draw to the left of the input box */
    prefix?: MethodNode;
    /** Class to append to prefix container */
    prefixClassName?: string;
    /** Node to draw to the right of the input box */
    postfix?: MethodNode;
    /** Class to append to postfix container */
    postfixClassName?: string;
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
    onChange: (newValue: string) => void;
    /** Classname to append to top level element */
    className?: string;
    /** Classname to append to top level element of TextInput */
    inputClassName?: string;
    /** Extra action to render at the far side of the label */
    labelFarSide?: React.ReactNode;
    attr?: TextInputAttributes & FormFieldAttributes;
}
/**
 * High level form text field
 *
 * @param props Control properties (defined in `TextFieldProps` interface)
 */
export declare const TextField: React.StatelessComponent<TextFieldProps>;
export default TextField;
