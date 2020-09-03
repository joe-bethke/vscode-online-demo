// Copyright (c) Microsoft. All rights reserved.

import React from "react";

import { Svg } from "components/shared/svg/svg";
import { svgs } from "utilities";

import "../cellRenderer.scss";

export const IsActivePackageRenderer = ({ value, context: { t } }) => (
    <div className="pcs-renderer-cell highlight">
        {value ? (
            <Svg path={svgs.checkmark} className="pcs-renderer-icon" />
        ) : (
            <Svg path={svgs.cancelX} className="pcs-renderer-icon" />
        )}
    </div>
);
