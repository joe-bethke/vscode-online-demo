/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps, OptionProps, SelectProps } from '../../Attributes';
export interface TimeInputType {
}
export interface TimeInputAttributes {
    container?: DivProps;
    hourSelect?: SelectProps;
    hourOption?: OptionProps;
    minuteSelect?: SelectProps;
    minuteOption?: OptionProps;
    secondSelect?: SelectProps;
    secondOption?: OptionProps;
    periodSelect?: SelectProps;
    periodOption?: OptionProps;
}
export interface TimeInputProps extends React.Props<TimeInputType> {
    /** HTML element name for label accessibility */
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
    /** Apply error styling */
    error?: boolean;
    /** Disable input */
    disabled?: boolean;
    /** Callback for new time input values */
    onChange: (time: string) => void;
    /** Classname to append to top level element */
    className?: string;
    /** Classname to append to HTML input elements */
    inputClassName?: string;
    attr?: TimeInputAttributes;
}
export interface TimeInputState {
    hours: number;
    minutes: number;
    seconds: number;
    period: 'AM' | 'PM' | '24H';
}
/**
 * High level generic form field
 *
 * @param props Control properties (defined in `TimeInputProps` interface)
 */
export declare class TimeInput extends React.Component<TimeInputProps, TimeInputState> {
    static defaultProps: {
        showSeconds: boolean;
        militaryTime: boolean;
        disabled: boolean;
        localTimezone: boolean;
        amLabel: string;
        pmLabel: string;
        attr: {
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
    private hours;
    private minutes;
    private seconds;
    private hourInput;
    private minuteInput;
    private secondInput;
    private periodInput;
    constructor(props: TimeInputProps);
    handleState(props: TimeInputProps): TimeInputState;
    handleTimezone(value: string | Date): Date;
    componentWillReceiveProps(newProps: any): void;
    update(name: 'hours' | 'minutes' | 'seconds' | 'period', value: string | number, period?: 'AM' | 'PM' | '24H'): void;
    render(): JSX.Element;
}
export default TimeInput;
