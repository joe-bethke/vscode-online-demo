/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps } from '../../Attributes';
export interface IconBackgroundType {
}
export interface IconBackgroundAttributes {
    container?: DivProps;
}
export interface IconBackgroundProps extends React.Props<IconBackgroundType> {
    /** Background color of circle */
    backgroundColor: string;
    /** Circle diameter in pixels */
    diameter: number;
    /** Center vertically and horizontally in parent element */
    centered?: boolean;
    /** Classname to append to top level element */
    className?: string;
    attr?: IconBackgroundAttributes;
}
/**
 * Background circle for Icons
 *
 * @param props Control properties (Defined by `IconBackgroundProps` interface)
 */
export declare const IconBackground: React.StatelessComponent<IconBackgroundProps>;
export default IconBackground;
