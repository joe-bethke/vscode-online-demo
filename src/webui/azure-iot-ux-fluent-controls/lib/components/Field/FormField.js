Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    Attributes_1 = require("../../Attributes"),
    Common_1 = require("../../Common"),
    Loader_1 = require("../Loader"),
    FormLabel_1 = require("./FormLabel"),
    FormError_1 = require("./FormError"),
    classNames = require("classnames/bind"),
    css = classNames.bind(require("./Field.module.scss"));
/**
 * High level generic form field
 *
 * @param props Control properties (defined in `FormFieldProps` interface)
 */
class FormField extends React.PureComponent {
    constructor(props) {
        super(props);
        this.handleKeyDown = this.handleKeyDown.bind(this);
        this.handleBlur = this.handleBlur.bind(this);
        this._self = React.createRef();
        this.state = {
            tooltipVisible: false,
        };
    }
    handleKeyDown(e) {
        // if user pressed Alt + F1 open the tooltip
        if (e.altKey && e.keyCode === Common_1.keyCode.f1) {
            this.setState({
                tooltipVisible: true,
            });
            e.preventDefault();
            e.stopPropagation();
            // if the user pressed escape key, close the tooltip
        } else if (e.keyCode === Common_1.keyCode.escape) {
            this.setState({
                tooltipVisible: false,
            });
            e.preventDefault();
            e.stopPropagation();
        } else if (e.keyCode === Common_1.keyCode.tab) {
            // BUG 3217787: if the user tabs out, close the tooltip and continue to default behavior:
            this.setState({
                tooltipVisible: false,
            });
        }
    }
    handleBlur(e) {
        if (
            !!e.relatedTarget &&
            !this._self.current.contains(e.relatedTarget)
        ) {
            this.setState({
                tooltipVisible: false,
            });
        }
    }
    render() {
        const props = this.props,
            containerClass = css(
                "input-container",
                {
                    "input-error": !!props.error,
                    required: props.required && typeof props.label === "string",
                },
                props.className
            );
        let error = props.error;
        if (props.loading) {
            error = React.createElement(Loader_1.HorizontalLoader, { dots: 6 });
        }
        return React.createElement(
            Attributes_1.Elements.div,
            {
                methodRef: this._self,
                className: containerClass,
                attr: props.attr.fieldContainer,
                onBlur: this.handleBlur,
            },
            !!props.label &&
                React.createElement(
                    FormLabel_1.FormLabel,
                    Object.assign(
                        {
                            name: props.name,
                            icon: "info",
                            balloon: props.tooltip,
                            attr: props.attr.fieldLabel,
                            required: props.required,
                            balloonExpanded: this.state.tooltipVisible,
                            farSide: props.labelFarSide,
                        },
                        props.attr.fieldTooltip
                    ),
                    props.label
                ),
            React.createElement(
                Attributes_1.Elements.div,
                {
                    className: css("content"),
                    attr: props.attr.fieldContent,
                    onKeyDown: this.handleKeyDown,
                },
                props.children
            ),
            React.createElement(
                FormError_1.FormError,
                {
                    className: props.errorClassName,
                    hidden: props.hideError,
                    title: props.errorTitle,
                    attr: {
                        container: Object.assign(
                            { "aria-live": "polite", "aria-atomic": "true" },
                            props.attr.fieldError
                        ),
                    },
                },
                error
            )
        );
    }
}
FormField.defaultProps = {
    name: undefined,
    label: undefined,
    loading: false,
    hideError: false,
    attr: {
        fieldContainer: {},
        fieldLabel: {},
        fieldContent: {},
        fieldError: {},
    },
};
exports.FormField = FormField;
exports.default = FormField;
