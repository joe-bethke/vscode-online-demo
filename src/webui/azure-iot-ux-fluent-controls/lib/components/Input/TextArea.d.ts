/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps, PreProps, TextAreaProps } from '../../Attributes';
export interface TextAreaType {
}
export interface TextAreaAttributes {
    container?: DivProps;
    textarea?: TextAreaProps;
    pre?: PreProps;
}
export interface TextAreaProps extends React.Props<TextAreaType> {
    /** HTML form element name */
    name: string;
    /** Current value of HTML input element */
    value?: string;
    /** Text area placeholder */
    placeholder?: string;
    /** Apply error styling to input element */
    error?: boolean;
    /** Add required attribute to HTML input element */
    required?: boolean;
    /** Disable HTML input element and apply disabled styling */
    disabled?: boolean;
    /** Read only HTML input element */
    readOnly?: boolean;
    /** Grow text area to fit user text */
    autogrow?: boolean;
    /** Autofocus */
    autoFocus?: boolean;
    /** Callback for HTML input element `onChange` events */
    onChange: (newValue: string) => void;
    /** Class to append to top level element */
    className?: string;
    attr?: TextAreaAttributes;
}
export interface TextAreaState {
}
/**
 * Low level text input control
 *
 * (Use the `TextField` control instead when making a form with standard styling)
 */
export declare class TextArea extends React.Component<TextAreaProps, TextAreaState> {
    static defaultProps: {
        autogrow: boolean;
        error: boolean;
        disabled: boolean;
        value: string;
        attr: {
            container: {};
            textarea: {};
            pre: {};
        };
    };
    private textarea;
    private ghost;
    constructor(props: TextAreaProps);
    componentDidUpdate(prevProps: TextAreaProps, prevState: TextAreaState): void;
    render(): JSX.Element;
    private bindTextArea;
    private bindGhost;
}
export default TextArea;
