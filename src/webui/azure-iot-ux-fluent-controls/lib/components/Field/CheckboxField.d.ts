/// <reference types="@types/react" />
import * as React from 'react';
import { MethodNode } from '../../Common';
import { CheckboxInputAttributes } from '../Input/CheckboxInput';
import { FormFieldAttributes } from './FormField';
export interface CheckboxFieldType {
}
export interface CheckboxFieldProps extends React.Props<CheckboxFieldType> {
    /** HTML form element name */
    name: string;
    /**
     * Current value of HTML checkbox element
     *
     * This must be an `Object` that is in `CheckboxFieldProps.options`
     */
    value: boolean;
    /** Label to display above input element */
    label: MethodNode;
    /** Error to display below input element */
    error?: MethodNode;
    /** Error HTML title in case of overflow */
    errorTitle?: string;
    /** Disable HTML input element */
    disabled?: boolean;
    /** Form field is required (appends a red asterisk to the label) */
    required?: boolean;
    /** Display horizontal loading animation instead of error */
    loading?: boolean;
    /** Autofocus */
    autoFocus?: boolean;
    /** Tooltip text to display in info icon bubble */
    tooltip?: MethodNode;
    /** Callback for HTML checkbox element `onChange` events */
    onChange: (newValue: boolean) => void;
    /** Classname to append to top level element */
    className?: string;
    /** Classname to append to top level element of CheckboxInput */
    inputClassName?: string;
    /** React node to render at the far side of the label. */
    labelFarSide?: React.ReactNode;
    attr?: FormFieldAttributes & CheckboxInputAttributes;
}
/**
 * High level form checkbox control
 *
 * IMPORTANT: The options provided to this control must all be UNIQUE. The
 * `value` property of checkboxes is the numerical index of the option in
 * `CheckboxField.options` so `CheckboxField.value` is compared to each value in
 * `options` (===) to decide which option is the one currently selected.
 *
 * @param props: Object fulfilling `CheckboxFieldProps` interface
 */
export declare const CheckboxField: React.StatelessComponent<CheckboxFieldProps>;
export default CheckboxField;
