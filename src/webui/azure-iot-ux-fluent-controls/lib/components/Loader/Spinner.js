Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    classNames = require("classnames/bind"),
    css = classNames.bind(require("./Spinner.module.scss"));
/**
 * Spinner showing Information, Warning, or Error with text, icon, and optional close button
 *
 * @param props Control properties (defined in `SpinnerProps` interface)
 */
exports.Spinner = (props) => {
    const className = css("cs-loader-inner"),
        containerClassName = css("cs-loader", props.className);
    return React.createElement(
        "div",
        { className: containerClassName },
        React.createElement(
            "div",
            { className: className },
            React.createElement("div", null)
        ),
        React.createElement(
            "div",
            { className: className },
            React.createElement("div", null)
        ),
        React.createElement(
            "div",
            { className: className },
            React.createElement("div", null)
        ),
        React.createElement(
            "div",
            { className: className },
            React.createElement("div", null)
        ),
        React.createElement(
            "div",
            { className: className },
            React.createElement("div", null)
        ),
        React.createElement(
            "div",
            { className: className },
            React.createElement("div", null)
        )
    );
};
exports.default = exports.Spinner;
