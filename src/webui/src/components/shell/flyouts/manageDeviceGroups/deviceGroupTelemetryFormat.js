// Copyright (c) Microsoft. All rights reserved.

import React from "react";
import { Observable } from "rxjs";
import update from "immutability-helper";

import { LinkedComponent } from "utilities";
import { svgs, Validator } from "utilities";
import {
    AjaxError,
    Btn,
    ComponentArray,
    ErrorMsg,
    FormControl,
    PropertyGrid as Grid,
    PropertyGridBody as GridBody,
    PropertyGridHeader as GridHeader,
    PropertyRow as Row,
    PropertyCell as Cell,
} from "components/shared";

import Flyout from "components/shared/flyout";
const Section = Flyout.Section;
update.extend("$autoArray", (val, obj) => update(obj || [], val));

const initialState = {
        isPending: false,
        error: undefined,
        successCount: 0,
        changesApplied: false,
        jobName: undefined,
        jobId: undefined,
        format: [],
        deletedKeys: [],
    },
    newTelemetry = () => ({
        key: "",
        displayName: "",
    });

export class DeviceGroupTelemetryFormat extends LinkedComponent {
    constructor(props) {
        super(props);
        this.state = initialState;

        this.formatLink = this.linkTo("format", props.onTelemetryChange);
    }

    componentDidMount() {
        if (this.props.format) {
            this.populateState(this.props.format);
        }
    }

    componentWillReceiveProps(nextProps) {
        if (
            nextProps.format &&
            (this.state.format || []).length !== nextProps.format.length
        ) {
            this.populateState(nextProps.format);
        }
    }

    componentWillUnmount() {
        if (this.populateStateSubscription) {
            this.populateStateSubscription.unsubscribe();
        }
        if (this.submitJobSubscription) {
            this.submitJobSubscription.unsubscribe();
        }
    }

    populateState(format) {
        if (this.populateStateSubscription) {
            this.populateStateSubscription.unsubscribe();
        }
        this.populateStateSubscription = Observable.of(format).subscribe(
            (format) =>
                this.setState(
                    update(this.state, {
                        format: { $set: format },
                    })
                )
        );
    }

    formIsValid() {
        return [this.formatLink].every((link) => !link.error);
    }

    getSummaryMessage() {
        const { t } = this.props,
            { isPending, changesApplied } = this.state;

        if (isPending) {
            return t("deviceGroupsFlyout.pending");
        } else if (changesApplied) {
            return t("deviceGroupsFlyout.applySuccess");
        }
        return t("deviceGroupsFlyout.affected");
    }

    addTelemetry = () =>
        this.formatLink.set([...this.formatLink.value, newTelemetry()]);

    deleteTelemetry = (index) => (evt) => {
        var formatMinusIndex = this.formatLink.value.filter(
            (_, idx) => index !== idx
        );
        this.setState(
            update(this.state, {
                format: {
                    $set:
                        formatMinusIndex.length > 0
                            ? formatMinusIndex
                            : [newTelemetry()],
                },
                deletedKeys: { $push: [this.formatLink.value[index].key] },
            }),
            function () {
                if (this.props.onTelemetryChange) {
                    this.props.onTelemetryChange(this.formatLink.value);
                }
            }.bind(this)
        );
    };

    render() {
        this.getSummaryMessage();
        const { t, format } = this.props,
            { error } = this.state,
            // Link these values in render because they need to update based on component state
            formatLinks = this.formatLink.getLinkedChildren((formatLink) => {
                const key = formatLink
                        .forkTo("key")
                        .check(
                            Validator.notEmpty,
                            this.props.t(
                                "deviceGroupsFlyout.format.validation.required"
                            )
                        ),
                    displayName = formatLink
                        .forkTo("displayName")
                        .check(
                            Validator.notEmpty,
                            this.props.t(
                                "deviceGroupsFlyout.format.validation.required"
                            )
                        ),
                    edited = !(!key.value && !displayName.value),
                    error = (edited && (key.error || displayName.error)) || "";
                return { key, displayName, edited, error };
            });
        // editedformat = formatLinks.filter(({ edited }) => edited);
        // formatHaveErrors = editedformat.some(({ error }) => !!error);

        return (
            <Section.Container>
                <Section.Header>
                    {t("deviceGroupsFlyout.format.telemetryFormatHeader")}
                </Section.Header>
                <Section.Content>
                    <Grid className="data-grid">
                        <GridHeader>
                            <Row>
                                <Cell className="col-3">
                                    {t("deviceGroupsFlyout.format.keyHeader")}
                                </Cell>
                                <Cell className="col-3">
                                    {t(
                                        "deviceGroupsFlyout.format.displayNameHeader"
                                    )}
                                </Cell>
                                <Cell className="col-1"></Cell>
                            </Row>
                        </GridHeader>
                        <GridBody>
                            {Object.keys(format).length > 0 &&
                                formatLinks.map(
                                    (
                                        { key, displayName, edited, error },
                                        idx
                                    ) => (
                                        <ComponentArray key={idx}>
                                            <Row
                                                className={
                                                    error
                                                        ? "error-data-row"
                                                        : ""
                                                }
                                            >
                                                <Cell className="col-3">
                                                    <FormControl
                                                        className="small"
                                                        type="text"
                                                        link={key}
                                                        errorState={!!error}
                                                    />
                                                </Cell>
                                                <Cell className="col-3">
                                                    <FormControl
                                                        className="small"
                                                        type="text"
                                                        link={displayName}
                                                        errorState={!!error}
                                                    />
                                                </Cell>
                                                <Cell className="col-1">
                                                    <Btn
                                                        className="icon-only-btn"
                                                        svg={svgs.trash}
                                                        onClick={this.deleteTelemetry(
                                                            idx
                                                        )}
                                                    />
                                                </Cell>
                                            </Row>
                                            {error ? (
                                                <Row className="error-msg-row">
                                                    <ErrorMsg>{error}</ErrorMsg>
                                                </Row>
                                            ) : null}
                                        </ComponentArray>
                                    )
                                )}

                            <Row className="action-row">
                                <Btn
                                    svg={svgs.plus}
                                    onClick={this.addTelemetry}
                                >
                                    {t("deviceGroupsFlyout.format.add")}
                                </Btn>
                            </Row>
                        </GridBody>
                    </Grid>

                    {error && (
                        <AjaxError
                            className="device-jobs-error"
                            t={t}
                            error={error}
                        />
                    )}
                </Section.Content>
            </Section.Container>
        );
    }
}
