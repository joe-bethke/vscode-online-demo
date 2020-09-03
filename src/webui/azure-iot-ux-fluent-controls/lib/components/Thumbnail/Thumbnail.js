Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    Attributes_1 = require("../../Attributes"),
    classNames = require("classnames/bind"),
    cx = classNames.bind(require("./Thumbnail.module.scss")),
    /**
     * Kind thumbnails load up an icon
     */
    kindIcons = {
        product: "icon-alias-product",
        device: "icon-alias-device",
        user: "icon-alias-user",
        unknown: "icon-alias-unknown",
        missing: "icon-alias-missing-image",
    };
class Thumbnail extends React.Component {
    constructor(props) {
        super(props);
        this.imgRef = React.createRef();
        this.handleError = (error) => {
            this.setState({
                imageLoaded: false,
            });
        };
        this.handleImageLoad = () => {
            this.setState({
                imageLoaded: true,
            });
        };
        this.state = { imageLoaded: false };
    }
    render() {
        const className = cx(
            "circle",
            this.props.size || "preview",
            this.props.className
        );
        if (this.props.loading) {
            return React.createElement(Attributes_1.Elements.div, {
                className: className,
            });
        }

        let icon = this.props.icon || kindIcons[this.props.kind];
        return React.createElement(
            Attributes_1.Elements.div,
            Object.assign({ className: className }, this.props.attr),
            this.props.url
                ? React.createElement("img", {
                      className: cx({ hidden: !this.state.imageLoaded }),
                      ref: this.imgRef,
                      src: this.props.url,
                      "aria-label": this.props.ariaLabel,
                      onLoad: this.handleImageLoad,
                      onError: this.handleError,
                  })
                : null,
            icon
                ? React.createElement("span", {
                      className: cx("icon", icon, {
                          hidden: this.state.imageLoaded,
                      }),
                  })
                : null
        );
    }
}
exports.Thumbnail = Thumbnail;
exports.default = Thumbnail;
