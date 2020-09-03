import React, { Component } from "react";

import {
    FormLabel,
    Indicator,
    SectionDesc,
    SectionHeader,
    SummaryBody,
    SummaryCount,
    SummarySection,
    Svg,
} from "components/shared";
import { svgs, joinClasses } from "utilities";
import { IoTHubManagerService } from "services";
import "./ruleSummary.scss";

const editRule = {
    id: "edit-rule-id",
    name: "Editted Rule",
    description: "",
};

export class RuleSummary extends Component {
    constructor(props) {
        super(props);

        this.state = {
            devicesAffected: 0,
        };
    }

    componentDidMount() {
        const { deviceCount, rule } = this.props;
        if (!isNaN(deviceCount)) {
            this.setState({
                devicesAffected: deviceCount,
            });
        } else if (rule) {
            this.getDeviceCount(rule.groupId);
        }
    }

    componentWillReceiveProps(nextProps) {
        if (
            nextProps.deviceCount !== undefined &&
            nextProps.deviceCount !== this.state.devicesAffected
        ) {
            this.setState({
                devicesAffected: nextProps.deviceCount,
            });
        }
    }

    componentWillUnmount() {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }

    getDeviceCount(groupId) {
        this.props.deviceGroups.some((group) => {
            if (group.id === groupId) {
                if (this.subscription) {
                    this.subscription.unsubscribe();
                }
                this.subscription = IoTHubManagerService.getDevices(
                    group.conditions
                ).subscribe(
                    (groupDevices) => {
                        this.setState({
                            devicesAffected: groupDevices.length,
                        });
                    },
                    (error) => this.setState({ error })
                );
                return true;
            }
            return false;
        });
    }

    render() {
        const { t, className, isPending, completedSuccessfully } = this.props,
            rule = this.props.rule || editRule,
            includeSummaryStatus = this.props.includeSummaryStatus || false,
            includeRuleInfo =
                this.props.includeRuleInfo === undefined
                    ? true
                    : this.props.includeRuleInfo,
            { devicesAffected } = this.state;

        return (
            <SummarySection
                key={rule.id}
                className={joinClasses("padded-bottom", className)}
            >
                {includeRuleInfo && <SectionHeader>{rule.name}</SectionHeader>}
                {includeRuleInfo && <FormLabel>{rule.description}</FormLabel>}
                <SummaryBody>
                    <SummaryCount>{devicesAffected}</SummaryCount>
                    <SectionDesc>
                        {t("rules.flyouts.ruleEditor.devicesAffected")}
                    </SectionDesc>
                    {includeSummaryStatus && isPending && <Indicator />}
                    {includeSummaryStatus && completedSuccessfully && (
                        <Svg className="summary-icon" path={svgs.apply} />
                    )}
                </SummaryBody>
            </SummarySection>
        );
    }
}
