/// <reference types="@types/react" />
import * as React from 'react';
export interface Attributes {
    tabIndex?: number;
    dataTestHook?: string;
    ariaLabel?: string;
    ariaDescribedBy?: string;
}
export interface Properties {
    expanded?: boolean;
    onClick?: React.EventHandler<any>;
    className?: string;
    scrollIntoView?: boolean;
    alignment?: 'left' | 'right';
    disabled?: boolean;
    attr?: Attributes;
}
export declare class Container extends React.PureComponent<Properties> {
    static defaultProps: Partial<Properties>;
    render(): JSX.Element;
}
export declare class Label extends React.Component<Properties & {
    title?: string;
}> {
    static defaultProps: Partial<Properties>;
    render(): JSX.Element;
}
export declare class Panel extends React.Component<Properties> {
    static defaultProps: Partial<Properties>;
    rawElement: HTMLElement;
    componentDidUpdate(): void;
    render(): JSX.Element;
}
