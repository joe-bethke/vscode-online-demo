/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps, ButtonProps } from '../../Attributes';
import { ActionTriggerButtonAttributes } from '../ActionTrigger/ActionTriggerButton';
import { MethodDate } from '../../Common';
export interface CalendarComponentType {
}
export interface CalendarAttributes {
    container?: DivProps;
    header?: DivProps;
    monthHeader?: DivProps;
    prevMonthButton?: ActionTriggerButtonAttributes;
    nextMonthButton?: ActionTriggerButtonAttributes;
    weekDayHeader?: DivProps;
    dateContainer?: DivProps;
    dateButton?: ButtonProps;
    dateRow?: DivProps;
}
export interface CalendarProps extends React.Props<CalendarComponentType> {
    /** Current selected date */
    value?: Date | string;
    /** i18n locale */
    locale?: string;
    /** Year to display (otherwise shows the year from value) */
    year?: number;
    /** Month to display (otherwise shows the month from value) */
    month?: number;
    /**
     * Treat the Date object with the local timezone
     *
     * Default: true
     */
    localTimezone?: boolean;
    /**
     * Callback for date change events
     * */
    onChange: (newValue: Date) => void;
    /** Classname to append to top level element */
    className?: string;
    attr?: CalendarAttributes;
}
export interface CalendarState {
    /** Date of the current month open in view */
    currentDate: MethodDate;
    /** Whether or not props.year/month updates update the view */
    detached: boolean;
}
/**
 * Calendar control
 *
 * @param props Control properties (defined in `CalendarProps` interface)
 */
export declare class Calendar extends React.Component<CalendarProps, Partial<CalendarState>> {
    static defaultProps: {
        localTimezone: boolean;
        attr: {
            container: {};
            header: {};
            monthHeader: {};
            prevMonthButton: {};
            nextMonthButton: {};
            weekDayHeader: {};
            dateContainer: {};
            dateButton: {};
            dateRow: {};
        };
    };
    private value;
    private monthNames;
    private dayNames;
    private _container;
    private nextFocusRow?;
    private nextFocusCol?;
    constructor(props: CalendarProps);
    componentWillReceiveProps(newProps: CalendarProps): void;
    componentDidUpdate(): void;
    onClick(date: MethodDate): void;
    onPrevMonth(event: any): void;
    onNextMonth(event: any): void;
    decrementMonth(): void;
    incrementMonth(): void;
    onKeyDown(e: React.KeyboardEvent<any>): void;
    setContainerRef(element: HTMLDivElement): void;
    render(): JSX.Element;
}
export default Calendar;
