Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    Common_1 = require("../../Common"),
    FormField_1 = require("../Field/FormField"),
    /** This import solves an error with exports of FormFieldAttributes defaults */
    TimeInput_1 = require("./TimeInput"),
    DatePicker_1 = require("./DatePicker"),
    Attributes_1 = require("../../Attributes"),
    classNames = require("classnames/bind"),
    css = classNames.bind(require("./DateTimeField.module.scss"));
/**
 * High level date time field
 *
 * @param props Control properties (defined in `DateTimeFieldProps` interface)
 */
class DateTimeField extends React.Component {
    constructor(props) {
        super(props);
        this.onDatePaste = (newDate) => {
            const date = new Date(newDate);
            if (Common_1.dateIsValid(date, this.props.localTimezone)) {
                const utcDate = date.toJSON();
                this.setState({
                    initialDate: date,
                    lastDate: utcDate,
                    lastTime: utcDate,
                });
                this.props.onChange(utcDate);
                return false;
            }
            this.setState({
                lastDate: "invalid",
            });
            this.props.onChange("invalid");
            return true;
        };
        this.onChange = (newDate, newTime) => {
            if (newDate === "") {
                this.props.onChange(newDate);
                return null;
            }
            if (
                newDate === "invalid" ||
                newTime === "invalid" ||
                !newDate ||
                !newTime
            ) {
                this.props.onChange("invalid");
                return null;
            }
            const date =
                    typeof newDate === "string" ? new Date(newDate) : newDate,
                time =
                    typeof newTime === "string" ? new Date(newTime) : newTime,
                newValue = this.props.localTimezone
                    ? new Date(
                          date.getFullYear(),
                          date.getMonth(),
                          date.getDate(),
                          time.getHours(),
                          time.getMinutes(),
                          time.getSeconds()
                      )
                    : new Date(
                          Date.UTC(
                              date.getUTCFullYear(),
                              date.getUTCMonth(),
                              date.getUTCDate(),
                              time.getUTCHours(),
                              time.getUTCMinutes(),
                              time.getUTCSeconds()
                          )
                      ),
                utcValue = newValue.toJSON();
            if (utcValue === "Invalid Date") {
                this.props.onChange("invalid");
            } else {
                this.props.onChange(utcValue);
            }
            return newValue;
        };
        this.onDateChange = (newDate) => {
            const newValue = this.onChange(newDate, this.state.lastTime);
            if (newValue) {
                const utcValue = newValue.toJSON();
                this.setState({
                    lastTime: utcValue,
                    lastDate: utcValue,
                });
            } else {
                this.setState({
                    lastDate: newDate,
                });
            }
        };
        this.onTimeChange = (newTime) => {
            const newValue = this.onChange(this.state.lastDate, newTime);
            if (newValue) {
                const utcValue = newValue.toJSON();
                this.setState({
                    initialDate: newValue,
                    lastTime: utcValue,
                    lastDate: utcValue,
                });
            }
        };
        this.state = this.getInitialState(props);
    }
    getInitialState(props) {
        const local = props.localTimezone;
        let initialValue = null,
            invalid = false;
        if (props.initialValue || props.initialValue === "") {
            if (typeof props.initialValue === "string") {
                const date = new Date(props.initialValue);
                if (Common_1.dateIsValid(date, local)) {
                    /**
                     * This is where DateTimeField receives an initial Date value
                     * so this is where localTimezone/GMT have to be handled.
                     *
                     * Calling new Date(Date.UTC(year, month, date, ...)) creates
                     * a Date object that looks like the local timezone but actually
                     * represents a time in GMT
                     */
                    initialValue = local
                        ? date
                        : new Date(
                              Date.UTC(
                                  date.getUTCFullYear(),
                                  date.getUTCMonth(),
                                  date.getUTCDate(),
                                  date.getUTCHours(),
                                  date.getUTCMinutes(),
                                  date.getUTCSeconds()
                              )
                          );
                } else {
                    invalid = true;
                }
            } else if (!Common_1.dateIsValid(props.initialValue, local)) {
                invalid = true;
            } else {
                /**
                 * This is where DateTimeField receives an initial Date value
                 * so this is where localTimezone/GMT have to be handled.
                 *
                 * Calling new Date(Date.UTC(year, month, date, ...)) creates
                 * a Date object that looks like the local timezone but actually
                 * represents a time in GMT
                 */
                initialValue = local
                    ? new Date(props.initialValue)
                    : new Date(
                          Date.UTC(
                              props.initialValue.getUTCFullYear(),
                              props.initialValue.getUTCMonth(),
                              props.initialValue.getUTCDate(),
                              props.initialValue.getUTCHours(),
                              props.initialValue.getUTCMinutes(),
                              props.initialValue.getUTCSeconds()
                          )
                      );
            }
        }
        let defaultTime = null;
        if (invalid) {
            const date = new Date();
            defaultTime = props.localTimezone
                ? date
                : new Date(
                      Date.UTC(
                          date.getUTCFullYear(),
                          date.getUTCMonth(),
                          date.getUTCDate(),
                          date.getUTCHours(),
                          date.getUTCMinutes(),
                          date.getUTCSeconds()
                      )
                  );
        }
        return {
            initialDate: invalid ? props.initialValue : initialValue.toJSON(),
            lastTime: invalid ? defaultTime.toJSON() : initialValue.toJSON(),
            lastDate: invalid ? "invalid" : initialValue.toJSON(),
        };
    }
    componentWillReceiveProps(newProps) {
        if (
            this.props.initialValue !== newProps.initialValue ||
            this.props.localTimezone !== newProps.localTimezone
        ) {
            this.setState(this.getInitialState(newProps));
        }
    }
    render() {
        const tooltipId = this.props.tooltip
                ? `${this.props.name}-tt`
                : undefined,
            errorId = `${this.props.name}-error`;
        let describedby = errorId;
        if (tooltipId) {
            describedby += " " + tooltipId;
        }
        const dateAttr = {
                input: Object.assign(
                    {
                        "aria-describedby": describedby,
                    },
                    this.props.attr.datePicker &&
                        this.props.attr.datePicker.input
                ),
                inputContainer:
                    this.props.attr.datePicker &&
                    this.props.attr.datePicker.inputContainer,
                inputIcon:
                    this.props.attr.datePicker &&
                    this.props.attr.datePicker.inputIcon,
                calendar:
                    this.props.attr.datePicker &&
                    this.props.attr.datePicker.calendar,
            },
            timeAttr = Object.assign(
                {
                    hourSelect: Object.assign(
                        { "aria-describedby": describedby },
                        this.props.attr.timeInput &&
                            this.props.attr.timeInput.hourSelect
                    ),
                    minuteSelect: Object.assign(
                        { "aria-describedby": describedby },
                        this.props.attr.timeInput &&
                            this.props.attr.timeInput.minuteSelect
                    ),
                    secondSelect: Object.assign(
                        { "aria-describedby": describedby },
                        this.props.attr.timeInput &&
                            this.props.attr.timeInput.secondSelect
                    ),
                    periodSelect: Object.assign(
                        { "aria-describedby": describedby },
                        this.props.attr.timeInput &&
                            this.props.attr.timeInput.periodSelect
                    ),
                },
                this.props.attr.timeInput && this.props.attr.timeInput
            ),
            fieldAttr = {
                fieldLabel: Object.assign(
                    {
                        balloon: {
                            balloon: {
                                id: tooltipId,
                            },
                        },
                    },
                    this.props.attr.fieldLabel
                ),
                fieldError: Object.assign(
                    {
                        id: errorId,
                    },
                    this.props.attr.fieldError
                ),
                fieldContent: this.props.attr.fieldContent,
                fieldContainer: this.props.attr.fieldContainer,
            };
        return React.createElement(
            FormField_1.FormField,
            {
                name: this.props.name,
                label: this.props.label,
                error: this.props.error,
                errorTitle: this.props.errorTitle,
                loading: this.props.loading,
                required: this.props.required,
                hideError: this.props.hideError,
                className: css("datetime-field", this.props.className),
                attr: fieldAttr,
                tooltip: this.props.tooltip,
                labelFarSide: this.props.labelFarSide,
            },
            React.createElement(
                Attributes_1.Elements.div,
                {
                    className: css("field-content"),
                    attr: this.props.attr.flexContainer,
                },
                React.createElement(
                    Attributes_1.Elements.span,
                    {
                        className: css("field-date"),
                        attr: this.props.attr.dateColumn,
                    },
                    React.createElement(DatePicker_1.DatePicker, {
                        name: this.props.name,
                        initialValue: this.state.initialDate,
                        tabIndex: this.props.tabIndex,
                        error: !!this.props.error,
                        disabled: this.props.disabled,
                        locale: this.props.locale,
                        localTimezone: this.props.localTimezone,
                        showAbove: this.props.showAbove,
                        format: this.props.format,
                        required: this.props.required,
                        onPaste: this.onDatePaste,
                        onChange: this.onDateChange,
                        onExpand: this.props.onExpand,
                        className: css(
                            "date-picker",
                            this.props.inputClassName
                        ),
                        attr: dateAttr,
                    })
                ),
                React.createElement(
                    Attributes_1.Elements.span,
                    {
                        className: css("field-time"),
                        attr: this.props.attr.timeColumn,
                    },
                    React.createElement(TimeInput_1.TimeInput, {
                        name: this.props.name,
                        value: this.state.lastTime,
                        amLabel: this.props.amLabel,
                        pmLabel: this.props.pmLabel,
                        localTimezone: this.props.localTimezone,
                        showSeconds: this.props.showSeconds,
                        militaryTime: this.props.militaryTime,
                        error: !!this.props.error,
                        disabled: this.props.disabled,
                        onChange: this.onTimeChange,
                        className: css(
                            "time-picker",
                            this.props.inputClassName
                        ),
                        attr: timeAttr,
                    })
                )
            )
        );
    }
}
DateTimeField.defaultProps = {
    format: Common_1.DateFormat.MMDDYYYY,
    tabIndex: -1,
    localTimezone: true,
    showAbove: false,
    showSeconds: false,
    militaryTime: false,
    attr: {
        fieldContainer: {},
        fieldLabel: {},
        fieldContent: {},
        fieldError: {},
        flexContainer: {},
        dateColumn: {},
        timeColumn: {},
        datePicker: {
            container: {},
            inputContainer: {},
            input: {},
            inputIcon: {},
            calendar: {},
        },
        timeInput: {
            container: {},
            hourSelect: {},
            hourOption: {},
            minuteSelect: {},
            minuteOption: {},
            secondSelect: {},
            secondOption: {},
            periodSelect: {},
            periodOption: {},
        },
    },
};
exports.DateTimeField = DateTimeField;
exports.default = DateTimeField;
