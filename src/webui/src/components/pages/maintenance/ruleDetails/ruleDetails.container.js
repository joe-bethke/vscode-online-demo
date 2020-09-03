// Copyright (c) Microsoft. All rights reserved.

import { connect } from "react-redux";
import { RuleDetails } from "./ruleDetails";
import {
    epics as appEpics,
    getActiveDeviceQueryConditions,
} from "store/reducers/appReducer";

const mapStateToProps = (state) => ({
        activeDeviceQueryConditions: getActiveDeviceQueryConditions(state),
    }),
    // Wrap the dispatch method
    mapDispatchToProps = (dispatch) => ({
        logEvent: (diagnosticsModel) =>
            dispatch(appEpics.actions.logEvent(diagnosticsModel)),
    });

export const RuleDetailsContainer = connect(
    mapStateToProps,
    mapDispatchToProps
)(RuleDetails);
