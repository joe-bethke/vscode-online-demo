/// <reference types="@types/react" />
import * as React from 'react';
import { ActionTriggerAttributes } from '../ActionTrigger';
import { ButtonProps } from '../../Attributes';
export interface ActionTriggerButtonAttributes {
    button?: ButtonProps;
}
export interface ActionTriggerButtonProps {
    /** Icon name (from Segoe UI MDL font) */
    icon: string;
    /** Icon name for icon on the right of ActionTrigger (from Segoe UI MDL font) */
    rightIcon?: string;
    /** Action trigger label */
    label?: string;
    /** Tab Index for Button */
    tabIndex?: number;
    /** Disable Action Trigger */
    disabled?: boolean;
    /** Classname to append to top level element */
    className?: string;
    /** Button onClick callback */
    onClick?: (event: any) => void;
    attr?: ActionTriggerButtonAttributes & ActionTriggerAttributes;
}
export declare const ActionTriggerButton: React.StatelessComponent<ActionTriggerButtonProps>;
export default ActionTriggerButton;
