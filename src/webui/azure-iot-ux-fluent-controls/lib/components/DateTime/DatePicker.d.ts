/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps, InputProps } from '../../Attributes';
import { CalendarAttributes } from './Calendar';
import { MethodDate, DateFormat } from '../../Common';
import { ActionTriggerButtonAttributes } from '../ActionTrigger';
export interface DatePickerType {
}
export interface DatePickerAttributes {
    inputContainer?: DivProps;
    input?: InputProps;
    inputIcon?: ActionTriggerButtonAttributes;
    calendar?: CalendarAttributes;
    container?: DivProps;
}
export interface DatePickerProps extends React.Props<DatePickerType> {
    /** HTML form element name */
    name: string;
    /**
     * Initial value of date picker
     *
     * The onChange callback API does not receives invalid Date UTC ISO strings
     * so we can only provide an initialValue to the DatePicker
     */
    initialValue?: Date | string;
    /** Tab index for calendar control */
    tabIndex?: number;
    /** Apply error styling to input element */
    error?: boolean;
    /** Add required attribute to HTML input element */
    required?: boolean;
    /** Disable HTML input element and apply disabled styling */
    disabled?: boolean;
    /**
     * Display the date in local timezone instead of GMT
     *
     * Default: true
     */
    localTimezone?: boolean;
    /** i18n locale */
    locale?: string;
    /**
     * Show Calendar below date picker input
     */
    showAbove?: boolean;
    /** Date format in text input */
    format?: DateFormat;
    /**
     * Callback for HTML input element `onChange` events
     *
     * When the user enters a valid date, onChange receives a UTC ISO string.
     *
     * When the string value in the text input is not a valid date, onChange
     * receives the string "invalid"
     */
    onChange: (newValue: string) => void;
    /**
     * Callback for paste events
     *
     * When the user pastes a valid date, onPaste receives a UTC ISO string.
     */
    onPaste?: (newValue: string) => void;
    /**
     * callback for clicking the calendar icon.
     */
    onExpand?: (expanded: boolean) => void;
    /** Class to append to top level element */
    className?: string;
    attr?: DatePickerAttributes;
}
export interface DatePickerState {
    value: string;
    initialValue?: MethodDate;
    expanded?: boolean;
}
/**
 * Low level date picker control
 *
 * (Use the `DateField` control instead when making a form with standard styling)
 */
export declare class DatePicker extends React.Component<DatePickerProps, Partial<DatePickerState>> {
    static defaultProps: {
        format: DateFormat;
        tabIndex: number;
        localTimezone: boolean;
        showAbove: boolean;
        attr: {
            container: {};
            inputContainer: {};
            input: {};
            inputIcon: {};
            calendar: {};
        };
    };
    /**
     * This variable tracks whether the user has copy pasted a value into the
     * text input. If a value is pasted into the DatePicker half of a DateTimeField,
     * tracking whether something was pasted allows the DateTimeField to set the
     * TimeInput to the pasted value. This also allows turning off regular parsing
     * if the pasted string is malformed to give the user a chance to correct it
     */
    private paste;
    private input;
    private _containerRef;
    oldSetState: any;
    constructor(props: DatePickerProps);
    /**
     * Use props.initialValue to generate a new state
     *
     * props.initialValue is used to set the hours/minutes/seconds on internal Date
     *
     * @param props DatePickerProps
     */
    getInitialState(props: DatePickerProps, currentValue: string): DatePickerState;
    /**
     * Update the Date/Time object used internally with a new initialValue
     *
     * @param newProps new DatePickerProps
     */
    componentWillReceiveProps(newProps: DatePickerProps): void;
    componentDidMount(): void;
    componentWillUnmount(): void;
    parse(newValue: string): {
        year: any;
        month: any;
        date: any;
        valid: boolean;
    };
    inputRef: (element: HTMLInputElement) => HTMLInputElement;
    onChange: (event: any) => void;
    onExpand: () => void;
    onSelect: (newValue: Date) => void;
    onPaste: () => void;
    onOuterMouseEvent: (e: MouseEvent) => void;
    onKeydown: (e: KeyboardEvent) => void;
    onBlur: (e: React.FocusEvent<any>) => void;
    setContainerRef: (element: HTMLElement) => void;
    render(): JSX.Element;
}
export default DatePicker;
