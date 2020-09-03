/// <reference types="@types/react" />
import * as React from 'react';
import { IconAttributes } from '../Icon';
import { DivProps } from '../../Attributes';
export interface ActionTriggerAttributes {
    container?: DivProps;
    icon?: IconAttributes;
    suffix?: IconAttributes;
}
export interface ActionTriggerComponentType {
}
export interface ActionTriggerProps extends React.Props<ActionTriggerComponentType> {
    /** Icon name (from Segoe UI MDL font) */
    icon: string;
    /** Icon name for icon on the right of ActionTrigger (from Segoe UI MDL font) */
    rightIcon?: string;
    /** Action trigger label */
    label?: string;
    /** Disable Action Trigger */
    disabled?: boolean;
    /** Classname to append to top level element */
    className?: string;
    attr?: ActionTriggerAttributes;
}
/**
 * ActionTrigger showing Information, Warning, or Error with text, icon, and optional close button
 *
 * @param props Control properties (defined in `ActionTriggerProps` interface)
 */
export declare const ActionTrigger: React.StatelessComponent<ActionTriggerProps>;
export default ActionTrigger;
