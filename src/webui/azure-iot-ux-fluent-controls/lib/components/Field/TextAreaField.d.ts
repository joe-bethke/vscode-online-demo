/// <reference types="@types/react" />
import * as React from 'react';
import { MethodNode } from '../../Common';
import { TextAreaAttributes } from '../Input/TextArea';
import { FormFieldAttributes } from './FormField';
export interface TextAreaFieldType {
}
export interface TextAreaFieldProps extends React.Props<TextAreaFieldType> {
    /** HTML form element name */
    name: string;
    /** Current value of HTML textarea element */
    value: string;
    /** HTML textarea element placeholder */
    placeholder?: string;
    /** Label to display above textarea element */
    label: MethodNode;
    /** Error to display below textarea element */
    error?: MethodNode;
    /** Error HTML title in case of overflow */
    errorTitle?: string;
    /** Grow text area to fit user text */
    autogrow?: boolean;
    /** Disable HTML textarea element */
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
    /** Classname to append to top level element of TextArea */
    inputClassName?: string;
    /** React node to render at the far side of the label. */
    labelFarSide?: React.ReactNode;
    attr?: TextAreaAttributes & FormFieldAttributes;
}
/**
 * High level form text field
 *
 * @param props Control properties (defined in `TextAreaFieldProps` interface)
 */
export declare const TextAreaField: React.StatelessComponent<TextAreaFieldProps>;
export default TextAreaField;
