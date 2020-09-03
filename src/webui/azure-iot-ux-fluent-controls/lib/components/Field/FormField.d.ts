/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps } from '../../Attributes';
import { MethodNode } from '../../Common';
import { FormLabelAttributes } from './FormLabel';
import { BalloonPosition, BalloonAlignment } from '../Balloon';
export interface FormFieldType {
}
export interface FormFieldAttributes {
    fieldContainer?: DivProps;
    fieldLabel?: FormLabelAttributes;
    fieldContent?: DivProps;
    fieldError?: DivProps;
    fieldTooltip?: {
        balloonPosition?: BalloonPosition;
        balloonAlignment?: BalloonAlignment;
    };
}
export interface FormFieldProps extends React.Props<FormFieldType> {
    /** HTML element name for label accessibility */
    name: string;
    /** Label to display above input element */
    label?: MethodNode;
    /** Error to display below input element */
    error?: MethodNode;
    /** Error HTML title in case of overflow */
    errorTitle?: string;
    /** Display horizontal loading animation instead of error */
    loading?: boolean;
    /** Appends a red asterisk to the label if it is a string */
    required?: boolean;
    /** Set error field to display: none */
    hideError?: boolean;
    /** Tooltip text to display in info icon bubble */
    tooltip?: MethodNode;
    /** Classname to append to top level element */
    className?: string;
    /** Classname to append to top level error element */
    errorClassName?: string;
    /** React node to render at the far side of the label. */
    labelFarSide?: React.ReactNode;
    attr?: FormFieldAttributes;
}
export interface FormFieldState {
    tooltipVisible: boolean;
}
/**
 * High level generic form field
 *
 * @param props Control properties (defined in `FormFieldProps` interface)
 */
export declare class FormField extends React.PureComponent<FormFieldProps, FormFieldState> {
    static defaultProps: {
        name: any;
        label: any;
        loading: boolean;
        hideError: boolean;
        attr: {
            fieldContainer: {};
            fieldLabel: {};
            fieldContent: {};
            fieldError: {};
        };
    };
    private _self;
    constructor(props: FormFieldProps);
    handleKeyDown(e: React.KeyboardEvent<any>): void;
    handleBlur(e: FocusEvent): void;
    render(): JSX.Element;
}
export default FormField;
