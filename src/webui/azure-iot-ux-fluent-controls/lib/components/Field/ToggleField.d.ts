/// <reference types="@types/react" />
import * as React from 'react';
import { MethodNode } from '../../Common';
import { FormFieldAttributes } from './FormField';
import { ToggleAttributes } from '../Toggle';
export interface ToggleFieldType {
}
export interface ToggleFieldProps extends React.Props<ToggleFieldType> {
    /** HTML form element name */
    name: string;
    /**
     * Current value of HTML select element
     *
     * This must be an `Object` that is in `SelectInputProps.options`
     */
    value: boolean;
    /** Label to display above input element */
    label: MethodNode;
    /** Error to display below input element */
    error?: MethodNode;
    /** Error HTML title in case of overflow */
    errorTitle?: string;
    onLabel?: MethodNode;
    offLabel?: MethodNode;
    /** Disable HTML input element */
    disabled?: boolean;
    /** Form field is required (appends a red asterisk to the label) */
    required?: boolean;
    /** Display horizontal loading animation instead of error */
    loading?: boolean;
    /** Auto Focus */
    autoFocus?: boolean;
    /** Tooltip text to display in info icon bubble */
    tooltip?: MethodNode;
    /** Callback for `onChange` events */
    onChange: (newValue: any) => void;
    /** Classname to append to top level element */
    className?: string;
    /** Classname to append to top level element of SelectInput */
    inputClassName?: string;
    /** React node to render at the far side of the label. */
    labelFarSide?: React.ReactNode;
    attr?: FormFieldAttributes & ToggleAttributes;
}
/**
 * High level form toggle switch control
 *
 * @param props: Object fulfilling `ToggleFieldProps` interface
 */
export declare const ToggleField: React.StatelessComponent<ToggleFieldProps>;
export default ToggleField;
