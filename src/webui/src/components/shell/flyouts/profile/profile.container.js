// Copyright (c) Microsoft. All rights reserved.

import { withRouter } from "react-router-dom";
import { connect } from "react-redux";
import { withNamespaces } from "react-i18next";

import { AuthService, TenantService } from "services";
import { getUser } from "store/reducers/appReducer";
import { Profile } from "./profile";
import { epics as appEpics } from "store/reducers/appReducer";

import {
    epics as tenantsEpics,
    getTenants,
    getTenantsError,
    getTenantsPendingStatus,
    getTenantsLastUpdated,
    getCurrentTenantName,
} from "store/reducers/tenantsReducer";

function deleteTenantFlow(switchId) {
    TenantService.deleteTenant().subscribe(
        (response) => {
            AuthService.switchTenant(switchId);
            console.log(response);
        },
        (error) => {
            console.log(error);
            alert(
                "Unable to delete this tenant. It may not be fully deployed yet."
            );
        }
    );
}

function switchTenantFlow(switchId) {
    TenantService.tenantIsDeployed(switchId).subscribe(
        (response) => {
            console.log(response);
            if (response) {
                AuthService.switchTenant(switchId);
            } else {
                alert(
                    "The tenant you're trying to switch to is not fully deployed. Please wait a few minutes before trying to access your new tenant."
                );
            }
        },
        (error) => {
            console.log(error);
            alert("An error ocurred while trying to switch tenants.");
        }
    );
}

const mapStateToProps = (state) => ({
        user: getUser(state),
        tenants: getTenants(state),
        currentTenant: getCurrentTenantName(state),
        tenantsError: getTenantsError(state),
        isPending: getTenantsPendingStatus(state),
        lastUpdated: getTenantsLastUpdated(state),
        isSystemAdmin: getUser(state).isSystemAdmin,
    }),
    mapDispatchToProps = (dispatch) => ({
        fetchTenants: () => dispatch(tenantsEpics.actions.fetchTenants()),
        logout: () => AuthService.logout(),
        switchTenant: (tenant) => switchTenantFlow(tenant),
        createTenant: () => TenantService.createTenant(),
        processTenantDisplayValue: (tenant) =>
            TenantService.processDisplayValue(tenant),
        logEvent: (diagnosticsModel) =>
            dispatch(appEpics.actions.logEvent(diagnosticsModel)),
        deleteTenantThenSwitch: (switchId) => deleteTenantFlow(switchId),
        updateTenant: (tenant, tenantName) =>
            TenantService.updateTenant(tenant, tenantName),
    });

export const ProfileContainer = withRouter(
    withNamespaces()(connect(mapStateToProps, mapDispatchToProps)(Profile))
);
