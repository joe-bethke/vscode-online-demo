/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps, SpanProps } from '../../Attributes';
import { MethodNode } from '../../Common';
export interface DropdownType {
}
export declare enum DropdownPosition {
    Top = 1,
    Bottom = 2,
    Left = 3,
    Right = 4
}
export declare enum DropdownAlignment {
    Start = 1,
    Center = 2,
    End = 3
}
export interface DropdownAttributes {
    container?: SpanProps;
    dropdownContainer?: DivProps;
    dropdown?: SpanProps;
}
export interface DropdownProps extends React.Props<DropdownType> {
    /** Contents of dropdown */
    dropdown: MethodNode;
    /** Shows the dropdown */
    visible?: boolean;
    onMouseEnter?: (event: any) => void;
    onMouseLeave?: (event: any) => void;
    positionClassNames?: string[];
    outerEvents?: string[];
    onOuterEvent?: (event: any) => void;
    /** Classname to append to top level element */
    className?: string;
    attr?: DropdownAttributes;
}
export interface DropdownState {
    positionIndex?: number;
}
/**
 * SimpleDropdown shows tooltip (with HTML) on hover over child
 *
 * NOTE: If a parent element of this control is `overflow: hidden` then the
 * balloon may not show up.
 *
 * @param props Control properties (defined in `SimpleDropdownProps` interface)
 */
export declare class Dropdown extends React.Component<DropdownProps, DropdownState> {
    static defaultProps: {
        tooltip: any;
        position: DropdownPosition;
        align: DropdownAlignment;
        visible: boolean;
        positionClassNames: any[];
        attr: {
            container: {};
            dropdownContainer: {};
            dropdown: {};
        };
    };
    private dropdown;
    private container;
    private fixedContainer;
    private dropdownOffset;
    private previousPosition;
    private dropdownDirection;
    private animationRequest;
    private eventsConnected;
    private positionFailed;
    private positionReset;
    private scrollOffset;
    private mouseX;
    private mouseY;
    private unmounting;
    constructor(props: DropdownProps);
    dropdownRef: (element: any) => any;
    containerRef: (element: any) => any;
    fixedRef: (element: any) => any;
    getPositionIndex(): number;
    componentWillReceiveProps(): void;
    componentWillUpdate(): void;
    componentDidMount(): void;
    componentDidUpdate(oldProps: DropdownProps, oldState: DropdownState): void;
    componentWillUnmount(): void;
    onChange: (event: any) => void;
    saveScrollOffset(): void;
    loadScrollOffset(): void;
    interactsWithDropdown(event: any, depth?: number): boolean;
    handleOuterEvent: (event: any) => void;
    /**
     * Get the dimensions and position of the dropdown element while it is
     * still in the DOM rendered by this component. This information is used
     * to position the dropdown when it is moved to the `display: fixed` element.
     *
     * The position is relative to the top-left corner of the container element,
     * calculated based on both elements' position relative to the user's window.
     */
    getDropdownOffset(): {
        top: number;
        left: number;
        width: number;
        height: number;
    };
    updateEventHandlers(): void;
    repositionDropdown(): void;
    render(): JSX.Element;
}
export default Dropdown;
