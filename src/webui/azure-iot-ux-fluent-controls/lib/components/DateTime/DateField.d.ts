/// <reference types="@types/react" />
import * as React from 'react';
import { MethodNode, DateFormat } from '../../Common';
import { DatePickerAttributes } from './DatePicker';
import { FormFieldAttributes } from '../Field/FormField';
export interface DateFieldType {
}
export interface DateFieldProps extends React.Props<DateFieldType> {
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
    /** Label to display above input element */
    label: MethodNode;
    /** Error to display below input element */
    error?: MethodNode;
    /** Error HTML title in case of overflow */
    errorTitle?: string;
    /** Disable HTML input element */
    disabled?: boolean;
    /** Form field is required (appends a red asterisk to the label) */
    required?: boolean;
    /** Display horizontal loading animation instead of error */
    loading?: boolean;
    /** Tooltip text to display in info icon bubble */
    tooltip?: MethodNode;
    /** Callback for HTML input element `onChange` events */
    onChange: (newValue: string) => void;
    /** Classname to append to top level element */
    className?: string;
    /** Classname to append to top level element of TextInput */
    inputClassName?: string;
    /**
     * callback for clicking the calendar icon
     */
    onExpand?: (expanded: boolean) => void;
    /** React node to render at the far side of the label. */
    labelFarSide?: React.ReactNode;
    attr?: DatePickerAttributes & FormFieldAttributes;
}
/**
 * High level form text field
 *
 * @param props Control properties (defined in `DateFieldProps` interface)
 */
export declare const DateField: React.StatelessComponent<DateFieldProps>;
export default DateField;
