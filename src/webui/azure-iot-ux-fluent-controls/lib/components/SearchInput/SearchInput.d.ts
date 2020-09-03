/// <reference types="@types/react" />
import * as React from 'react';
import { TextInputType, TextInputAttributes } from '../Input/TextInput';
export declare const prefixClassName: string;
export declare const postfixClassName: string;
export interface SearchInputProps extends React.Props<TextInputType> {
    label: string;
    onSubmit: React.EventHandler<any>;
    onChange: (newValue: string) => void;
    value: string;
    containerClassName?: string;
    inputClassName?: string;
    attr?: TextInputAttributes;
}
export declare class SearchInput extends React.PureComponent<SearchInputProps> {
    render(): JSX.Element;
}
export default SearchInput;
