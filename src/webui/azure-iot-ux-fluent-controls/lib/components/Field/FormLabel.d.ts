/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps, LabelProps } from '../../Attributes';
import { MethodNode } from '../../Common';
import { IconAttributes } from '../Icon';
import { BalloonAlignment, BalloonPosition, BalloonAttributes } from '../Balloon';
export interface FormLabelType {
}
export interface FormLabelAttributes {
    container?: DivProps;
    innerContainer?: DivProps;
    farSideContainer?: DivProps;
    text?: LabelProps;
    icon?: IconAttributes;
    balloon?: BalloonAttributes;
}
export interface FormLabelProps extends React.Props<FormLabelType> {
    /** HTML element name for label accessibility */
    name: string;
    /** Form field is required (appends a red asterisk to the label) */
    required?: boolean;
    /**
     * Icon to show to the right of the label
     *
     * default: 'info'
     */
    icon?: string;
    /**
     * Help balloon to show when the user mouses over the icon
     *
     * If this property is provided, the icon provided becomes visible and
     * defaults to the 'info' icon if props.icon is empty
     */
    balloon?: MethodNode;
    /**
     * Where to display Balloon relative to child element
     *
     * `BalloonPosition.[Top | Bottom | Left | Right]`
     *
     * Default: BalloonPosition.Top
     */
    balloonPosition?: BalloonPosition;
    /**
     * Alignment of Balloon relative to child
     *
     * `BalloonAlignment.[Start | Center | End]`
     *
     * Default: BalloonAllignment.Center
     */
    balloonAlignment?: BalloonAlignment;
    /**
     * force balloon to be visible
     */
    balloonExpanded?: boolean;
    /** Classname to append to top level element */
    className?: string;
    /** Extra node to render at the far side of the label */
    farSide?: React.ReactNode;
    attr?: FormLabelAttributes;
}
export declare const requiredClassName: string;
/**
 * High level generic form field
 *
 * @param props Control properties (defined in `FormLabelProps` interface)
 */
export declare const FormLabel: React.StatelessComponent<FormLabelProps>;
export default FormLabel;
