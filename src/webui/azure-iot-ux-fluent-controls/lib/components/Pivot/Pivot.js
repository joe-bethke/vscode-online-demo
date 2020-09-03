Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    styled_components_1 = require("styled-components"),
    Attributes_1 = require("../../Attributes"),
    Icon_1 = require("../Icon"),
    classNames = require("classnames/bind"),
    css = classNames.bind(require("./Pivot.module.scss"));
exports.pivotClassName = css("pivot");
exports.menuClassName = css("pivot-menu");
const StyledPivotBorder = styled_components_1.default(
    Attributes_1.Elements.div
)`
    &&&& {
        ${(props) =>
            props.theme.colorBgBtnPrimaryRest &&
            "border-bottom: 2px solid" + props.theme.colorBgBtnPrimaryRest}
    }
`;
exports.Pivot = (props) => {
    let contents;
    if (props.icon) {
        contents = React.createElement(
            Icon_1.Icon,
            {
                icon: props.icon,
                size: Icon_1.IconSize.xsmall,
                className: css("pivot-icon"),
                labelClassName: css("pivot-icon-label"),
                attr: props.attr.icon,
            },
            props.text
        );
    } else {
        contents = React.createElement(
            Attributes_1.Elements.span,
            { className: css("pivot-label"), attr: props.attr.content },
            props.text
        );
    }
    const PivotBorderProxy = props.selected
        ? StyledPivotBorder
        : Attributes_1.Elements.div;
    /**
     * Contents are rendered twices to give the pivot height and allow the text
     * to center vertically
     */
    return React.createElement(
        Attributes_1.Elements.div,
        {
            className: css("pivot-container", {
                disabled: props.disabled,
                selected: props.selected,
            }),
            attr: props.attr.container,
        },
        contents,
        contents,
        React.createElement(PivotBorderProxy, {
            className: css("pivot-border"),
            attr: props.attr.bottomBorder,
        }),
        React.createElement(Attributes_1.Elements.div, {
            className: css("focus-border"),
            attr: props.attr.focusBorder,
        }),
        React.createElement(Attributes_1.Elements.div, {
            className: css("pivot-contents"),
            attr: props.attr.innerContent,
        })
    );
};
exports.Pivot.defaultProps = {
    text: undefined,
    attr: {
        container: {},
        bottomBorder: {},
        focusBorder: {},
        content: {},
        icon: { container: {}, label: {} },
    },
};
exports.default = exports.Pivot;
