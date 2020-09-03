/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps, ButtonProps } from '../../Attributes';
import { MethodNode } from '../../Common';
export interface ToggleType {
}
export interface ToggleAttributes {
    container?: DivProps;
    button?: ButtonProps;
    border?: DivProps;
    switch?: DivProps;
    text?: DivProps;
}
export interface ToggleProps extends React.Props<ToggleType> {
    on?: boolean;
    /** Disable Action Trigger */
    disabled?: boolean;
    /** AutoFocus */
    autoFocus?: boolean;
    name: string;
    onLabel?: MethodNode;
    offLabel?: MethodNode;
    onChange: (newValue: boolean) => void;
    /** Classname to append to top level element */
    className?: string;
    attr?: ToggleAttributes;
}
/**
 * Toggle button that is an on or off state
 *
 * @param props Control properties (defined in `ToggleProps` interface)
 */
export declare const Toggle: React.StatelessComponent<ToggleProps>;
export default Toggle;
