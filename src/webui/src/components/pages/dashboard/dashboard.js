// Copyright (c) Microsoft. All rights reserved.

import React, { Component } from "react";
import { Observable, Subject } from "rxjs";
import moment from "moment";

import Config from "app.config";
import { TelemetryService, IdentityGatewayService } from "services";
import { permissions } from "services/models";
import {
    compareByProperty,
    getIntervalParams,
    retryHandler,
    getDeviceGroupParam,
} from "utilities";
import { Grid, Cell } from "./grid";
import { PanelErrorBoundary } from "./panel";
import { DeviceGroupDropdownContainer as DeviceGroupDropdown } from "components/shell/deviceGroupDropdown";
import { ManageDeviceGroupsBtnContainer as ManageDeviceGroupsBtn } from "components/shell/manageDeviceGroupsBtn";
import { TimeIntervalDropdownContainer as TimeIntervalDropdown } from "components/shell/timeIntervalDropdown";
import { ResetActiveDeviceQueryBtnContainer as ResetActiveDeviceQueryBtn } from "components/shell/resetActiveDeviceQueryBtn";
import {
    OverviewPanel,
    AlertsPanelContainer as AlertsPanel,
    TelemetryPanel,
    AnalyticsPanel,
    MapPanelContainer as MapPanel,
    ExamplePanel,
    transformTelemetryResponse,
    chartColorObjects,
} from "./panels";
import {
    ComponentArray,
    ContextMenu,
    ContextMenuAlign,
    PageContent,
    Protected,
    RefreshBarContainer as RefreshBar,
} from "components/shared";
import { CreateDeviceQueryBtnContainer as CreateDeviceQueryBtn } from "components/shell/createDeviceQueryBtn";

import "./dashboard.scss";

const initialState = {
        // Telemetry data
        telemetry: {},
        telemetryIsPending: true,
        telemetryError: null,
        telemetryQueryExceededLimit: false,

        // Analytics data
        analyticsVersion: 0,
        currentActiveAlerts: [],
        topAlerts: [],
        alertsPerDeviceId: {},
        criticalAlertsChange: 0,
        analyticsIsPending: true,
        analyticsError: null,

        // Create data
        openWarningCount: undefined,
        openCriticalCount: undefined,

        // Map data
        devicesInAlert: {},

        lastRefreshed: undefined,
        selectedDeviceGroupId: undefined,
    },
    refreshEvent = (deviceIds = [], timeInterval) => ({
        deviceIds,
        timeInterval,
    }),
    { retryWaitTime, maxRetryAttempts } = Config;

export class Dashboard extends Component {
    constructor(props) {
        super(props);

        this.state = initialState;

        this.subscriptions = [];
        this.dashboardRefresh$ = new Subject(); // Restarts all streams
        this.telemetryRefresh$ = new Subject();
        this.panelsRefresh$ = new Subject();

        this.props.updateCurrentWindow("Dashboard");
    }

    componentWillMount() {
        if (this.props.location.search) {
            this.setState({
                selectedDeviceGroupId: getDeviceGroupParam(
                    this.props.location.search
                ),
            });
        }
        IdentityGatewayService.VerifyAndRefreshCache();
    }

