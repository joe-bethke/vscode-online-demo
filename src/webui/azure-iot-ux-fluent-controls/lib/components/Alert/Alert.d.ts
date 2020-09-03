/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps } from '../../Attributes';
import { IconAttributes } from '../Icon';
export declare enum AlertType {
    Information = 0,
    Warning = 1,
    Error = 2
}
export interface AlertComponentType {
}
export interface AlertAttributes {
    container?: DivProps;
    icon?: IconAttributes;
    contents?: DivProps;
    closeButtonTitle?: string;
}
export interface AlertProps extends React.Props<AlertComponentType> {
    /** Icon name (from Segoe UI MDL font) */
    icon?: string;
    /**
     * Alert type described using `AlertType` enum
     *
     * (`AlertType.[Information | Warning | Error]`)
     */
    type?: AlertType;
    /**
     * Callback for close button
     *
     * (If empty, the close button is not displayed)
     */
    onClose?: () => void;
    /** Fixed width (284 pixels) */
    fixed?: boolean;
    /**
     * Alert displays multiple lines
     *
     * (By default, alerts only show one line with ellipsis overflow)
     */
    multiline?: boolean;
    /** Classname to append to top level element */
    className?: string;
    attr?: AlertAttributes;
}
/**
 * Alert showing Information, Warning, or Error with text, icon, and optional close button
 *
 * @param props Control properties (defined in `AlertProps` interface)
 */
export declare const Alert: React.StatelessComponent<AlertProps>;
export default Alert;
