/// <reference types="@types/react" />
import * as React from 'react';
import { MethodNode } from '../../Common';
import { TimeInputAttributes } from './TimeInput';
import { FormFieldAttributes } from '../Field/FormField';
export interface TimeFieldType {
}
export interface TimeFieldProps extends React.Props<TimeFieldType> {
    /** HTML form element name */
    name: string;
    /** Value */
    value?: string | Date;
    /** Label for "AM" select option */
    amLabel?: string;
    /** Label for "PM" select option */
    pmLabel?: string;
    /**
     * Show the time in the local timezone instead of GMT
     *
     * Default: true
     */
    localTimezone?: boolean;
    /** Display the seconds dropdown */
    showSeconds?: boolean;
    /** Use 24 hour clock */
    militaryTime?: boolean;
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
    /** React node to render at the far side of the label. */
    labelFarSide?: React.ReactNode;
    attr?: TimeInputAttributes & FormFieldAttributes;
}
/**
 * High level form text field
 *
 * @param props Control properties (defined in `TimeFieldProps` interface)
 */
export declare const TimeField: React.StatelessComponent<TimeFieldProps>;
export default TimeField;
