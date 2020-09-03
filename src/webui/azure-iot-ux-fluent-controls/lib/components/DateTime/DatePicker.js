Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    Attributes_1 = require("../../Attributes"),
    Calendar_1 = require("./Calendar"),
    helpers_1 = require("./helpers"),
    Common_1 = require("../../Common"),
    ActionTrigger_1 = require("../ActionTrigger"),
    classNames = require("classnames/bind"),
    css = classNames.bind(require("./DatePicker.module.scss"));
/**
 * Low level date picker control
 *
 * (Use the `DateField` control instead when making a form with standard styling)
 */
class DatePicker extends React.Component {
    constructor(props) {
        super(props);
        this.inputRef = (element) => (this.input = element);
        this.onChange = (event) => {
            let newValue = event.target.value;
            if (newValue === "") {
                this.paste = false;
            }
            if (this.paste) {
                const date = Common_1.MethodDate.fromString(
                    this.props.localTimezone,
                    newValue
                );
                if (date) {
                    newValue = helpers_1.formatDate(
                        date.dateObject,
                        this.props.format,
                        this.props.localTimezone
                    );
                    this.paste = false;
                    if (this.props.onPaste) {
                        this.props.onPaste(date.dateObject.toJSON());
                    } else {
                        this.props.onChange(date.dateObject.toJSON());
                    }
                } else {
                    this.props.onChange("invalid");
                    this.setState({ value: newValue });
                }
            } else {
                let result = this.parse(newValue);
                if (result.valid) {
                    const initialValue = this.state.initialValue,
                        dateValue = new Common_1.MethodDate(
                            this.props.localTimezone,
                            result.year,
                            result.month - 1,
                            result.date,
                            initialValue.hours,
                            initialValue.minutes,
                            initialValue.seconds
                        );
                    /**
                     * Using the MethodDate/Date constructor forces years to be
                     * at least 100 but we have to support any year > 0
                     */
                    if (result.year < 100) {
                        if (this.props.localTimezone) {
                            dateValue.dateObject.setFullYear(
                                result.year,
                                result.month - 1,
                                result.date
                            );
                        } else {
                            dateValue.dateObject.setUTCFullYear(
                                result.year,
                                result.month - 1,
                                result.date
                            );
                        }
                    }
                    this.props.onChange(dateValue.dateObject.toJSON());
                } else {
                    this.props.onChange(newValue === "" ? newValue : "invalid");
                    this.setState({ value: newValue });
                }
            }
            if (newValue.length === 0) {
                this.paste = false;
            }
        };
        this.onExpand = () => {
            const nextExpanded = !this.state.expanded;
            this.setState({ expanded: nextExpanded });
            if (typeof this.props.onExpand === "function") {
                this.props.onExpand(nextExpanded);
            }
        };
        this.onSelect = (newValue) => {
            this.setState({ expanded: false });
            this.props.onChange(newValue.toJSON());
        };
        this.onPaste = () => {
            this.paste = true;
        };
        this.onOuterMouseEvent = (e) => {
            if (this.state.expanded && !this._containerRef.contains(e.target)) {
                this.setState({ expanded: false });
            }
        };
        this.onKeydown = (e) => {
            if (this.state.expanded && e.keyCode === Common_1.keyCode.escape) {
                e.preventDefault();
                e.stopPropagation();
                this.setState({ expanded: false });
            }
        };
        this.onBlur = (e) => {
            if (
                e.relatedTarget &&
                !this._containerRef.contains(e.relatedTarget)
            ) {
                this.setState({ expanded: false });
            }
        };
        this.setContainerRef = (element) => {
            this._containerRef = element;
        };
        const newState = this.getInitialState(props, "");
        this.state = Object.assign({}, newState, { expanded: false });
        this.paste = false;
    }
    /**
     * Use props.initialValue to generate a new state
     *
     * props.initialValue is used to set the hours/minutes/seconds on internal Date
     *
     * @param props DatePickerProps
     */
    getInitialState(props, currentValue) {
        const local = props.localTimezone;
        let value = currentValue,
            initialValue = null;
        if (props.initialValue) {
            if (props.initialValue === "invalid") {
                if (this.state && this.state.initialValue) {
                    initialValue = Common_1.MethodDate.fromString(
                        props.localTimezone,
                        this.state.initialValue.dateObject.toJSON()
                    );
                }
            } else if (typeof props.initialValue === "string") {
                const date = Common_1.MethodDate.fromString(
                    local,
                    props.initialValue
                );
                if (date && Common_1.dateIsValid(date.dateObject, local)) {
                    initialValue = date;
                    const parsed = this.parse(currentValue);
                    if (
                        date.year !== parsed.year ||
                        date.month !== parsed.month - 1 ||
                        date.date !== parsed.date ||
                        !parsed.valid
                    ) {
                        /**
                         * Here we use props.initialValue to set the value of the text box
                         *
                         * This happens if state.value is different from the new initialValue
                         * or if the text input (state.value) is in an invalid state such as
                         * empty values or invalid dates like febuary 30th (2/30/2017)
                         */
                        value = helpers_1.formatDate(
                            date.dateObject,
                            props.format,
                            local
                        );
                    }
                } else {
                    value = props.initialValue;
                }
            } else if (props.initialValue) {
                value = helpers_1.formatDate(
                    props.initialValue,
                    props.format,
                    local
                );
                if (Common_1.dateIsValid(props.initialValue, local)) {
                    initialValue = Common_1.MethodDate.fromDate(
                        local,
                        props.initialValue
                    );
                }
            } else {
                initialValue = new Common_1.MethodDate(local);
            }
        }
        if (
            !initialValue ||
            initialValue.dateObject.toString() === "Invalid Date"
        ) {
            const today = new Common_1.MethodDate(local);
            initialValue = today;
        }
        return {
            value: value,
            initialValue: initialValue,
        };
    }
    /**
     * Update the Date/Time object used internally with a new initialValue
     *
     * @param newProps new DatePickerProps
     */
    componentWillReceiveProps(newProps) {
        if (
            (this.props.initialValue !== newProps.initialValue ||
                this.props.localTimezone !== newProps.localTimezone) &&
            newProps.initialValue !== "invalid"
        ) {
            const newState = this.getInitialState(newProps, this.input.value);
            this.setState(Object.assign({}, newState));
        }
    }
    componentDidMount() {
        window.addEventListener("click", this.onOuterMouseEvent);
        window.addEventListener("keydown", this.onKeydown);
    }
    componentWillUnmount() {
        window.removeEventListener("click", this.onOuterMouseEvent);
        window.removeEventListener("keydown", this.onKeydown);
    }
    parse(newValue) {
        let valid = true,
            split = newValue.split("/");
        if (split.length !== 3) {
            valid = false;
            while (split.length < 3) {
                split.push("-1");
            }
        }
        let date, month, year;
        if (this.props.format === Common_1.DateFormat.DDMMYYYY) {
            year = parseInt(split[2]);
            month = parseInt(split[1]);
            date = parseInt(split[0]);
        } else if (this.props.format === Common_1.DateFormat.MMDDYYYY) {
            year = parseInt(split[2]);
            month = parseInt(split[0]);
            date = parseInt(split[1]);
        } else if (this.props.format === Common_1.DateFormat.YYYYMMDD) {
            year = parseInt(split[0]);
            month = parseInt(split[1]);
            date = parseInt(split[2]);
        }
        /**
         * If you set Date.year to a number below 100, it assumes that you're
         * supplying a 2 digit year instead of 4 digits, turning 20 into 2020 etc
         */
        if (isNaN(year) || year < 100) {
            valid = false;
        }
        if (isNaN(month) || month < 1 || month > 12) {
            valid = false;
        }
        if (isNaN(date) || date < 1 || date > 31) {
            valid = false;
        }
        if (valid) {
            let parsed = new Common_1.MethodDate(
                this.props.localTimezone,
                year,
                month - 1,
                date
            );
            if (month !== parsed.month + 1 || date !== parsed.date) {
                valid = false;
            }
        }
        return { year, month, date, valid };
    }
    render() {
        const placeholder = helpers_1.placeholders[this.props.format],
            parsed = this.parse(this.state.value),
            inputClassName = css("date-picker-input", {
                error:
                    !!this.props.error ||
                    (!parsed.valid && !!this.props.initialValue),
            }),
            value = parsed.valid
                ? new Common_1.MethodDate(
                      this.props.localTimezone,
                      parsed.year,
                      parsed.month - 1,
                      parsed.date
                  ).dateObject.toJSON()
                : null;
        return React.createElement(
            Attributes_1.Elements.div,
            {
                methodRef: this.setContainerRef,
                className: css("date-picker-container", this.props.className),
                attr: this.props.attr.container,
                onBlur: this.onBlur,
            },
            React.createElement(
                Attributes_1.Elements.div,
                {
                    className: css("date-picker-input-container"),
                    attr: this.props.attr.inputContainer,
                },
                React.createElement(Attributes_1.Elements.input, {
                    type: "text",
                    name: this.props.name,
                    value: this.state.value,
                    className: inputClassName,
                    placeholder: placeholder,
                    onChange: this.onChange,
                    onPaste: this.onPaste,
                    required: this.props.required,
                    disabled: this.props.disabled,
                    methodRef: this.inputRef,
                    attr: this.props.attr.input,
                }),
                React.createElement(ActionTrigger_1.ActionTriggerButton, {
                    icon: "calendar",
                    className: css("date-picker-calendar-icon"),
                    onClick: this.onExpand,
                    disabled: this.props.disabled,
                    attr: this.props.attr.inputIcon,
                    "aria-haspopup": true,
                    "aria-expanded": this.state.expanded,
                })
            ),
            this.state.expanded &&
                React.createElement(
                    Attributes_1.Elements.div,
                    {
                        className: css("date-picker-dropdown", {
                            above: this.props.showAbove,
                        }),
                    },
                    React.createElement(Calendar_1.Calendar, {
                        value: value,
                        onChange: this.onSelect,
                        className: css("date-picker-calendar"),
                        year: parsed.year || null,
                        month: parsed.month - 1,
                        localTimezone: this.props.localTimezone,
                        locale: this.props.locale,
                        attr: this.props.attr.calendar,
                    }),
                    React.createElement("div", {
                        className: css("date-picker-dropdown-triangle"),
                    })
                )
        );
    }
}
DatePicker.defaultProps = {
    format: Common_1.DateFormat.MMDDYYYY,
    tabIndex: -1,
    localTimezone: true,
    showAbove: false,
    attr: {
        container: {},
        inputContainer: {},
        input: {},
        inputIcon: {},
        calendar: {},
    },
};
exports.DatePicker = DatePicker;
exports.default = DatePicker;
