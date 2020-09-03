/// <reference types="@types/react" />
import * as React from 'react';
import { ActionTriggerButtonAttributes, ActionTriggerAttributes } from '../ActionTrigger';
import { DivProps } from '../../Attributes';
export interface ContextPanelProperties {
    onClose: React.EventHandler<any>;
    omitPortal?: boolean;
    header: React.ReactNode;
    children?: React.ReactNode;
    footer?: React.ReactNode;
    attr?: {
        container?: DivProps;
        header?: DivProps;
        content?: DivProps;
        footer?: DivProps;
        closeButton?: ActionTriggerButtonAttributes & ActionTriggerAttributes;
    };
}
export declare function ContextPanel(props: ContextPanelProperties): JSX.Element;
export default ContextPanel;
