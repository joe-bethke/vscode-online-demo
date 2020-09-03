Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react"),
    classNames = require("classnames");
function mergeAttributeObjects(leftInput, rightInput, names) {
    const output = {};
    let left = leftInput || {},
        right = rightInput || {};
    for (let name of names) {
        const oldAttr = left[name] || {},
            newAttr = right[name] || {};
        output[name] = mergeAttributes(oldAttr, newAttr);
    }
    return output;
}
exports.mergeAttributeObjects = mergeAttributeObjects;
function mergeAttributes(leftAttr, rightAttr) {
    const oldAttr = leftAttr || {},
        newAttr = rightAttr || {},
        className = classNames(oldAttr.className, newAttr.className);
    if (oldAttr.className) {
        delete oldAttr.className;
    }
    if (newAttr.className) {
        delete newAttr.className;
    }
    const fnCombiner = {};
    for (let key in oldAttr) {
        if (newAttr[key]) {
            const oldFn = oldAttr[key],
                newFn = newAttr[key];
            if (oldFn instanceof Function && newFn instanceof Function) {
                fnCombiner[key] = (...args) => {
                    oldFn(...args);
                    newFn(...args);
                };
                delete oldAttr[key];
                delete newAttr[key];
            }
        }
    }
    let ref = newAttr.ref;
    if (oldAttr.ref) {
        if (newAttr.ref) {
            const oldRef = oldAttr.ref,
                newRef = newAttr.ref;
            ref = (element) => {
                oldRef(element);
                newRef(element);
            };
            delete oldAttr.ref;
        } else {
            ref = oldAttr.ref;
        }
    }
    if (newAttr.ref) {
        delete newAttr.ref;
    }
    return Object.assign({}, oldAttr, newAttr, fnCombiner, { className, ref });
}
exports.mergeAttributes = mergeAttributes;
function AttrElementWrapper(element) {
    return function (props) {
        props = Object.assign({}, props);
        let attr = Object.assign({}, props.attr);
        if (attr) {
            delete props.attr;
        } else {
            let children;
            if (props.children) {
                children = props.children;
                delete props.children;
            }
            return React.createElement(element, props, ...children);
        }
        const className = classNames(props.className, attr.className);
        if (props.className) {
            delete props.className;
        }
        if (attr.className) {
            delete attr.className;
        }
        let ref = props.methodRef;
        if (attr.ref) {
            if (props.methodRef) {
                const oldRef = props.methodRef,
                    newRef = attr.ref;
                ref = (element) => {
                    newRef(element);
                    oldRef(element);
                };
            } else {
                ref = attr.ref;
            }
            delete attr.ref;
        }
        delete props.methodRef;
        if (attr.key) {
            delete attr.key;
        }
        let hasChildren = false,
            propChildren = [];
        if (props.children) {
            hasChildren = true;
            propChildren = props.children;
            delete props.children;
        }
        let attrChildren = [];
        if (attr.children) {
            hasChildren = true;
            attrChildren = attr.children;
            delete attr.children;
        }
        props = Object.assign({}, props, attr, { className, ref });
        if (hasChildren) {
            props.children = [propChildren].concat([attrChildren]);
        }
        return React.createElement(element, props);
    };
}
exports.AttrElementWrapper = AttrElementWrapper;
const a = AttrElementWrapper("a"),
    button = AttrElementWrapper("button"),
    div = AttrElementWrapper("div"),
    footer = AttrElementWrapper("footer"),
    header = AttrElementWrapper("header"),
    input = AttrElementWrapper("input"),
    image = AttrElementWrapper("img"),
    label = AttrElementWrapper("label"),
    nav = AttrElementWrapper("nav"),
    option = AttrElementWrapper("option"),
    pre = AttrElementWrapper("pre"),
    section = AttrElementWrapper("section"),
    select = AttrElementWrapper("select"),
    span = AttrElementWrapper("span"),
    textarea = AttrElementWrapper("textarea");
exports.Elements = {
    a: a,
    button: button,
    div: div,
    footer: footer,
    header: header,
    input: input,
    image: image,
    label: label,
    nav: nav,
    option: option,
    pre: pre,
    section: section,
    select: select,
    span: span,
    textarea: textarea,
};
exports.default = exports.Elements;
