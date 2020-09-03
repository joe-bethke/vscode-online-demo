/// <reference types="@types/react" />
import * as React from 'react';
import { GenericManagementListProps } from './GenericManagementList';
export interface ManagementListComponentType {
}
export declare type ManagementListProps = GenericManagementListProps<any>;
/**
 * Management List
 *
 * If you need type checking, you should use `GenericManagementList` with the
 * `CreateManagementList` function instead.
 *
 * Check `GenericManagementList` for documentation on properties
 *
 * @param props Control properties (defined in `ManagementListProps` interface)
 */
export declare const ManagementList: React.StatelessComponent<ManagementListProps>;
export default ManagementList;
