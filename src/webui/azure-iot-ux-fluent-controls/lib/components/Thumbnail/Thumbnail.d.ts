/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps, ImageProps } from '../../Attributes';
/**
 * Scalable thumbnail for a product or a device
 */
export interface ThumbnailProperties {
    loading?: boolean;
    url?: string;
    /** The icon to display. */
    icon?: string;
    /**
     * The kind of thumbnail icon.
     * @deprecated use `icon` instead
     */
    kind?: 'product' | 'device' | 'user' | 'unknown' | 'missing';
    className?: string;
    size?: 'preview' | 'masthead' | 'list-item' | 'list-tile' | 'search-result';
    ariaLabel?: string;
    attr?: {
        container?: DivProps;
        img?: ImageProps;
    };
}
/**
 * Object that tracks the current state of the toolbar. Since we're just tracking
 * one in-memory boolean flag that doesn't affect anything outside this component,
 * we can just use React's setState instead of creating a new Store for this.
 */
export interface ThumbnailState {
    /** Flag that is set when the browser has finished loading the image */
    imageLoaded: boolean;
}
export declare class Thumbnail extends React.Component<ThumbnailProperties, ThumbnailState> {
    private imgRef;
    constructor(props: ThumbnailProperties);
    render(): JSX.Element;
    private handleError;
    private handleImageLoad;
}
export default Thumbnail;
