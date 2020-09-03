// Copyright (c) Microsoft. All rights reserved.

import { connect } from "react-redux";
import { withNamespaces } from "react-i18next";
import { PackageNew } from "./packageNew";
import { getActiveDeviceGroupId, getTheme } from "store/reducers/appReducer";
import {
    getCreatePackageError,
    getCreatePackagePendingStatus,
    epics as packagesEpics,
    redux as packagesRedux,
    getConfigTypes,
    getConfigTypesError,
    getConfigTypesPendingStatus,
} from "store/reducers/packagesReducer";
import { epics as appEpics } from "store/reducers/appReducer";

// Pass the global info needed
const mapStateToProps = (state) => ({
        theme: getTheme(state),
        isPending: getCreatePackagePendingStatus(state),
        deviceGroup: getActiveDeviceGroupId(state),
        error: getCreatePackageError(state),
        configTypes: getConfigTypes(state),
        configTypesError: getConfigTypesError(state),
        configTypesIsPending: getConfigTypesPendingStatus(state),
    }),
    // Wrap the dispatch methods
    mapDispatchToProps = (dispatch) => ({
        createPackage: (packageModel) =>
            dispatch(packagesEpics.actions.createPackage(packageModel)),
        resetPackagesPendingError: () =>
            dispatch(
                packagesRedux.actions.resetPendingAndError(
                    packagesEpics.actionTypes.createPackage
                )
            ),
        logEvent: (diagnosticsModel) =>
            dispatch(appEpics.actions.logEvent(diagnosticsModel)),
        fetchConfigTypes: () =>
            dispatch(packagesEpics.actions.fetchConfigTypes()),
    });

export const PackageNewContainer = withNamespaces()(
    connect(mapStateToProps, mapDispatchToProps)(PackageNew)
);
