/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps } from '../../Attributes';
import { MethodNode } from '../../Common';
export interface GalleryCardType {
}
export interface GalleryCardAttributes {
    container?: DivProps;
    content?: DivProps;
    banner?: DivProps;
}
export interface GalleryCardProps extends React.Props<GalleryCardType> {
    /**
     * Element to display as `GalleryCard` background
     *
     * Default: Solid background with color #eaeaea
     * */
    background?: MethodNode;
    /** Banner string to display above the `GalleryCard` background */
    banner?: string;
    /**
     * Fixed width and height (284 pixels)
     *
     * Default: true
     */
    fixed?: boolean;
    /** Classname to append to top level element */
    className?: string;
    /** Classname to append to content element */
    contentClassName?: string;
    /** Data test hook string for testing */
    dataTestHook?: string;
    attr?: GalleryCardAttributes;
}
/**
 * Gallery Card control
 *
 * You should usually mark this control as `fixed` because the container
 * element gets its width from its content like the background and children
 *
 * @param props Control properties (Defined in `GalleryCardProps` interface)
 */
export declare const GalleryCard: React.StatelessComponent<GalleryCardProps>;
export interface BannerType {
}
export interface BannerAttributes {
    container?: DivProps;
}
export interface BannerProps extends React.Props<BannerType> {
    className?: string;
    attr?: BannerAttributes;
}
/** TODO: Remove this Banner control. GalleryCard banner is now a string */
export declare const Banner: React.StatelessComponent<BannerProps>;
export default GalleryCard;
