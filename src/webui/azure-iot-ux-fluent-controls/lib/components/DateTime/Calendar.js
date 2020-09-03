Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    Attributes_1 = require("../../Attributes"),
    ActionTriggerButton_1 = require("../ActionTrigger/ActionTriggerButton"),
    helpers_1 = require("./helpers"),
    Common_1 = require("../../Common"),
    classNames = require("classnames/bind"),
    css = classNames.bind(require("./Calendar.module.scss"));
/**
 * Calendar control
 *
 * @param props Control properties (defined in `CalendarProps` interface)
 */
class Calendar extends React.Component {
    constructor(props) {
        super(props);
        if (typeof this.props.value === "string") {
            this.value = Common_1.MethodDate.fromString(
                this.props.localTimezone,
                this.props.value
            );
        } else if (this.props.value) {
            this.value = Common_1.MethodDate.fromDate(
                this.props.localTimezone,
                this.props.value
            );
        } else {
            this.value = new Common_1.MethodDate(this.props.localTimezone);
        }
        let currentDate = this.value.copy();
        if (props.year > 0) {
            currentDate.year = props.year;
        }
        if (props.month === 0 || props.month > 0) {
            currentDate.month = props.month;
        }
        currentDate.date = 1;
        this.state = {
            currentDate: currentDate,
            detached: false,
        };
        this.monthNames = helpers_1.getLocalMonths(this.props.locale);
        this.dayNames = helpers_1.getLocalWeekdays(this.props.locale);
        this.onPrevMonth = this.onPrevMonth.bind(this);
        this.onNextMonth = this.onNextMonth.bind(this);
        this.onKeyDown = this.onKeyDown.bind(this);
        this.setContainerRef = this.setContainerRef.bind(this);
    }
    componentWillReceiveProps(newProps) {
        const date = this.state.currentDate.copy();
        let update = false;
        if (newProps.year !== this.props.year && newProps.year > 0) {
            date.year = newProps.year;
            update = true;
        }
        if (
            typeof newProps.month === "number" &&
            newProps.month !== this.props.month &&
            (newProps.month === 0 || newProps.month > 0)
        ) {
            date.month = newProps.month;
            update = true;
        }
        if (update && !this.state.detached && date.isValid()) {
            this.setState({ currentDate: date });
        }
        if (
            this.props.value !== newProps.value ||
            this.props.localTimezone !== newProps.localTimezone
        ) {
            if (typeof newProps.value === "string") {
                this.value = Common_1.MethodDate.fromString(
                    newProps.localTimezone,
                    newProps.value
                );
            } else if (newProps.value) {
                this.value = Common_1.MethodDate.fromDate(
                    newProps.localTimezone,
                    newProps.value
                );
            } else {
                this.value = new Common_1.MethodDate(newProps.localTimezone);
            }
        }
    }
    componentDidUpdate() {
        if (this.nextFocusRow != null && this.nextFocusCol != null) {
            const nextFocus = this._container.querySelectorAll(
                `[data-row="${this.nextFocusRow}"][data-col="${this.nextFocusCol}"]`
            )[0];
            if (nextFocus != null) {
                nextFocus.focus();
            }
            this.nextFocusRow = undefined;
            this.nextFocusCol = undefined;
        }
    }
    onClick(date) {
        if (this.props.onChange) {
            this.props.onChange(date.dateObject);
            this.setState({
                currentDate: Common_1.MethodDate.fromDate(
                    this.props.localTimezone,
                    date.dateObject
                ),
                detached: false,
            });
        }
    }
    onPrevMonth(event) {
        event.preventDefault();
        this.decrementMonth();
    }
    onNextMonth(event) {
        event.preventDefault();
        this.incrementMonth();
    }
    decrementMonth() {
        /** Dates are mutable so we're going to copy it over */
        const newDate = this.state.currentDate.copy(),
            curDate = newDate.date,
            targetMonth = newDate.month === 0 ? 11 : newDate.month - 1;
        newDate.month -= 1;
        if (newDate.month !== targetMonth || newDate.date !== curDate) {
            newDate.date = 1;
            newDate.month = targetMonth + 1;
            newDate.date = 0;
        }
        this.setState({ currentDate: newDate, detached: true });
    }
    incrementMonth() {
        /** Dates are mutable so we're going to copy it over */
        const newDate = this.state.currentDate.copy(),
            curDate = newDate.date,
            targetMonth = newDate.month === 11 ? 0 : newDate.month + 1;
        newDate.month += 1;
        if (newDate.month !== targetMonth || newDate.date !== curDate) {
            newDate.date = 1;
            newDate.month = targetMonth + 1;
        }
        this.setState({ currentDate: newDate, detached: true });
    }
    onKeyDown(e) {
        const element = e.currentTarget,
            row = parseInt(element.getAttribute("data-row"), 10),
            col = parseInt(element.getAttribute("data-col"), 10);
        if (!isNaN(row) && !isNaN(col)) {
            let nextRow = row,
                nextCol = col,
                nextFocus;
            switch (e.keyCode) {
                case Common_1.keyCode.pagedown:
                    e.preventDefault();
                    e.stopPropagation();
                    this.nextFocusCol = nextCol;
                    this.nextFocusRow = nextRow;
                    this.incrementMonth();
                    break;
                case Common_1.keyCode.pageup:
                    e.preventDefault();
                    e.stopPropagation();
                    this.nextFocusCol = nextCol;
                    this.nextFocusRow = nextRow;
                    this.decrementMonth();
                    break;
                case Common_1.keyCode.up:
                    e.preventDefault();
                    e.stopPropagation();
                    nextRow -= 1;
                    break;
                case Common_1.keyCode.down:
                    e.preventDefault();
                    e.stopPropagation();
                    nextRow += 1;
                    break;
                case Common_1.keyCode.left:
                    e.preventDefault();
                    e.stopPropagation();
                    nextCol -= 1;
                    if (nextCol < 0) {
                        nextCol = 6;
                        nextRow -= 1;
                    }
                    break;
                case Common_1.keyCode.right:
                    e.preventDefault();
                    e.stopPropagation();
                    nextCol += 1;
                    if (nextCol > 6) {
                        nextCol = 0;
                        nextRow += 1;
                    }
                    break;
                default:
                    break;
            }
            nextFocus = this._container.querySelectorAll(
                `[data-row="${nextRow}"][data-col="${nextCol}"]`
            )[0];
            // if we found the next button to focus on, focus it
            if (nextFocus != null) {
                nextFocus.focus();
            }
        }
    }
    setContainerRef(element) {
        this._container = element;
    }
    render() {
        const rowClassName = css("calendar-row"),
            colClassName = css("disabled"),
            curYear = this.state.currentDate.year,
            curMonth = this.state.currentDate.month,
            weekdays = this.dayNames.map((day) => {
                return React.createElement(
                    Attributes_1.Elements.div,
                    { key: day, attr: this.props.attr.weekDayHeader },
                    day
                );
            });
        // First day of `month`
        let start = this.state.currentDate.copy();
        start.date = 1;
        // Last day of `month`
        let end = this.state.currentDate.copy();
        end.date = 1;
        end.month += 1;
        end.date = 0;
        let rows = [],
            row = [];
        start.date -= start.dateObject.getDay();
        end.date += 6 - end.dateObject.getDay();
        while (start.isBefore(end)) {
            // We have to copy the date, otherwise it will get modified in place
            row.push(start.copy());
            if (row.length >= Common_1.weekLength) {
                rows.push(row);
                row = [];
            }
            start.date += 1;
        }
        const content = rows.map((row, rowIndex) => {
            let inner = row.map((col, colIndex) => {
                const onClick = (event) => {
                        this.onClick(col);
                        event.preventDefault();
                    },
                    date = col.date,
                    colMonth = col.month,
                    key = `${colMonth}-${date}`,
                    ariaLabel = new Date(
                        `${curYear}-${colMonth + 1}-${date}`
                    ).toLocaleDateString(this.props.locale, {
                        weekday: "long",
                        year: "numeric",
                        month: "long",
                        day: "numeric",
                    });
                /** Grayed out day from another month */
                if (colMonth !== curMonth) {
                    return React.createElement(
                        Attributes_1.Elements.button,
                        {
                            type: "button",
                            "aria-label": ariaLabel,
                            "data-row": rowIndex,
                            "data-col": colIndex,
                            onKeyDown: this.onKeyDown,
                            className: colClassName,
                            onClick: onClick,
                            key: key,
                            attr: this.props.attr.dateButton,
                        },
                        date
                    );
                }
                /** Selected day */
                if (this.props.value) {
                    const isSelected =
                        this.props.value &&
                        date === this.value.date &&
                        col.month === this.value.month &&
                        col.year === this.value.year;
                    if (isSelected) {
                        return React.createElement(
                            Attributes_1.Elements.button,
                            {
                                type: "button",
                                "aria-label": ariaLabel,
                                "data-row": rowIndex,
                                "data-col": colIndex,
                                onKeyDown: this.onKeyDown,
                                className: css("selected"),
                                onClick: onClick,
                                key: key,
                                attr: this.props.attr.dateButton,
                            },
                            date
                        );
                    }
                }
                /** Everything else */
                return React.createElement(
                    Attributes_1.Elements.button,
                    {
                        type: "button",
                        "aria-label": ariaLabel,
                        "data-row": rowIndex,
                        "data-col": colIndex,
                        onKeyDown: this.onKeyDown,
                        onClick: onClick,
                        key: key,
                        attr: this.props.attr.dateButton,
                    },
                    date
                );
            });
            return React.createElement(
                Attributes_1.Elements.div,
                {
                    className: rowClassName,
                    key: rowIndex,
                    attr: this.props.attr.dateRow,
                },
                inner
            );
        });
        return React.createElement(
            Attributes_1.Elements.div,
            {
                methodRef: this.setContainerRef,
                className: css("calendar", this.props.className),
                attr: this.props.attr.container,
            },
            React.createElement(
                Attributes_1.Elements.div,
                {
                    className: css("calendar-header"),
                    attr: this.props.attr.header,
                },
                React.createElement(
                    Attributes_1.Elements.div,
                    {
                        className: css("calendar-month"),
                        attr: this.props.attr.monthHeader,
                    },
                    `${this.monthNames[curMonth]} ${curYear}`
                ),
                React.createElement(
                    "div",
                    { className: css("action-bar") },
                    React.createElement(
                        ActionTriggerButton_1.ActionTriggerButton,
                        {
                            className: css("calendar-chevron"),
                            onClick: this.onPrevMonth,
                            icon: "chevronUp",
                            attr: this.props.attr.prevMonthButton,
                        }
                    ),
                    React.createElement(
                        ActionTriggerButton_1.ActionTriggerButton,
                        {
                            icon: "chevronDown",
                            className: css("calendar-chevron"),
                            onClick: this.onNextMonth,
                            attr: this.props.attr.nextMonthButton,
                        }
                    )
                )
            ),
            React.createElement(
                Attributes_1.Elements.div,
                {
                    className: css("calendar-days"),
                    attr: this.props.attr.dateContainer,
                },
                weekdays
            ),
            content
        );
    }
}
Calendar.defaultProps = {
    localTimezone: true,
    attr: {
        container: {},
        header: {},
        monthHeader: {},
        prevMonthButton: {},
        nextMonthButton: {},
        weekDayHeader: {},
        dateContainer: {},
        dateButton: {},
        dateRow: {},
    },
};
exports.Calendar = Calendar;
exports.default = Calendar;
