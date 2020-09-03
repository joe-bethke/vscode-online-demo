/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps, SpanProps } from '../../Attributes';
import { MethodNode } from '../../Common';
import { IconAttributes } from '../Icon';
export declare const pivotClassName: string;
export declare const menuClassName: string;
export interface PivotType {
}
export interface PivotAttributes {
    container?: DivProps;
    bottomBorder?: DivProps;
    focusBorder?: DivProps;
    content?: SpanProps;
    innerContent?: DivProps;
    icon?: IconAttributes;
}
export interface PivotProps extends React.Props<PivotType> {
    icon?: string;
    text: string | MethodNode;
    selected?: boolean;
    disabled?: boolean;
    className?: string;
    attr?: PivotAttributes;
}
export declare const Pivot: React.StatelessComponent<PivotProps>;
export default Pivot;
