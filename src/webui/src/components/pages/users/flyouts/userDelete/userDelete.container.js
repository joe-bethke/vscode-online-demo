// Copyright (c) Microsoft. All rights reserved.

import { connect } from "react-redux";
import { withNamespaces } from "react-i18next";
import { UserDelete } from "./userDelete";
import { redux as userRedux } from "store/reducers/usersReducer";

// Wrap the dispatch method
const mapDispatchToProps = (dispatch) => ({
    deleteUsers: (userIds) => dispatch(userRedux.actions.deleteUsers(userIds)),
});

export const UserDeleteContainer = withNamespaces()(
    connect(null, mapDispatchToProps)(UserDelete)
);
