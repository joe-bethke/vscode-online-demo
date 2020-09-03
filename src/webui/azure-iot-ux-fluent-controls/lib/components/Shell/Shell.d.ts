/// <reference types="@types/react" />
import * as React from 'react';
import { MastheadProperties } from '../Masthead/Masthead';
import { NavigationProperties } from '../Navigation/Navigation';
export { MastheadProperties, MastheadSearchItem, MastheadToolbarItem, MastheadUserItem } from '../Masthead/Masthead';
export { NavigationProperties, NavigationItemSeparator } from '../Navigation/Navigation';
/** Root theme that will be passed thru the control tree. */
export interface ShellTheme {
    base: string;
    colorBgMasthead?: string;
    colorBgMastheadHover?: string;
    colorBgMastheadDisabled?: string;
    colorTextMastheadRest?: string;
    colorTextMastheadDisabled?: string;
    colorBgBtnPrimaryRest?: string;
    colorBgBtnPrimaryHover?: string;
    colorBgBtnPrimaryDisabled?: string;
    colorTextBtnPrimaryRest?: string;
    colorTextBtnPrimaryDisabled?: string;
    colorBgBtnStandardRest?: string;
    colorBgBtnStandardHover?: string;
    colorBgBtnStandardDisabled?: string;
    colorTextBtnStandardRest?: string;
    colorTextBtnStandardDisabled?: string;
}
export interface ShellProperties {
    theme?: string | ShellTheme;
    isRtl?: boolean;
    masthead?: MastheadProperties;
    navigation?: NavigationProperties;
    children?: React.ReactNode;
    onClick?: React.MouseEventHandler<HTMLDivElement>;
}
export declare function Shell({ theme, isRtl, masthead, navigation, children, onClick }: ShellProperties): JSX.Element;
export default Shell;
