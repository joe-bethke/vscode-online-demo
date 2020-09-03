// Copyright (c) Microsoft. All rights reserved.

import { connect } from "react-redux";
import { withNamespaces } from "react-i18next";
import { SystemAdminNew } from "./systemAdminNew";

import {
    epics as usersEpics,
    redux as usersRedux,
    getAllNonSystemAdminsError,
    getAllNonSystemAdmins,
    getAllNonSystemAdminsPendingStatus,
} from "store/reducers/usersReducer";
import { epics as appEpics } from "store/reducers/appReducer";
// import { IdentityGatewayService } from "services";

// Pass the global info needed
const mapStateToProps = (state) => ({
        allNonSystemAdmins: getAllNonSystemAdmins(state),
        userError: getAllNonSystemAdminsError(state),
        isPending: getAllNonSystemAdminsPendingStatus(state),
    }),
    // Wrap the dispatch method
    mapDispatchToProps = (dispatch) => ({
        insertAllNonSystemAdmins: (users) =>
            dispatch(usersRedux.actions.insertAllNonSystemAdmins(users)),
        fetchAllNonSystemAdmins: () =>
            dispatch(usersEpics.actions.fetchAllNonSystemAdmins()),
        logEvent: (diagnosticsModel) => {
            return dispatch(appEpics.actions.logEvent(diagnosticsModel));
        },
    });

export const SystemAdminNewContainer = withNamespaces()(
    connect(mapStateToProps, mapDispatchToProps)(SystemAdminNew)
);
