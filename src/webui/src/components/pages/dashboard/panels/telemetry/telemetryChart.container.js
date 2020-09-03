// Copyright (c) Microsoft. All rights reserved.

import { connect } from "react-redux";
import {
    epics as appEpics,
    getActiveDeviceGroup,
} from "store/reducers/appReducer";
import { TelemetryChart } from "./telemetryChart";

const mapStateToProps = (state) => ({
        deviceGroup: getActiveDeviceGroup(state),
    }),
    mapDispatchToProps = (dispatch) => ({
        logEvent: (diagnosticsModel) =>
            dispatch(appEpics.actions.logEvent(diagnosticsModel)),
    });

export const TelemetryChartContainer = connect(
    mapStateToProps,
    mapDispatchToProps
)(TelemetryChart);
