/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps, AnchorProps, OptionAttr } from '../../Attributes';
import { PivotOption } from '../../Common';
import { PivotAttributes } from './Pivot';
export interface PivotMenuType {
}
export interface PivotMenuAttributes {
    container?: DivProps;
    anchor?: AnchorProps;
    pivot?: PivotAttributes;
}
export interface PivotMenuProps extends React.Props<PivotMenuType> {
    links: (PivotOption & OptionAttr<{
        anchor?: AnchorProps;
    } & PivotAttributes>)[];
    active?: string;
    tabIndex?: number;
    className?: string;
    anchorClassName?: string;
    pivotClassName?: string;
    attr?: PivotMenuAttributes;
}
export declare const PivotMenu: React.StatelessComponent<PivotMenuProps>;
export default PivotMenu;
