/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps } from '../../Attributes';
export interface FormErrorType {
}
export interface FormErrorAttributes {
    container?: DivProps;
}
export interface FormErrorProps extends React.Props<FormErrorType> {
    /** Title tag for error in case of overflow */
    title?: string;
    /** Hide error */
    hidden?: boolean;
    /** Classname to append to top level element */
    className?: string;
    attr?: FormErrorAttributes;
}
/**
 * Form Error
 *
 * @param props Control properties (defined in `FormErrorProps` interface)
 */
export declare const FormError: React.StatelessComponent<FormErrorProps>;
export default FormError;
