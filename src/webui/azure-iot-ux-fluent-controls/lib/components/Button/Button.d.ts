/// <reference types="@types/react" />
import * as React from 'react';
import { SpanProps, ButtonProps as AttrButtonProps } from '../../Attributes';
export interface ButtonComponentType {
}
export interface ButtonAttributes {
    container?: AttrButtonProps;
    icon?: SpanProps;
}
export interface ButtonProps extends React.Props<ButtonComponentType> {
    /** Button title attribute */
    title?: string;
    /** Button type attribute */
    type?: string;
    /** Icon name (from icons.css) */
    icon?: string;
    /** Use primary style */
    primary?: boolean;
    /** Disable button */
    disabled?: boolean;
    /**
     * Callback for button onClick
     */
    onClick: (event: any) => void;
    /** Classname to append to top level element */
    className?: string;
    attr?: ButtonAttributes;
}
/**
 * Button showing Information, Warning, or Error with text, icon, and optional close button
 *
 * @param props Control properties (defined in `ButtonProps` interface)
 */
export declare const Button: React.StatelessComponent<ButtonProps>;
export default Button;
