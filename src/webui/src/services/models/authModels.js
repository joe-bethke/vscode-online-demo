// Copyright (c) Microsoft. All rights reserved.

import "polyfills";
import { camelCaseReshape } from "utilities";

export const permissions = {
    readAll: "ReadAll",

    createDeviceGroups: "CreateDeviceGroups",
    deleteDeviceGroups: "DeleteDeviceGroups",
    updateDeviceGroups: "UpdateDeviceGroups",

    createDevices: "CreateDevices",
    deleteDevices: "DeleteDevices",
    updateDevices: "UpdateDevices",

    createRules: "CreateRules",
    deleteRules: "DeleteRules",
    updateRules: "UpdateRules",

    deleteAlarms: "DeleteAlarms",
    updateAlarms: "UpdateAlarms",

    createJobs: "CreateJobs",

    updateSIMManagement: "UpdateSIMManagement",

    deletePackages: "DeletePackages",
    createPackages: "CreatePackages",

    createDeployments: "CreateDeployments",
    deleteDeployments: "DeleteDeployments",

    inviteUsers: "InviteUsers",
    deleteUsers: "DeleteUsers",

    deleteTenant: "DeleteTenant",

    enableAlerting: "EnableAlerting",
    disableAlerting: "DisableAlerting",
    sendC2DMessage: "SendC2DMessages",
    tagPackages: "TagPackages",
};

export const toUserModel = (user = {}) =>
    camelCaseReshape(user, {
        id: "id",
        email: "email",
        name: "name",
        roles: "roles",
        allowedActions: "permissions",
        tenant: "tenant",
        availableTenants: "availableTenants",
        token: "token",
        isSystemAdmin: "isSystemAdmin",
    });

//When authentication is disabled, this user has all permissions.
export const authDisabledUser = {
    id: "AuthIsDisabled",
    email: "authdisabled@iot.auth",
    name: "Disabled Auth",
    roles: ["Admin"],
    permissions: Object.values(permissions),
};
