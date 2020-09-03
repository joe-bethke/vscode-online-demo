/// <reference types="@types/react" />
import * as React from 'react';
export interface SpinnerType {
}
export interface SpinnerProps extends React.Props<SpinnerType> {
    /** Classname to append to top level element */
    className?: string;
}
/**
 * Spinner showing Information, Warning, or Error with text, icon, and optional close button
 *
 * @param props Control properties (defined in `SpinnerProps` interface)
 */
export declare const Spinner: (props: SpinnerProps) => JSX.Element;
export default Spinner;
