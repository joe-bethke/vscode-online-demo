/// <reference types="@types/react" />
import * as React from 'react';
export interface AccordionAttributes {
    ariaRole?: string;
    dataTestHook?: string;
}
export interface AccordionProperties {
    children: React.ReactElement<any> | Array<React.ReactElement<any>> | React.ReactChildren | React.ReactNode;
    expanded?: boolean;
    id: string;
    label: string;
    onToggle: () => void;
    attr?: AccordionAttributes;
}
export declare class Accordion extends React.PureComponent<AccordionProperties> {
    static defaultProps: Partial<AccordionProperties>;
    render(): JSX.Element;
}
