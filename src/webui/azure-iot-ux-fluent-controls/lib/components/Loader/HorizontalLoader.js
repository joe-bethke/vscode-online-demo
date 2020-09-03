Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    classNames = require("classnames/bind"),
    css = classNames.bind(require("./HorizontalLoader.module.scss"));
/**
 * HorizontalLoader showing Information, Warning, or Error with text, icon, and optional close button
 *
 * @param props Control properties (defined in `HorizontalLoaderProps` interface)
 */
exports.HorizontalLoader = (props) => {
    const className = css("horizontal-loader-inner"),
        containerClassName = css("horizontal-loader");
    let dots = [];
    for (let i = 0; i < props.dots; i++) {
        dots.push(
            React.createElement(
                "div",
                { className: className, key: i },
                React.createElement("div", null)
            )
        );
    }
    return React.createElement("div", { className: containerClassName }, dots);
};
exports.HorizontalLoader.defaultProps = {
    dots: 6,
};
exports.default = exports.HorizontalLoader;