    componentDidMount() {
        if (this.state.selectedDeviceGroupId) {
            window.history.replaceState(
                {},
                document.title,
                this.props.location.pathname
            );
        }

        // Ensure the rules are loaded
        this.refreshRules();

        // Telemetry stream - START
        const onPendingStart = () =>
                this.setState({ telemetryIsPending: true }),
            getTelemetryStream = ({ deviceIds = [] }) =>
                deviceIds.length === 0
                    ? Observable.from(() => {})
                    : TelemetryService.getTelemetryByDeviceId(
                          deviceIds,
                          TimeIntervalDropdown.getTimeIntervalDropdownValue()
                      )
                          .flatMap((items) => {
                              this.setState({
                                  telemetryQueryExceededLimit:
                                      items.length >=
                                      Config.telemetryQueryResultLimit,
                              });
                              return Observable.of(items);
                          })
                          .merge(
                              this.telemetryRefresh$ // Previous request complete
                                  .delay(Config.telemetryRefreshInterval) // Wait to refresh
                                  .do(onPendingStart)
                                  .flatMap((_) =>
                                      TelemetryService.getTelemetryByDeviceIdP1M(
                                          deviceIds
                                      )
                                  )
                          )
                          .flatMap(
                              transformTelemetryResponse(
                                  () => this.state.telemetry
                              )
                          )
                          .map((telemetry) => ({
                              telemetry,
                              telemetryIsPending: false,
                          })) // Stream emits new state
                          // Retry any retryable errors
                          .retryWhen(
                              retryHandler(maxRetryAttempts, retryWaitTime)
                          ),
            // Telemetry stream - END

            // Analytics stream - START
            getAnalyticsStream = ({ deviceIds = [], timeInterval }) =>
                this.panelsRefresh$
                    .delay(Config.dashboardRefreshInterval)
                    .startWith(0)
                    .do((_) => this.setState({ analyticsIsPending: true }))
                    .flatMap((_) => {
                        const devices = deviceIds.length
                                ? deviceIds.join(",")
                                : undefined,
                            [
                                currentIntervalParams,
                                previousIntervalParams,
                            ] = getIntervalParams(timeInterval),
                            currentParams = {
                                ...currentIntervalParams,
                                devices,
                            },
                            previousParams = {
                                ...previousIntervalParams,
                                devices,
                            };
                        if (this.props.alerting.isActive) {
                            return Observable.forkJoin(
                                TelemetryService.getActiveAlerts(currentParams),
                                TelemetryService.getActiveAlerts(
                                    previousParams
                                ),

                                TelemetryService.getAlerts(currentParams),
                                TelemetryService.getAlerts(previousParams)
                            );
                        } else {
                            return Observable.forkJoin([], [], [], []);
                        }
                    })
                    .map(
                        ([
                            currentActiveAlerts,
                            previousActiveAlerts,

                            currentAlerts,
                            previousAlerts,
                        ]) => {
                            // Process all the data out of the currentAlerts list
                            const currentAlertsStats = currentAlerts.reduce(
                                    (acc, alert) => {
                                        const isOpen =
                                                alert.status ===
                                                Config.alertStatus.open,
                                            isWarning =
                                                alert.severity ===
                                                Config.ruleSeverity.warning,
                                            isCritical =
                                                alert.severity ===
                                                Config.ruleSeverity.critical;
                                        let updatedAlertsPerDeviceId =
                                            acc.alertsPerDeviceId;
                                        if (alert.deviceId) {
                                            updatedAlertsPerDeviceId = {
                                                ...updatedAlertsPerDeviceId,
                                                [alert.deviceId]:
                                                    (updatedAlertsPerDeviceId[
                                                        alert.deviceId
                                                    ] || 0) + 1,
                                            };
                                        }
                                        return {
                                            openWarningCount:
                                                (acc.openWarningCount || 0) +
                                                (isWarning && isOpen ? 1 : 0),
                                            openCriticalCount:
                                                (acc.openCriticalCount || 0) +
                                                (isCritical && isOpen ? 1 : 0),
                                            totalCriticalCount:
                                                (acc.totalCriticalCount || 0) +
                                                (isCritical ? 1 : 0),
                                            alertsPerDeviceId: updatedAlertsPerDeviceId,
                                        };
                                    },
                                    { alertsPerDeviceId: {} }
                                ),
                                // ================== Critical Alerts Count - START
                                currentCriticalAlerts =
                                    currentAlertsStats.totalCriticalCount,
                                previousCriticalAlerts = previousAlerts.reduce(
                                    (cnt, { severity }) =>
                                        severity ===
                                        Config.ruleSeverity.critical
                                            ? cnt + 1
                                            : cnt,
                                    0
                                ),
                                criticalAlertsChange = (
                                    ((currentCriticalAlerts -
                                        previousCriticalAlerts) /
                                        currentCriticalAlerts) *
                                    100
                                ).toFixed(2),
                                // ================== Critical Alerts Count - END

                                // ================== Top Alerts - START
                                currentTopAlerts = currentActiveAlerts
                                    .sort(compareByProperty("count"))
                                    .slice(0, Config.maxTopAlerts),
                                // Find the previous counts for the current top analytics
                                previousTopAlertsMap = previousActiveAlerts.reduce(
                                    (acc, { ruleId, count }) =>
                                        ruleId in acc
                                            ? { ...acc, [ruleId]: count }
                                            : acc,
                                    currentTopAlerts.reduce(
                                        (acc, { ruleId }) => ({
                                            ...acc,
                                            [ruleId]: 0,
                                        }),
                                        {}
                                    )
                                ),
                                topAlerts = currentTopAlerts.map(
                                    ({ ruleId, count }) => ({
                                        ruleId,
                                        count,
                                        previousCount:
                                            previousTopAlertsMap[ruleId] || 0,
                                    })
                                ),
                                // ================== Top Alerts - END

                                devicesInAlert = currentAlerts
                                    .filter(
                                        ({ status }) =>
                                            status === Config.alertStatus.open
                                    )
                                    .reduce(
                                        (
                                            acc,
                                            { deviceId, severity, ruleId }
                                        ) => {
                                            return {
                                                ...acc,
                                                [deviceId]: {
                                                    severity,
                                                    ruleId,
                                                },
                                            };
                                        },
                                        {}
                                    );

                            return {
                                analyticsIsPending: false,
                                analyticsVersion:
                                    this.state.analyticsVersion + 1,

                                // Analytics data
                                currentActiveAlerts,
                                topAlerts,
                                criticalAlertsChange,
                                alertsPerDeviceId:
                                    currentAlertsStats.alertsPerDeviceId,

                                // Create data
                                openWarningCount:
                                    currentAlertsStats.openWarningCount,
                                openCriticalCount:
                                    currentAlertsStats.openCriticalCount,

                                // Map data
                                devicesInAlert,
                            };
                        }
                    )
                    // Retry any retryable errors
                    .retryWhen(retryHandler(maxRetryAttempts, retryWaitTime));
        // Analytics stream - END

        this.subscriptions.push(
            this.dashboardRefresh$.subscribe(() => this.setState(initialState))
        );

        this.subscriptions.push(
            this.dashboardRefresh$.switchMap(getTelemetryStream).subscribe(
                (telemetryState) =>
                    this.setState(
                        { ...telemetryState, lastRefreshed: moment() },
                        () => this.telemetryRefresh$.next("r")
                    ),
                (telemetryError) =>
                    this.setState({ telemetryError, telemetryIsPending: false })
            )
        );

        this.subscriptions.push(
            this.dashboardRefresh$.switchMap(getAnalyticsStream).subscribe(
                (analyticsState) =>
                    this.setState(
                        { ...analyticsState, lastRefreshed: moment() },
                        () => this.panelsRefresh$.next("r")
                    ),
                (analyticsError) =>
                    this.setState({ analyticsError, analyticsIsPending: false })
            )
        );

        // Start polling all panels
        if (this.props.deviceLastUpdated) {
            this.dashboardRefresh$.next(
                refreshEvent(
                    Object.keys(this.props.devices || {}),
                    this.props.timeInterval
                )
            );
        }
    }

