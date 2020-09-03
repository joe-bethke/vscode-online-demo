// Copyright (c) Microsoft. All rights reserved.

import React from "react";
import Flyout from "components/shared/flyout";

import "./help.scss";

const docLinks = [
    {
        translationId: "header.getStarted",
        url:
            "https://docs.microsoft.com/en-us/azure/iot-suite/iot-suite-remote-monitoring-monitor",
    },
    {
        translationId: "header.documentation",
        url: "https://docs.microsoft.com/en-us/azure/iot-suite",
    },
    {
        translationId: "header.sendSuggestion",
        url:
            "https://feedback.azure.com/forums/916438-azure-iot-solution-accelerators",
    },
];

export const Help = (props) => {
    const { t, onClose } = props;
    return (
        <Flyout.Container
            header={t("helpFlyout.title")}
            t={t}
            onClose={onClose}
        >
            <ul className="help-list">
                {docLinks.map(({ url, translationId }) => (
                    <li key={translationId}>
                        <a target="_blank" rel="noopener noreferrer" href={url}>
                            {t(translationId)}
                        </a>
                    </li>
                ))}
            </ul>
        </Flyout.Container>
    );
};
