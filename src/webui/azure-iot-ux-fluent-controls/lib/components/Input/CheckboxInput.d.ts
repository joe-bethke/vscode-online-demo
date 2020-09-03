/// <reference types="@types/react" />
import * as React from 'react';
import { MethodNode } from '../../Common';
import { DivProps, LabelProps, SpanProps, InputProps } from '../../Attributes';
import { IconAttributes } from '../Icon';
export interface CheckboxInputType {
}
export interface CheckboxInputState {
    cancelFocused: boolean;
}
export interface CheckboxInputAttributes {
    container?: DivProps;
    label?: LabelProps;
    input?: InputProps;
    text?: SpanProps;
    checkbox?: SpanProps;
    indeterminateFill?: SpanProps;
    checkmarkIcon?: IconAttributes;
    border?: SpanProps;
}
export interface CheckboxInputProps extends React.Props<CheckboxInputType> {
    /** HTML form element name */
    name: string;
    /** Label for HTML input element */
    label: MethodNode;
    /** Allow multiple columns for checkbox */
    columns?: boolean;
    /** Checked */
    checked?: boolean;
    /** Apply hidden attribute to checkbox */
    hidden?: boolean;
    /** Add required attribute to HTML input element */
    required?: boolean;
    /** Disable HTML input element and apply disabled styling */
    disabled?: boolean;
    /** Shows the checkbox in indeterminate state */
    indeterminate?: boolean;
    /** Autofocus */
    autoFocus?: boolean;
    /** Callback for HTML radio button element onChange events */
    onChange: (newValue: boolean) => void;
    /** Classname to append to top level element */
    className?: string;
    attr?: CheckboxInputAttributes;
}
/**
 * Low level checkbox control
 *
 * (Use the `CheckboxField` control instead when making a form with standard styling)
 *
 * @param props Control properties (defined in `CheckboxInputProps` interface)
 */
export declare const CheckboxInput: React.StatelessComponent<CheckboxInputProps>;
export default CheckboxInput;
