/// <reference types="@types/react" />
import * as React from 'react';
interface Properties {
    children?: React.ReactNode;
}
interface State {
    container?: HTMLElement;
}
export declare class Portal extends React.Component<Properties, State> {
    constructor(props: Properties);
    componentDidMount(): void;
    render(): React.ReactPortal;
}
export {};
