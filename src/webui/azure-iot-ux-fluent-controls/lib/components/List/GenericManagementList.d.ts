/// <reference types="@types/react" />
import * as React from 'react';
import { DivProps, ButtonProps, OptionAttr } from '../../Attributes';
import { IconAttributes } from '../Icon';
import { MethodNode, GridColumn, SortDirection } from '../../Common';
import { CheckboxInputAttributes } from '../Input/CheckboxInput';
export interface GenericManagementListComponentType {
}
export interface GenericManagementListAttributes {
    container?: DivProps;
    column?: DivProps;
    rowContent?: DivProps;
    rowHeaderButton?: ButtonProps;
    rowHeaderChevron?: IconAttributes;
    selectAllEmpty?: DivProps;
    selectAllCheckbox?: CheckboxInputAttributes;
    selectAllContainer?: DivProps;
    selectRowContent?: DivProps;
    selectRowCheckbox?: CheckboxInputAttributes;
}
export interface GenericManagementListProps<T> extends React.Props<GenericManagementListComponentType> {
    /**
     * List of `GridColumn` objects that provide mappings from row type T to
     * column values and sorting
     *
     * See documentation for GridColumn<T> for more information
     */
    columns: Array<GridColumn<T> & OptionAttr<DivProps>>;
    /**
     * List of row objects
     *
     * This can be a list of anything that satisfies the GridColumn callbacks
     * provided in props.columns
     */
    rows: Array<T & OptionAttr<DivProps>>;
    /**
     * HTML input element name prefix to use for checkboxes
     *
     * default: management-list
     */
    name?: string;
    /**
     * Callback for checkbox value changes
     *
     * If this callback is not provided, row selection checkboxes will not be shown
     */
    onSelect?: (row: T, newValue: boolean) => void;
    /**
     * Callback for select all checkbox value changes
     *
     * If this callback is not provided, select all checkbox will not be shown
     */
    onSelectAll?: (allSelected: boolean) => void;
    /**
     * A key of row type `T` or callback that returns whether a row is selected.
     *
     * If this is not provided, row selection checkboxes will not be shown
     */
    isSelected?: ((row: T) => boolean) | keyof T;
    /**
     * A key of row type `T` or callback that returns the label for the select checkbox
     * for accessibility.
     *
     * If this is not provided, no label will be rendered.
     */
    selectLabel?: ((row: T) => MethodNode) | keyof T;
    /**
     * A label for the select all checkbox for accessibility
     */
    selectAllLabel?: MethodNode;
    /**
     * Currently sorted column
     */
    sortedColumn?: GridColumn<T>;
    /**
     * Direction of current sort
     *
     * 'ascending' | 'descending'
     *
     * Default: 'ascending'
     */
    sortDirection?: SortDirection;
    /** Classname to append to top level element */
    className?: string;
    attr?: GenericManagementListAttributes;
}
/**
 * Generic Management List
 *
 * To use this component in TSX:
 *
 * ```ts
 * type CustomManagementList = GenericManagementList<Type>;
 *
 * <CustomManagementList rows={Type[]} columns={GridColumn<Type>} />
 * ```
 *
 * If you don't need type checking, you should use `ManagementList` instead.
 *
 * @param props Control properties (defined in `GenericManagementListProps` interface)
 */
export declare class GenericManagementList<T> extends React.PureComponent<GenericManagementListProps<T>, {}> {
    static defaultProps: {
        name: string;
        selectAllLabel: string;
        selectLabel: () => string;
        defaultDirection: string;
        attr: {
            container: {};
            column: {};
            rowHeader: {};
            rowContent: {};
            rowHeaderChevron: {};
            selectAllContainer: {};
            selectAllEmptyContainer: {};
            selectAllCheckbox: {};
            selectRowContainer: {};
            selectRowCheckbox: {};
        };
    };
    constructor(props: Readonly<GenericManagementListProps<T>>);
    render(): JSX.Element;
}
export default GenericManagementList;
