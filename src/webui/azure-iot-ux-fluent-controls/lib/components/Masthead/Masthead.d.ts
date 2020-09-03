/// <reference types="@types/react" />
import * as React from 'react';
import { MethodNode } from '../../Common';
import * as InlinePopup from '../InlinePopup';
import { NavigationProperties } from '../Navigation/Navigation';
import { ActionTriggerAttributes, ActionTriggerButtonAttributes } from '../ActionTrigger';
import { TextInputAttributes } from '../Input/TextInput';
export interface MastheadToolbarItem {
    icon: string;
    label: string;
    selected: boolean;
    onClick: React.EventHandler<any>;
    attr?: ActionTriggerButtonAttributes & ActionTriggerAttributes;
}
export interface MastheadSearchItem {
    /** The placeholder text for the search input */
    label: string;
    /** The user input value */
    value: string;
    /**
     * For small screen sizes, the search input is collapsed and replaced with a
     * toolbar button. When the button is clicked, the input is shown and occupies
     * the full width of the masthead. This field controls whether or not the full
     * width expanded view is shown.
     */
    expanded?: boolean;
    /** Event handler to call when the search input should be submitted. */
    onSubmit: React.EventHandler<any>;
    /** Event handler to call when the search `value` should be changed. */
    onChange: (newValue: string) => void;
    /** Event handler to call when the `expanded` property  should be toggled. */
    onExpand: React.EventHandler<any>;
    attr?: TextInputAttributes;
}
export interface MastheadUserItem {
    onMenuClick?: React.EventHandler<any>;
    menuExpanded?: boolean;
    menuItems?: MethodNode;
    thumbnailUrl?: string;
    displayName: string;
    email: string;
    attr?: InlinePopup.Attributes;
}
export interface MastheadProperties {
    branding: MethodNode;
    logo?: MethodNode;
    navigation?: NavigationProperties;
    search?: MastheadSearchItem;
    more?: {
        selected: boolean;
        onClick: React.EventHandler<any>;
        title: string;
        attr?: InlinePopup.Attributes;
    };
    toolbarItems?: Array<MastheadToolbarItem>;
    user?: React.ReactNode;
}
export declare class Masthead extends React.PureComponent<MastheadProperties> {
    getToolbarItems: () => JSX.Element[];
    render(): JSX.Element;
}
export default Masthead;
