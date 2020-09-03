// Copyright (c) Microsoft. All rights reserved.

import Config from "app.config";
import { gridValueFormatters } from "components/shared/pcsGrid/pcsGridConfig";

const { checkForEmpty } = gridValueFormatters;

/** A collection of column definitions for the users grid */
export const userColumnDefs = {
    // id: {
    //   headerName: 'users.grid.userId',
    //   field: 'id',
    //   valueFormatter: ({ value }) => checkForEmpty(value)
    // },
    name: {
        headerName: "users.grid.userName",
        field: "name",
        sort: "asc",
        valueFormatter: ({ value }) => checkForEmpty(value),
    },
    role: {
        headerName: "users.grid.userRole",
        field: "role",
        valueFormatter: ({ value }) => checkForEmpty(value),
    },
    type: {
        headerName: "users.grid.userType",
        field: "type",
        valueFormatter: ({ value }) => checkForEmpty(value),
    },
};

/** Given a user object, extract and return the user Id */
export const getSoftSelectId = ({ Id, name }) => Id + name;

/** Shared user grid AgGrid properties */
export const defaultUserGridProps = {
    enableColResize: true,
    multiSelect: true,
    pagination: true,
    paginationPageSize: Config.paginationPageSize,
    rowSelection: "multiple",
};
