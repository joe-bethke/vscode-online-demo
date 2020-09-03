/// <reference types="@types/react" />
import * as React from 'react';
import { TextInputAttributes } from './TextInput';
import { MethodNode } from '../../Common';
export interface NumberInputType {
}
export interface NumberInputProps extends React.Props<NumberInputType> {
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
    /** Node to draw to the left of the input box */
    prefix?: MethodNode;
    /** Node to draw to the right of the input box */
    postfix?: MethodNode;
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
    onChange: (newValue: number | 'invalid') => void;
    /** Class to append to top level element */
    className?: string;
    attr?: TextInputAttributes;
}
export interface NumberInputState {
    value: string;
    paste?: boolean;
}
/**
 * Low level text input control
 *
 * (Use the `TextField` control instead when making a form with standard styling)
 */
export declare class NumberInput extends React.Component<NumberInputProps, NumberInputState> {
    static defaultProps: {
        name: any;
        initialValue: string;
        onChange: any;
        integer: boolean;
        positive: boolean;
        step: string;
        attr: {
            container: {};
            input: {};
            inputContainer: {};
            prefix: {};
            postfix: {};
        };
    };
    private paste;
    constructor(props: NumberInputProps);
    onKeyDown: (event: any) => void;
    isPositive(): boolean;
    isInteger(): boolean;
    onChange: (newValue: string) => void;
    onPaste: (event: any) => void;
    getInitialState(initialValue: number | string): NumberInputState;
    getValue(value: string): number | 'invalid';
    componentDidUpdate(oldProps: NumberInputProps, oldState: NumberInputState): void;
    componentWillReceiveProps(newProps: NumberInputProps): void;
    render(): JSX.Element;
}
export default NumberInput;
