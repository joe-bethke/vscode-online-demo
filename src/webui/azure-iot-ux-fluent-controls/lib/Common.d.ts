/// <reference types="@types/react" />
import * as React from 'react';
export declare type MethodNode = React.ReactElement<any> | Array<React.ReactElement<any>> | React.ReactChildren | React.ReactNode;
export declare enum DateFormat {
    MMDDYYYY = 0,
    DDMMYYYY = 1,
    YYYYMMDD = 2
}
export interface LabelOption {
    /** Text label to show */
    label: MethodNode;
    /** Label be hidden */
    hidden?: boolean;
    /** Label be disabled */
    disabled?: boolean;
}
export interface FormOption extends LabelOption {
    /** Value of select box option */
    value: any;
}
export interface LinkOption extends LabelOption {
    /** Anchor href */
    href: string;
    /** Anchor onclick */
    onClick?: (event: any) => void;
    /** Accessibility title */
    title?: string;
}
export interface PivotOption extends LinkOption {
    /** Pivot item icon */
    icon?: string;
    /** Pivot key (used for selecting active Pivot) */
    key: string;
}
export declare const keyCode: KeyCode;
export interface KeyCode {
    backspace: number;
    tab: number;
    enter: number;
    shift: number;
    ctrl: number;
    alt: number;
    escape: number;
    space: number;
    left: number;
    up: number;
    right: number;
    down: number;
    pageup: number;
    pagedown: number;
    end: number;
    home: number;
    num0: number;
    num9: number;
    numpad0: number;
    numpad9: number;
    slash: number;
    period: number;
    comma: number;
    dash: number;
    firefoxDash: number;
    f1: number;
}
export declare const weekLength: number;
export declare const hasClassName: (target: any, className: any) => boolean;
export declare const dateIsValid: (date: Date, localTimezone: boolean) => boolean;
export declare class MethodDate {
    localTimezone: boolean;
    dateObject: Date;
    constructor(localTimezone: boolean, year?: number, month?: number, date?: number, hours?: number, minutes?: number, seconds?: number);
    static fromDate(localTimezone: boolean, date: Date): MethodDate;
    static fromString(localTimezone: boolean, dateString: string): MethodDate;
    isBefore(date: MethodDate): boolean;
    copy(): MethodDate;
    toDate(localTimezone?: boolean): Date;
    isValid(): boolean;
    year: number;
    month: number;
    date: number;
    hours: number;
    minutes: number;
    seconds: number;
    milliseconds: number;
}
export declare type SortDirection = null | 'ascending' | 'descending';
export interface GridColumn<T> {
    /** Label for this column */
    label: MethodNode;
    /** Function mapping the row type T to a value to display for this column */
    mapColumn: ((row: T) => MethodNode) | keyof T;
    /** Callback for when the column is sorted in ascending order */
    onAscending?: () => void;
    /** Callback for when the column is sorted in descending order */
    onDescending?: () => void;
    /** Direction to sort when the column is first clicked */
    defaultDirection?: SortDirection;
    /**
     * Width style to set on column div
     *
     * If a number is provided, it is used as a pixel value for 'flex-basis'
     *
     * If a string is provided, it is used as the value for 'flex'
     */
    width?: number;
}
export declare const autoFocusRef: (e: HTMLElement) => void;
