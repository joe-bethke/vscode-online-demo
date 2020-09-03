/// <reference types="@types/react" />
import * as React from 'react';
export interface HorizontalLoaderType {
}
export interface HorizontalLoaderProps extends React.Props<HorizontalLoaderType> {
    /** Icon name (from Segoe UI MDL font) */
    dots?: number;
    /** Classname to append to top level element */
    className?: string;
}
/**
 * HorizontalLoader showing Information, Warning, or Error with text, icon, and optional close button
 *
 * @param props Control properties (defined in `HorizontalLoaderProps` interface)
 */
export declare const HorizontalLoader: React.StatelessComponent<HorizontalLoaderProps>;
export default HorizontalLoader;
