// Copyright (c) Microsoft. All rights reserved.

import React from "react";
import { shallow } from "enzyme";
import "polyfills";

import { DevicesContainer } from "./users.container";

describe("Devices Component", () => {
    it("Renders without crashing", () => {
        const fakeProps = {
                users: {},
                entities: {},
                error: undefined,
                isPending: false,
                userGroups: [],
                lastUpdated: undefined,
                fetchDevices: () => {},
                changeDeviceGroup: (id) => {},
                t: () => {},
                updateCurrentWindow: () => {},
                logEvent: () => {},
            },
            wrapper = shallow(<DevicesContainer {...fakeProps} />);
    });
});
