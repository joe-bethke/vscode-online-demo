/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps, SpanProps, InputProps, LabelProps } from '../../Attributes';
import { MethodNode } from '../../Common';
export interface RadioInputType {
}
export interface RadioInputState {
    cancelFocused: boolean;
}
export interface RadioInputAttributes {
    container?: DivProps;
    label?: LabelProps;
    input?: InputProps;
    radio?: SpanProps;
    text?: SpanProps;
    fill?: SpanProps;
    border?: SpanProps;
}
export interface RadioInputProps extends React.Props<RadioInputType> {
    /** HTML form element name */
    name: string;
    /** Value of HTML input element */
    value: string;
    /** Label for HTML input element */
    label: MethodNode;
    /** Allow multiple columns for radio button */
    columns?: boolean;
    /** Checked */
    checked?: boolean;
    /** Add required attribute to HTML input element */
    required?: boolean;
    /** Disable HTML input element and apply disabled styling */
    disabled?: boolean;
    /** Hide HTML input element */
    hidden?: boolean;
    /** Autofocus */
    autoFocus?: boolean;
    /** Callback for HTML radio button element onChange events */
    onChange: (newValue: string) => void;
    /** Classname to append to top level element */
    className?: string;
    attr?: RadioInputAttributes;
}
/**
 * Low level radio button control
 *
 * (Use the `RadioField` control instead when making a form with standard styling)
 *
 * @param props Control properties (defined in `RadioInputProps` interface)
 */
export declare const RadioInput: React.StatelessComponent<RadioInputProps>;
export default RadioInput;
