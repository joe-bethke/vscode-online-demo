/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps } from '../../Attributes';
export interface SolidBackgroundType {
}
export interface SolidBackgroundAttributes {
    container?: DivProps;
}
export interface SolidBackgroundProps extends React.Props<SolidBackgroundType> {
    /**
     * Background color (accepts string color names and RGB hex values)
     *
     * Default: #eaeaea
     */
    backgroundColor?: string;
    /**
     * Fixed width and height (284 x ?? pixels)
     *
     * Default: true
     */
    fixed?: boolean;
    /** Classname to append to top level element */
    className?: string;
    attr?: SolidBackgroundAttributes;
}
/**
 * Solid color background for `GalleryCard`
 *
 * Should usually be marked as `fixed`, otherwise it will have no dimensions
 *
 * @param props Control properties (Defined in `ImageBackgroundProps` interface)
 */
export declare const SolidBackground: React.StatelessComponent<SolidBackgroundProps>;
export default SolidBackground;
