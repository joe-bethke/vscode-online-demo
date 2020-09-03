// Copyright (c) Microsoft. All rights reserved.

import { connect } from "react-redux";
import { UsersGrid } from "./usersGrid";
import { epics as appEpics } from "store/reducers/appReducer";

// Wrap the dispatch method
const mapDispatchToProps = (dispatch) => ({
    logEvent: (diagnosticsModel) =>
        dispatch(appEpics.actions.logEvent(diagnosticsModel)),
});

export const UsersGridContainer = connect(null, mapDispatchToProps)(UsersGrid);
