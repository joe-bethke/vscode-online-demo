/// <reference types="@types/react" />
import * as React from 'react';
import { ButtonProps, NavProps } from '../../Attributes';
export interface NavigationAttributes {
    container?: NavProps;
    navButton?: ButtonProps;
}
export interface NavigationProperties {
    isExpanded: boolean;
    onClick: React.EventHandler<any>;
    attr?: NavigationAttributes;
    children?: React.ReactNode;
}
export declare function Navigation({ isExpanded, onClick, attr, children }: NavigationProperties): JSX.Element;
export declare function NavigationItemSeparator(): JSX.Element;
export default Navigation;
