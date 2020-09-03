/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps, SpanProps } from '../../Attributes';
import { MethodNode } from '../../Common';
export interface BalloonType {
}
export declare enum BalloonPosition {
    Top = 1,
    Bottom = 2,
    Left = 3,
    Right = 4
}
export declare enum BalloonAlignment {
    Start = 1,
    Center = 2,
    End = 3
}
export interface BalloonAttributes {
    container?: SpanProps;
    balloonContainer?: DivProps;
    balloon?: SpanProps;
    balloonContent?: SpanProps;
}
export interface BalloonProps extends React.Props<BalloonType> {
    /** Contents of balloon */
    tooltip: MethodNode;
    /**
     * Where to display Balloon relative to child element
     *
     * `BalloonPosition.[Top | Bottom | Left | Right]`
     *
     * Default: BalloonPosition.Top
     */
    position?: BalloonPosition;
    /**
     * Alignment of Balloon relative to child
     *
     * `BalloonAlignment.[Start | Center | End]`
     *
     * Default: BalloonAllignment.Center
     */
    align?: BalloonAlignment;
    /**
     * Allow Balloon contents to span multiple lines
     *
     * default: true
     */
    multiline?: boolean;
    /**
     * Allow balloon to reposition itself if it isn't completely visible
     *
     * default: true
     */
    autoPosition?: boolean;
    /** Classname to append to top level element */
    className?: string;
    /** Classname to append to inner balloon element */
    balloonClassName?: string;
    /** Forces the balloon to be expanded */
    expanded?: boolean;
    attr?: BalloonAttributes;
}
export interface BalloonState {
    hovered?: boolean;
    visible?: boolean;
    position?: BalloonPosition;
    align?: BalloonAlignment;
}
/**
 * SimpleBalloon shows tooltip (with HTML) on hover over child
 *
 * NOTE: If a parent element of this control is `overflow: hidden` then the
 * balloon may not show up.
 *
 * @param props Control properties (defined in `SimpleBalloonProps` interface)
 */
export declare class Balloon extends React.Component<BalloonProps, BalloonState> {
    static defaultProps: {
        tooltip: any;
        position: BalloonPosition;
        align: BalloonAlignment;
        expanded: boolean;
        multiline: boolean;
        autoPosition: boolean;
        attr: {
            container: {};
            balloonContainer: {};
            balloon: {};
        };
    };
    constructor(props: BalloonProps);
    componentWillReceiveProps(newProps: BalloonProps): void;
    shouldComponentUpdate(newProps: BalloonProps, newState: BalloonState): boolean;
    onMouseEnter: (event: any) => void;
    onMouseLeave: (event: any) => void;
    getClassName(reverse: boolean): string;
    render(): JSX.Element;
}
export default Balloon;
