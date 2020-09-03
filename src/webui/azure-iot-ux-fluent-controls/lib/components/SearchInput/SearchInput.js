Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    Icon_1 = require("../Icon"),
    TextInput_1 = require("../Input/TextInput"),
    ActionTrigger_1 = require("../ActionTrigger"),
    classNames = require("classnames/bind"),
    css = classNames.bind(require("./SearchInput.module.scss"));
exports.prefixClassName = css("prefix-addon");
exports.postfixClassName = css("postfix-addon");
class SearchInput extends React.PureComponent {
    render() {
        const postfix = this.props.value
            ? React.createElement(ActionTrigger_1.ActionTriggerButton, {
                  onClick: this.props.onSubmit,
                  icon: "forward",
                  className: css("search-button"),
                  attr: {
                      button: { title: this.props.label, type: "submit" },
                      container: this.props.attr
                          ? this.props.attr.postfix
                          : null,
                  },
              })
            : null;
        return React.createElement(
            "form",
            {
                className: css(
                    "search-input-container",
                    this.props.containerClassName
                ),
                onSubmit: this.props.onSubmit,
                role: "search",
            },
            React.createElement(Icon_1.Icon, {
                icon: "search",
                className: css("search-prefix-icon"),
            }),
            React.createElement(TextInput_1.default, {
                name: "search-input",
                value: this.props.value === null ? "" : this.props.value,
                className: css("search-input", this.props.inputClassName),
                onChange: this.props.onChange,
                placeholder: this.props.label,
                postfix: postfix,
                postfixClassName: css("search-button-container"),
                attr: {
                    input: Object.assign(
                        {
                            className: css("input-component"),
                            autoComplete: "off",
                            "aria-label": this.props.label,
                            type: "search",
                            onClick: blockPropagation,
                        },
                        this.props.attr && this.props.attr.input
                    ),
                },
            })
        );
    }
}
exports.SearchInput = SearchInput;
function blockPropagation(event) {
    event.stopPropagation();
}
exports.default = SearchInput;
