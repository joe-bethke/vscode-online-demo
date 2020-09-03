/// <reference types="@types/react" />
import * as React from 'react';
import { MethodNode, DateFormat } from '../../Common';
import { FormFieldAttributes } from '../Field/FormField';
/** This import solves an error with exports of FormFieldAttributes defaults */
import { TimeInputAttributes } from './TimeInput';
import { DatePickerAttributes } from './DatePicker';
import { DivProps, SpanProps } from '../../Attributes';
export interface DateTimeFieldType {
}
export interface DateTimeFieldAttributes {
    datePicker?: DatePickerAttributes;
    timeInput?: TimeInputAttributes;
    flexContainer?: DivProps;
    dateColumn?: SpanProps;
    timeColumn?: SpanProps;
}
export interface DateTimeFieldProps extends React.Props<DateTimeFieldType> {
    /** HTML form element name */
    name: string;
    /** Current value of HTML input element */
    initialValue: string | Date;
    /** Label to display above input element */
    label?: MethodNode;
    /** Error to display below input element */
    error?: MethodNode;
    /** Error HTML title in case of overflow */
    errorTitle?: string;
    /** Date format in text input */
    format?: DateFormat;
    /** Tab index for calendar control */
    tabIndex?: number;
    /** Label for "AM" select option */
    amLabel?: string;
    /** Label for "PM" select option */
    pmLabel?: string;
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
    /** Display the seconds dropdown */
    showSeconds?: boolean;
    /** Use 24 hour clock */
    militaryTime?: boolean;
    /** Disable HTML input element */
    disabled?: boolean;
    /** Form field is required (appends a red asterisk to the label) */
    required?: boolean;
    /** Display horizontal loading animation instead of error */
    loading?: boolean;
    /** Set error field to display: none */
    hideError?: boolean;
    /** Tooltip text to display in info icon bubble */
    tooltip?: MethodNode;
    /** Callback for HTML input element `onChange` events */
    onChange: (newValue: string) => void;
    /** Classname to append to top level element */
    className?: string;
    /** Classname to append to top level element of DatePicker and TimeInput */
    inputClassName?: string;
    /**
     * callback for clicking calendar icon
     */
    onExpand?: (expanded: boolean) => void;
    /** React node to render at the far side of the label. */
    labelFarSide?: React.ReactNode;
    attr?: DateTimeFieldAttributes & FormFieldAttributes;
}
export interface DateTimeFieldState {
    initialDate: string | Date;
    lastDate: string;
    lastTime: string;
}
/**
 * High level date time field
 *
 * @param props Control properties (defined in `DateTimeFieldProps` interface)
 */
export declare class DateTimeField extends React.Component<DateTimeFieldProps, Partial<DateTimeFieldState>> {
    static defaultProps: {
        format: DateFormat;
        tabIndex: number;
        localTimezone: boolean;
        showAbove: boolean;
        showSeconds: boolean;
        militaryTime: boolean;
        attr: {
            fieldContainer: {};
            fieldLabel: {};
            fieldContent: {};
            fieldError: {};
            flexContainer: {};
            dateColumn: {};
            timeColumn: {};
            datePicker: {
                container: {};
                inputContainer: {};
                input: {};
                inputIcon: {};
                calendar: {};
            };
            timeInput: {
                container: {};
                hourSelect: {};
                hourOption: {};
                minuteSelect: {};
                minuteOption: {};
                secondSelect: {};
                secondOption: {};
                periodSelect: {};
                periodOption: {};
            };
        };
    };
    constructor(props: DateTimeFieldProps);
    getInitialState(props: DateTimeFieldProps): DateTimeFieldState;
    componentWillReceiveProps(newProps: DateTimeFieldProps): void;
    onDatePaste: (newDate: string) => boolean;
    onChange: (newDate: string | Date, newTime: string | Date) => Date;
    onDateChange: (newDate: string) => void;
    onTimeChange: (newTime: string) => void;
    render(): JSX.Element;
}
export default DateTimeField;
