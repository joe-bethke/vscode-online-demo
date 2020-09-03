Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    Attributes_1 = require("../../Attributes"),
    Icon_1 = require("../Icon"),
    Balloon_1 = require("../Balloon"),
    classNames = require("classnames/bind"),
    css = classNames.bind(require("./Field.module.scss"));
exports.requiredClassName = css("label", "required");
/**
 * High level generic form field
 *
 * @param props Control properties (defined in `FormLabelProps` interface)
 */
exports.FormLabel = (props) => {
    const balloon = props.balloon
        ? React.createElement(
              Balloon_1.Balloon,
              {
                  tooltip: props.balloon,
                  align:
                      props.balloonAlignment ||
                      Balloon_1.BalloonAlignment.Start,
                  position:
                      props.balloonPosition || Balloon_1.BalloonPosition.Top,
                  className: css("label-icon"),
                  multiline: true,
                  expanded: props.balloonExpanded,
                  attr: Attributes_1.mergeAttributeObjects(
                      props.attr.balloon,
                      {
                          balloon: {
                              className: css("label-balloon"),
                          },
                          balloonContainer: {
                              role: "tooltip",
                              "aria-live": "polite",
                              "aria-atomic": "true",
                          },
                      },
                      [
                          "container",
                          "balloonContainer",
                          "balloon",
                          "balloonContent",
                      ]
                  ),
              },
              React.createElement(Icon_1.Icon, {
                  icon: props.icon,
                  size: Icon_1.IconSize.xsmall,
                  attr: props.attr.icon,
              })
          )
        : "";
    return React.createElement(
        Attributes_1.Elements.div,
        {
            className: css("label-container", props.className),
            attr: props.attr.container,
        },
        React.createElement(
            Attributes_1.Elements.div,
            {
                className: css("label-inner-container"),
                attr: props.attr.innerContainer,
            },
            React.createElement(
                Attributes_1.Elements.label,
                {
                    className: css("label", { required: props.required }),
                    htmlFor: props.name,
                    attr: props.attr.text,
                },
                props.children
            ),
            balloon
        ),
        props.farSide &&
            React.createElement(
                Attributes_1.Elements.div,
                {
                    className: css("label-farSide-container"),
                    attr: props.attr.farSideContainer,
                },
                props.farSide
            )
    );
};
exports.FormLabel.defaultProps = {
    name: undefined,
    required: false,
    icon: "info",
    attr: {
        container: {},
        text: {},
        icon: {
            container: {},
            label: {},
        },
        balloon: {
            container: {},
            balloonContainer: {},
            balloon: {},
        },
    },
};
exports.default = exports.FormLabel;
