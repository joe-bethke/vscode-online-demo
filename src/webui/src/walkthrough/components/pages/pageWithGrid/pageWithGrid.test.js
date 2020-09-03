// Copyright (c) Microsoft. All rights reserved.

import React from "react";
import { shallow } from "enzyme";
import "polyfills";

import { PageWithGrid } from "./pageWithGrid";

describe("PageWithGrid Component", () => {
    it("Renders without crashing", () => {
        const fakeProps = {
                data: undefined,
                error: undefined,
                isPending: false,
                lastUpdated: undefined,
                fetchData: () => {},
                t: () => {},
            },
            wrapper = shallow(<PageWithGrid {...fakeProps} />);
    });
});
