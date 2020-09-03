Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    Attributes_1 = require("../../Attributes"),
    Dropdown_1 = require("../Dropdown"),
    classNames = require("classnames/bind"),
    css = classNames.bind(require("./Balloon.module.scss"));
let BalloonPosition;
(function (BalloonPosition) {
    BalloonPosition[(BalloonPosition.Top = 1)] = "Top";
    BalloonPosition[(BalloonPosition.Bottom = 2)] = "Bottom";
    BalloonPosition[(BalloonPosition.Left = 3)] = "Left";
    BalloonPosition[(BalloonPosition.Right = 4)] = "Right";
})(
    (BalloonPosition =
        exports.BalloonPosition || (exports.BalloonPosition = {}))
);
let BalloonAlignment;
(function (BalloonAlignment) {
    BalloonAlignment[(BalloonAlignment.Start = 1)] = "Start";
    BalloonAlignment[(BalloonAlignment.Center = 2)] = "Center";
    BalloonAlignment[(BalloonAlignment.End = 3)] = "End";
})(
    (BalloonAlignment =
        exports.BalloonAlignment || (exports.BalloonAlignment = {}))
);
/**
 * SimpleBalloon shows tooltip (with HTML) on hover over child
 *
 * NOTE: If a parent element of this control is `overflow: hidden` then the
 * balloon may not show up.
 *
 * @param props Control properties (defined in `SimpleBalloonProps` interface)
 */
class Balloon extends React.Component {
    constructor(props) {
        super(props);
        this.onMouseEnter = (event) => {
            this.setState({
                hovered: true,
                visible: true,
            });
        };
        this.onMouseLeave = (event) => {
            this.setState({
                hovered: false,
                visible: this.props.expanded,
            });
        };
        this.state = {
            hovered: false,
            visible: this.props.expanded,
            position: this.props.position,
            align: this.props.align,
        };
    }
    componentWillReceiveProps(newProps) {
        this.setState({
            visible: this.state.hovered || newProps.expanded,
            position: newProps.position,
            align: newProps.align,
        });
    }
    shouldComponentUpdate(newProps, newState) {
        if (newProps !== this.props) {
            return true;
        }
        if (this.state.visible !== newState.visible) {
            return true;
        }
        if (
            this.state.position !== newState.position ||
            this.state.align !== newState.align
        ) {
            return true;
        }
        return false;
    }
    getClassName(reverse) {
        let position, reversePosition;
        switch (this.props.position) {
            case BalloonPosition.Bottom:
                position = "bottom";
                reversePosition = "top";
                break;
            case BalloonPosition.Left:
                position = "left";
                reversePosition = "right";
                break;
            case BalloonPosition.Right:
                position = "right";
                reversePosition = "left";
                break;
            default:
                position = "top";
                reversePosition = "bottom";
        }
        let align;
        switch (this.props.align) {
            case BalloonAlignment.Start:
                align = "start";
                break;
            case BalloonAlignment.End:
                align = "end";
                break;
            default:
                align = "center";
        }
        return css(`${reverse ? reversePosition : position}-${align}`);
    }
    render() {
        let { balloonClassName, multiline, className } = this.props;
        balloonClassName = css(
            "balloon-dropdown",
            this.getClassName(false),
            balloonClassName
        );
        className = css("balloon-container", className);
        const innerClassName = css("balloon-inner-container", { multiline }),
            positions = [this.getClassName(false), this.getClassName(true)];
        return React.createElement(
            Dropdown_1.Dropdown,
            {
                dropdown: React.createElement(
                    Attributes_1.Elements.span,
                    {
                        className: innerClassName,
                        attr: this.props.attr.balloonContent,
                    },
                    this.props.tooltip
                ),
                visible: this.state.visible,
                className: className,
                positionClassNames: this.props.autoPosition ? positions : [],
                onMouseEnter: this.onMouseEnter,
                onMouseLeave: this.onMouseLeave,
                attr: {
                    container: this.props.attr.container,
                    dropdownContainer: this.props.attr.balloonContainer,
                    dropdown: Attributes_1.mergeAttributes(
                        this.props.attr.balloon,
                        { className: balloonClassName }
                    ),
                },
            },
            this.props.children
        );
    }
}
Balloon.defaultProps = {
    tooltip: undefined,
    position: BalloonPosition.Top,
    align: BalloonAlignment.Center,
    expanded: false,
    multiline: true,
    autoPosition: true,
    attr: {
        container: {},
        balloonContainer: {},
        balloon: {},
    },
};
exports.Balloon = Balloon;
exports.default = Balloon;