    componentWillUnmount() {
        this.subscriptions.forEach((sub) => sub.unsubscribe());
    }

    componentWillReceiveProps(nextProps) {
        if (
            nextProps.deviceLastUpdated !== this.props.deviceLastUpdated ||
            nextProps.timeInterval !== this.props.timeInterval
        ) {
            this.dashboardRefresh$.next(
                refreshEvent(
                    Object.keys(nextProps.devices),
                    nextProps.timeInterval
                )
            );
        }
    }

    refreshDashboard = () =>
        this.dashboardRefresh$.next(
            refreshEvent(
                Object.keys(this.props.devices),
                this.props.timeInterval
            )
        );

    refreshRules = () => {
        if (!this.props.rulesError && !this.props.rulesIsPending) {
            this.props.fetchRules();
        }
    };

    render() {
        const {
                alerting,
                theme,
                timeInterval,
                timeSeriesExplorerUrl,

                azureMapsKey,
                azureMapsKeyError,
                azureMapsKeyIsPending,

                devices,
                devicesError,
                devicesIsPending,

                activeDeviceGroup,
                deviceGroups,
                deviceGroupError,

                rules,
                rulesError,
                rulesIsPending,
                t,
            } = this.props,
            {
                telemetry,
                telemetryIsPending,
                telemetryError,
                telemetryQueryExceededLimit,

                analyticsVersion,
                currentActiveAlerts,
                topAlerts,
                alertsPerDeviceId,
                criticalAlertsChange,
                analyticsIsPending,
                analyticsError,

                openWarningCount,
                openCriticalCount,

                devicesInAlert,

                lastRefreshed,
            } = this.state,
            // Count the number of online and offline devices
            deviceIds = Object.keys(devices),
            onlineDeviceCount = deviceIds.length
                ? deviceIds.reduce(
                      (count, deviceId) =>
                          devices[deviceId].connected ? count + 1 : count,
                      0
                  )
                : undefined,
            offlineDeviceCount = deviceIds.length
                ? deviceIds.length - onlineDeviceCount
                : undefined,
            // Add parameters to Time Series Insights Url
            timeSeriesParamUrl = timeSeriesExplorerUrl
                ? timeSeriesExplorerUrl +
                  '&relativeMillis=1800000&timeSeriesDefinitions=[{"name":"Devices","splitBy":"iothub-connection-device-id"}]'
                : undefined,
            // Add the alert rule name to the list of top alerts
            topAlertsWithName = topAlerts.map((alert) => ({
                ...alert,
                name: (rules[alert.ruleId] || {}).name || alert.ruleId,
            })),
            // Add the alert rule name to the list of currently active alerts
            currentActiveAlertsWithName = currentActiveAlerts.map((alert) => ({
                ...alert,
                name: (rules[alert.ruleId] || {}).name || alert.ruleId,
                // limit the number shown in the UI to 1000 active
                count: Math.min(alert.count, Config.maxAlertsCount),
            })),
            // Determine if the rules for all of the alerts are actually loaded.
            unloadedRules =
                topAlerts.filter((alert) => !rules[alert.ruleId]).length +
                currentActiveAlerts.filter((alert) => !rules[alert.ruleId])
                    .length;
        if (unloadedRules > 0) {
            // Fetch the rules since at least one alert doesn't know the name for its rule
            this.refreshRules();
        }

        // Convert the list of alerts by device id to alerts by device type
        const alertsPerDeviceType = Object.keys(alertsPerDeviceId).reduce(
            (acc, deviceId) => {
                const deviceType = (devices[deviceId] || {}).type || deviceId;
                return {
                    ...acc,
                    [deviceType]:
                        (acc[deviceType] || 0) + alertsPerDeviceId[deviceId],
                };
            },
            {}
        );

        return (
            <ComponentArray>
                <ContextMenu>
                    <ContextMenuAlign left={true}>
                        <DeviceGroupDropdown
                            deviceGroupIdFromUrl={
                                this.state.selectedDeviceGroupId
                            }
                        />
                        <Protected permission={permissions.updateDeviceGroups}>
                            <ManageDeviceGroupsBtn />
                        </Protected>
                        {this.props.activeDeviceQueryConditions.length !== 0 ? (
                            <>
                                <CreateDeviceQueryBtn />
                                <ResetActiveDeviceQueryBtn />
                            </>
                        ) : null}
                    </ContextMenuAlign>
                    <ContextMenuAlign>
                        <TimeIntervalDropdown
                            onChange={this.props.updateTimeInterval}
                            value={timeInterval}
                            limitExceeded={telemetryQueryExceededLimit}
                            activeDeviceGroup={activeDeviceGroup}
                            t={t}
                        />
                        <RefreshBar
                            refresh={this.refreshDashboard}
                            time={lastRefreshed}
                            isPending={analyticsIsPending || devicesIsPending}
                            t={t}
                        />
                    </ContextMenuAlign>
                </ContextMenu>
                <PageContent className="dashboard-container">
                    <Grid>
                        <Cell className="col-1 devices-overview-cell">
                            <OverviewPanel
                                activeDeviceGroup={activeDeviceGroup}
                                openWarningCount={openWarningCount}
                                openCriticalCount={openCriticalCount}
                                onlineDeviceCount={onlineDeviceCount}
                                offlineDeviceCount={offlineDeviceCount}
                                isPending={
                                    analyticsIsPending || devicesIsPending
                                }
                                error={
                                    deviceGroupError ||
                                    devicesError ||
                                    analyticsError
                                }
                                alerting={alerting}
                                t={t}
                            />
                        </Cell>
                        <Cell className="col-5">
                            <PanelErrorBoundary
                                msg={t("dashboard.panels.map.runtimeError")}
                            >
                                <MapPanel
                                    analyticsVersion={analyticsVersion}
                                    azureMapsKey={azureMapsKey}
                                    devices={devices}
                                    devicesInAlert={devicesInAlert}
                                    mapKeyIsPending={azureMapsKeyIsPending}
                                    isPending={
                                        devicesIsPending || analyticsIsPending
                                    }
                                    error={
                                        azureMapsKeyError ||
                                        devicesError ||
                                        analyticsError
                                    }
                                    t={t}
                                />
                            </PanelErrorBoundary>
                        </Cell>
                        <Cell className="col-3">
                            <AlertsPanel
                                alerts={currentActiveAlertsWithName}
                                isPending={analyticsIsPending || rulesIsPending}
                                error={rulesError || analyticsError}
                                t={t}
                                deviceGroups={deviceGroups}
                            />
                        </Cell>
                        <Cell className="col-6">
                            <TelemetryPanel
                                timeSeriesExplorerUrl={timeSeriesParamUrl}
                                telemetry={telemetry}
                                isPending={telemetryIsPending}
                                limitExceeded={telemetryQueryExceededLimit}
                                lastRefreshed={lastRefreshed}
                                error={deviceGroupError || telemetryError}
                                theme={theme}
                                colors={chartColorObjects}
                                t={t}
                            />
                        </Cell>
                        <Cell className="col-4">
                            <AnalyticsPanel
                                timeSeriesExplorerUrl={timeSeriesParamUrl}
                                topAlerts={topAlertsWithName}
                                alertsPerDeviceId={alertsPerDeviceType}
                                criticalAlertsChange={criticalAlertsChange}
                                isPending={
                                    analyticsIsPending ||
                                    rulesIsPending ||
                                    devicesIsPending
                                }
                                error={
                                    devicesError || rulesError || analyticsError
                                }
                                theme={theme}
                                colors={chartColorObjects}
                                t={t}
                            />
                        </Cell>
                        {Config.showWalkthroughExamples && (
                            <Cell className="col-4">
                                <ExamplePanel t={t} />
                            </Cell>
                        )}
                    </Grid>
                </PageContent>
            </ComponentArray>
        );
    }
}
