Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    react_dom_1 = require("react-dom"),
    root_1 = require("./root");
class Portal extends React.Component {
    constructor(props) {
        super(props);
        this.state = {};
    }
    componentDidMount() {
        // The portal root element won't be present on initial render, or if
        // we're rendering server-side. Therefore, wait till this component has
        // been mounted and then create the portal. This also allows us to use
        // autoFocus in a descendant (otherwise, the portal element is inserted
        // in the DOM tree after the children are mounted, meaning that children
        // will be mounted on a detached DOM node)
        const container = document.getElementById(root_1.ElementId);
        this.setState({ container });
    }
    render() {
        const { container } = this.state;
        if (!container) {
            return null;
        }
        return react_dom_1.createPortal(this.props.children, container);
    }
}
exports.Portal = Portal;
