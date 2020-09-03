// Copyright (c) Microsoft. All rights reserved.

import { connect } from "react-redux";
import { withNamespaces } from "react-i18next";
import { SystemAdminDelete } from "./systemAdminDelete";
import {
    epics as usersEpics,
    redux as userRedux,
    getAllSystemAdmins,
    getAllSystemAdminsError,
    getAllSystemAdminsPendingStatus,
} from "store/reducers/usersReducer";
import { getUser } from "store/reducers/appReducer";

// Wrap the dispatch method
const mapStateToProps = (state) => ({
        allSystemAdmins: getAllSystemAdmins(state),
        userError: getAllSystemAdminsError(state),
        isPending: getAllSystemAdminsPendingStatus(state),
        loggedInUser: getUser(state),
        loggedInUserId: getUser(state).id,
    }),
    mapDispatchToProps = (dispatch) => ({
        deleteUsers: (userIds) =>
            dispatch(userRedux.actions.deleteUsers(userIds)),
        getAllSystemAdmins: () =>
            dispatch(usersEpics.actions.fetchAllSystemAdmins()),
    });

export const SystemAdminDeleteContainer = withNamespaces()(
    connect(mapStateToProps, mapDispatchToProps)(SystemAdminDelete)
);
