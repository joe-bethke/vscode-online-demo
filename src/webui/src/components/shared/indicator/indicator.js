// Copyright (c) Microsoft. All rights reserved.

import React from "react";
import { joinClasses } from "utilities";

import "./indicator.scss";

const Dot = () => (
        <div className="dot">
            <span className="inner" />
        </div>
    ),
    validSizes = new Set(["large", "medium", "normal", "small", "mini"]),
    validPatterns = new Set(["ring", "bar"]);

/** Creates a loading indicator */
export const Indicator = (props) => {
    const { size, pattern, className } = props,
        sizeClass = validSizes.has(size) ? size : "normal",
        patternClass = validPatterns.has(pattern) ? pattern : "ring";
    return (
        <div
            className={joinClasses(
                "wait-indicator",
                sizeClass,
                patternClass,
                className
            )}
        >
            <Dot />
            <Dot />
            <Dot />
            <Dot />
            <Dot />
            <Dot />
        </div>
    );
};
