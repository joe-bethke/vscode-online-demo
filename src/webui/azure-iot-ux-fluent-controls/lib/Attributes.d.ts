/// <reference types="@types/react" />
import * as React from 'react';
export declare type AttrProps<T extends HTMLElement> = React.HTMLProps<T> & React.ClassAttributes<T> & any;
export declare type AnchorProps = AttrProps<HTMLAnchorElement>;
export declare type ButtonProps = AttrProps<HTMLButtonElement>;
export declare type DivProps = AttrProps<HTMLDivElement>;
export declare type FooterProps = AttrProps<HTMLDivElement>;
export declare type HeaderProps = AttrProps<HTMLDivElement>;
export declare type InputProps = AttrProps<HTMLInputElement>;
export declare type ImageProps = AttrProps<HTMLImageElement>;
export declare type LabelProps = AttrProps<HTMLLabelElement>;
export declare type NavProps = AttrProps<HTMLDivElement>;
export declare type OptionProps = AttrProps<HTMLOptionElement>;
export declare type PreProps = AttrProps<HTMLPreElement>;
export declare type SectionProps = AttrProps<HTMLDivElement>;
export declare type SelectProps = AttrProps<HTMLSelectElement>;
export declare type SpanProps = AttrProps<HTMLSpanElement>;
export declare type TextAreaProps = AttrProps<HTMLTextAreaElement>;
export declare type HTMLElementAttr<T extends HTMLElement> = AttrProps<T> & {
    ref?: React.Ref<T>;
};
export declare type AttrWrapperProps<T extends HTMLElement> = HTMLElementAttr<T> & {
    attr?: any;
    methodRef?: React.Ref<T>;
};
export declare type AttrWrapper<T extends HTMLElement> = (props: AttrWrapperProps<T>) => React.DOMElement<AttrWrapperProps<T>, T>;
export interface OptionAttr<T> {
    attr?: T;
}
export declare function mergeAttributeObjects<T, K extends keyof T>(leftInput: T, rightInput: T, names: K[]): T;
export declare function mergeAttributes<T extends HTMLElement>(leftAttr: HTMLElementAttr<T>, rightAttr: HTMLElementAttr<T>): HTMLElementAttr<T>;
export declare function AttrElementWrapper<T extends HTMLElement>(element: string): AttrWrapper<T>;
export declare const Elements: {
    a: AttrWrapper<HTMLAnchorElement>;
    button: AttrWrapper<HTMLButtonElement>;
    div: AttrWrapper<HTMLDivElement>;
    footer: AttrWrapper<HTMLDivElement>;
    header: AttrWrapper<HTMLDivElement>;
    input: AttrWrapper<HTMLInputElement>;
    image: AttrWrapper<HTMLInputElement>;
    label: AttrWrapper<HTMLLabelElement>;
    nav: AttrWrapper<HTMLDivElement>;
    option: AttrWrapper<HTMLOptionElement>;
    pre: AttrWrapper<HTMLPreElement>;
    section: AttrWrapper<HTMLDivElement>;
    select: AttrWrapper<HTMLSelectElement>;
    span: AttrWrapper<HTMLSpanElement>;
    textarea: AttrWrapper<HTMLTextAreaElement>;
};
export default Elements;
