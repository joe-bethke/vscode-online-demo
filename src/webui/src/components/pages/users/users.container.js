// Copyright (c) Microsoft. All rights reserved.

import { connect } from "react-redux";
import { withNamespaces } from "react-i18next";
import { Users } from "./users";
// import {
//   epics as usersEpics,
//   getDevices,
//   getDevicesError,
//   getDevicesLastUpdated,
//   getDevicesPendingStatus
// } from 'store/reducers/devicesReducer';
import {
    epics as usersEpics,
    getUsers,
    getUsersError,
    getUsersLastUpdated,
    getUsersPendingStatus,
} from "store/reducers/usersReducer";
import {
    redux as appRedux,
    epics as appEpics,
    getUser,
} from "store/reducers/appReducer";

// Pass the devices status
// const mapStateToProps = state => ({
//   users: getDevices(state),
//   userError: getDevicesError(state),
//   isPending: getDevicesPendingStatus(state),
//   lastUpdated: getDevicesLastUpdated(state)
// });
const mapStateToProps = (state) => ({
        users: getUsers(state),
        userError: getUsersError(state),
        isPending: getUsersPendingStatus(state),
        lastUpdated: getUsersLastUpdated(state),
        isSystemAdmin: getUser(state).isSystemAdmin,
    }),
    // Wrap the dispatch method
    mapDispatchToProps = (dispatch) => ({
        fetchUsers: () => dispatch(usersEpics.actions.fetchUsers()),
        updateCurrentWindow: (currentWindow) =>
            dispatch(appRedux.actions.updateCurrentWindow(currentWindow)),
        logEvent: (diagnosticsModel) =>
            dispatch(appEpics.actions.logEvent(diagnosticsModel)),
    });
export const UsersContainer = withNamespaces()(
    connect(mapStateToProps, mapDispatchToProps)(Users)
);
