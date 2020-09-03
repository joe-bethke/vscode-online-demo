/// <reference types="@types/react" />
import * as React from 'react';
import { ActionTriggerAttributes } from '../ActionTrigger';
import { AnchorProps } from '../../Attributes';
export interface ActionTriggerLinkAttributes {
    anchor?: AnchorProps;
}
export interface ActionTriggerLinkProps {
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
    /** Anchor href */
    href: string;
    /** Anchor onClick callback */
    onClick?: () => void;
    /** Anchor accessibility title */
    title?: string;
    attr?: ActionTriggerLinkAttributes & ActionTriggerAttributes;
}
export declare const ActionTriggerLink: React.StatelessComponent<ActionTriggerLinkProps>;
export default ActionTriggerLink;
