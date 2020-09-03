/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps, ButtonProps, InputProps } from '../../Attributes';
import { MethodNode } from '../../Common';
export declare const prefixClassName: string;
export declare const postfixClassName: string;
export interface TextInputType {
}
export interface TextInputAttributes {
    container?: DivProps;
    input?: InputProps;
    inputContainer?: DivProps;
    prefix?: DivProps;
    postfix?: DivProps;
    clearButton?: ButtonProps;
}
export interface TextInputProps extends React.Props<TextInputType> {
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
    /** Node to draw to the left of the input box */
    prefix?: MethodNode;
    /** Class to append to prefix container */
    prefixClassName?: string;
    /** Node to draw to the right of the input box */
    postfix?: MethodNode;
    /** Class to append to postfix container */
    postfixClassName?: string;
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
    /** Callback for HTML input element `onChange` events */
    onChange: (newValue: string) => void;
    /** Class to append to top level element */
    className?: string;
    attr?: TextInputAttributes;
}
/**
 * Low level text input control
 *
 * (Use the `TextField` control instead when making a form with standard styling)
 */
export declare class TextInput extends React.PureComponent<TextInputProps> {
    static defaultProps: Partial<TextInputProps>;
    constructor(props: TextInputProps);
    onChange(event: React.SyntheticEvent<HTMLInputElement>): void;
    onClear(): void;
    render(): JSX.Element;
}
export default TextInput;
