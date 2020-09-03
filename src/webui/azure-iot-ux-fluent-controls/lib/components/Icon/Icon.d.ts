/// <reference types="@types/react" />
import * as React from 'react';
import { SpanProps } from '../../Attributes';
export declare enum IconSize {
    xsmall = 1,
    small = 2,
    medium = 3,
    large = 4,
    xlarge = 5,
    xxlarge = 6
}
export interface IconType {
}
export interface IconAttributes {
    container?: SpanProps;
    label?: SpanProps;
}
export interface IconProps extends React.Props<IconType> {
    /** Icon name (from icons.css) */
    icon: string;
    /**
     * Icon font size as defined by `IconSize` enum
     *
     * `IconSize.[xsmall | small | medium | large | xlarge | xxlarge]`
     *
     * Starts at 16 pixels (`IconSize.xsmall`) and increases 16 pixels at a
     * time until 96 pixels (`IconSize.xxlarge`)
     *
     * Defaults: `IconSize.medium` (48x48 pixels)
     */
    size?: IconSize;
    /**
     * Icon font size
     *
     * Overrides `IconProps.size`
     */
    fontSize?: number;
    /** Icon color (accepts string color names and RGB hex values) */
    color?: string;
    /** Center vertically and horizontally in parent element */
    centered?: boolean;
    /** Classname to append to top level element */
    className?: string;
    /**
     * Classname for Icon label
     *
     * Even with props.className getting CSS specificity right to modify font-
     * size of the label is problematic.
     */
    labelClassName?: string;
    attr?: IconAttributes;
}
/**
 * Icon loaded from Segoe UI MDL icons font
 *
 * Renders children so this control can be used with text
 *
 * @param props Control properties (Defined in `IconProps` interface)
 */
export declare const Icon: React.StatelessComponent<IconProps>;
export default Icon;
