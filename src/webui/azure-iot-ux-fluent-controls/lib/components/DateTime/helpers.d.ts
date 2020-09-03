import { DateFormat } from '../../Common';
export declare const placeholders: string[];
export declare const formatDate: (date: Date, format: DateFormat, localTimezone: boolean) => string;
export declare const replaceAt: (value: string, index: number, newValue: string) => string;
export declare const getLocalWeekdays: (locale: any) => any[];
export declare const getLocalMonths: (locale: any) => any[];
