/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps, ImageProps } from '../../Attributes';
export interface ImageBackgroundType {
}
export interface ImageBackgroundAttributes {
    container?: DivProps;
    image?: ImageProps;
}
export interface ImageBackgroundProps extends React.Props<ImageBackgroundType> {
    /** Background image url */
    src: string;
    alt?: string;
    title?: string;
    /**
     * Fixed width and height (284 x ?? pixels)
     *
     * Default: true
     */
    fixed?: boolean;
    /** Classname to append to top level element */
    className?: string;
    attr?: ImageBackgroundAttributes;
}
/**
 * Image background for `GalleryCard`
 *
 * Should usually be marked as `fixed`, otherwise it will have no dimensions
 *
 * @param props Control properties (Defined in `ImageBackgroundProps` interface)
 */
export declare const ImageBackground: React.StatelessComponent<ImageBackgroundProps>;
export default ImageBackground;
