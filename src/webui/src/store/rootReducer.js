// Copyright (c) Microsoft. All rights reserved.

import { combineReducers } from "redux";

// Reducers
import { reducer as appReducer } from "./reducers/appReducer";
import { reducer as deploymentsReducer } from "./reducers/deploymentsReducer";
import { reducer as devicesReducer } from "./reducers/devicesReducer";
import { reducer as packagesReducer } from "./reducers/packagesReducer";
import { reducer as rulesReducer } from "./reducers/rulesReducer";
import { reducer as simulationReducer } from "./reducers/deviceSimulationReducer";
import { reducer as usersReducer } from "./reducers/usersReducer";
import { reducer as tenantsReducer } from "./reducers/tenantsReducer";

const rootReducer = combineReducers({
    ...appReducer,
    ...deploymentsReducer,
    ...devicesReducer,
    ...usersReducer,
    ...tenantsReducer,
    ...packagesReducer,
    ...rulesReducer,
    ...simulationReducer,
});

export default rootReducer;
