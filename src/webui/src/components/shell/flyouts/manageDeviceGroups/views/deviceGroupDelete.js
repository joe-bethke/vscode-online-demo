// Copyright (c) Microsoft. All rights reserved.

import React, { Component } from "react";

import { Toggle } from "@microsoft/azure-iot-ux-fluent-controls/lib/components/Toggle";

import { ConfigService } from "services";
import { svgs } from "utilities";
import { permissions, toDiagnosticsModel } from "services/models";
import {
    AjaxError,
    Btn,
    BtnToolbar,
    Indicator,
    Protected,
    SectionDesc,
    SectionHeader,
    SummaryBody,
    SummarySection,
    Svg,
} from "components/shared";

export class DeviceGroupDelete extends Component {
    constructor(props) {
        super(props);
        this.state = {
            id: undefined,
            deviceGroupName: "",
            confirmStatus: false,
            isPending: false,
            error: undefined,
            changesApplied: false,
        };
    }

    componentDidMount() {
        this.setState({
            id: this.props.id,
            deviceGroupName: this.props.displayName,
        });
    }

    componentWillReceiveProps(nextProps) {
        this.setState({
            id: this.props.id,
            deviceGroupName: this.props.displayName,
        });
    }

    componentWillUnmount() {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }

    toggleConfirm = (value) => {
        this.setState({ confirmStatus: value });
    };

    deleteDeviceGroup = (event) => {
        event.preventDefault();
        this.setState({ isPending: true, error: null });
        this.props.logEvent(toDiagnosticsModel("DeviceGroup_Delete", {}));
        this.subscription = ConfigService.deleteDeviceGroup(
            this.state.id
        ).subscribe(
            (deletedGroupId) => {
                if (this.props.activeDeviceGroupId === deletedGroupId) {
                    this.props.changeDeviceGroup(this.props.deviceGroups[0].id);
                }
                this.props.deleteDeviceGroups([deletedGroupId]);
            },
            (error) =>
                this.setState({
                    error,
                    isPending: false,
                    changesApplied: true,
                }),
            () =>
                this.setState({
                    isPending: false,
                    changesApplied: true,
                    confirmStatus: false,
                })
        );
    };

    getSummaryMessage() {
        const { t } = this.props,
            { isPending, changesApplied } = this.state;

        if (isPending) {
            return t("deviceGroupsFlyout.delete.pending");
        } else if (changesApplied) {
            return t("deviceGroupsFlyout.delete.applySuccess");
        }
    }

    onCancel = () => {
        this.props.logEvent(
            toDiagnosticsModel("DeviceGroup_Cancel_Delete", {})
        );
        this.props.cancelDelete();
    };

    onClose = () => {
        this.props.logEvent(toDiagnosticsModel("DeviceGroup_Cancel", {}));
        this.props.closeDeviceGroup();
    };

    render() {
        const { t } = this.props,
            {
                id,
                deviceGroupName,
                confirmStatus,
                isPending,
                error,
                changesApplied,
            } = this.state,
            completedSuccessfully = changesApplied && !error,
            summaryMessage = this.getSummaryMessage();
        return (
            <Protected permission={permissions.deleteDevices}>
                <form
                    className="device-group-delete-container"
                    onSubmit={this.deleteDeviceGroup}
                >
                    <div className="device-group-delete-header">
                        {t("deviceGroupsFlyout.delete.header")}
                    </div>
                    <div className="device-group-delete-descr">
                        {t("deviceGroupsFlyout.delete.description", {
                            deviceGroupName,
                        })}
                    </div>
                    <br />
                    <Toggle
                        name="device-group-flyouts-delete"
                        attr={{
                            button: {
                                "aria-label": t(
                                    "deviceGroupsFlyout.delete.header"
                                ),
                            },
                        }}
                        on={confirmStatus}
                        onChange={this.toggleConfirm}
                        onLabel={t("deviceGroupsFlyout.delete.confirmYes")}
                        offLabel={t("deviceGroupsFlyout.delete.confirmNo")}
                    />

                    <SummarySection>
                        <SectionHeader>
                            {t("deviceGroupsFlyout.delete.summaryHeader")}
                        </SectionHeader>
                        <SummaryBody>
                            {!error && (
                                <SectionDesc>{summaryMessage}</SectionDesc>
                            )}
                            {this.state.isPending && <Indicator />}
                            {completedSuccessfully && (
                                <Svg
                                    className="summary-icon"
                                    path={svgs.apply}
                                />
                            )}
                        </SummaryBody>
                    </SummarySection>

                    {error && (
                        <AjaxError
                            className="device-group-delete-error"
                            t={t}
                            error={error}
                        />
                    )}
                    {!changesApplied && (
                        <BtnToolbar>
                            <Btn
                                svg={svgs.trash}
                                primary={true}
                                disabled={
                                    isPending ||
                                    id === undefined ||
                                    !confirmStatus
                                }
                                type="submit"
                            >
                                {t("deviceGroupsFlyout.delete.apply")}
                            </Btn>
                            <Btn svg={svgs.cancelX} onClick={this.onCancel}>
                                {t("deviceGroupsFlyout.delete.cancel")}
                            </Btn>
                        </BtnToolbar>
                    )}
                    {!!changesApplied && (
                        <BtnToolbar>
                            <Btn svg={svgs.cancelX} onClick={this.onClose}>
                                {t("deviceGroupsFlyout.delete.close")}
                            </Btn>
                        </BtnToolbar>
                    )}
                </form>
            </Protected>
        );
    }
}
