// Copyright (c) Microsoft. All rights reserved.

import React, { Component } from "react";

import { Btn } from "components/shared";
import { svgs } from "utilities";
import { toDiagnosticsModel } from "services/models";

export class CreateDeviceQueryBtn extends Component {
    onClick = () => {
        this.props.logEvent(toDiagnosticsModel("CreateDeviceQuery_Click", {}));
        this.setState({ openFlyoutName: "create-device-query" });
        this.props.openFlyout();
    };

    render() {
        return (
            <Btn svg={svgs.manageFilters} onClick={this.onClick}>
                {this.props.t("createDeviceQuery.title")}
            </Btn>
        );
    }
}
