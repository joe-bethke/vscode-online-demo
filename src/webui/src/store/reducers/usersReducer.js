// Copyright (c) Microsoft. All rights reserved.

import "rxjs";
import { Observable } from "rxjs";
import moment from "moment";
import { schema, normalize } from "normalizr";
import update from "immutability-helper";
import { createSelector } from "reselect";
import { IdentityGatewayService } from "services";
import {
    createReducerScenario,
    createEpicScenario,
    resetPendingAndErrorReducer,
    errorPendingInitialState,
    pendingReducer,
    errorReducer,
    setPending,
    toActionCreator,
    getPending,
    getError,
} from "store/utilities";

// ========================= Epics - START
const handleError = (fromAction) => (error) =>
    Observable.of(
        redux.actions.registerError(fromAction.type, { error, fromAction })
    );

export const epics = createEpicScenario({
    /** Loads the users */
    fetchUsers: {
        type: "USERS_FETCH",
        epic: (fromAction, store) => {
            return IdentityGatewayService.getUsers()
                .map(toActionCreator(redux.actions.updateUsers, fromAction))
                .catch(handleError(fromAction));
        },
    },
    fetchAllNonSystemAdmins: {
        type: "USERS_ALL_FETCH",
        epic: (fromAction, store) => {
            return IdentityGatewayService.getAllNonSystemAdmins()
                .map(
                    toActionCreator(
                        redux.actions.updateAllNonSystemAdmins,
                        fromAction
                    )
                )
                .catch(handleError(fromAction));
        },
    },
    fetchAllSystemAdmins: {
        type: "SUPER_USERS_ALL_FETCH",
        epic: (fromAction, store) => {
            return IdentityGatewayService.getAllSystemAdmins()
                .map(
                    toActionCreator(
                        redux.actions.updateAllSystemAdmins,
                        fromAction
                    )
                )
                .catch(handleError(fromAction));
        },
    },
});
// ========================= Epics - END

// ========================= Schemas - START
const userSchema = new schema.Entity("users"),
    userListSchema = new schema.Array(userSchema),
    // ========================= Schemas - END

    // ========================= Reducers - START
    initialState = {
        ...errorPendingInitialState,
        entities: {},
        items: [],
        lastUpdated: "",
    },
    updateUsersReducer = (state, { payload, fromAction }) => {
        console.log("Update Users Reducers");
        const {
            entities: { users },
            result,
        } = normalize(payload, userListSchema);
        return update(state, {
            entities: { $set: users },
            items: { $set: result },
            lastUpdated: { $set: moment() },
            ...setPending(fromAction.type, false),
        });
    },
    updateAllNonSystemAdminsReducer = (state, { payload, fromAction }) => {
        console.log("Update All Users Reducers");

        return update(state, {
            systemAdmins: { $set: payload },
            ...setPending(fromAction.type, false),
        });
    },
    updateAllSystemAdminsReducer = (state, { payload, fromAction }) => {
        console.log("Update All Users Reducers");

        return update(state, {
            currentSystemAdmins: { $set: payload },
            ...setPending(fromAction.type, false),
        });
    },
    deleteUsersReducer = (state, { payload }) => {
        const spliceArr = payload.reduce((idxAcc, payloadItem) => {
            const idx = state.items.indexOf(payloadItem);
            if (idx !== -1) {
                idxAcc.push([idx, 1]);
            }
            return idxAcc;
        }, []);
        return update(state, {
            entities: { $unset: payload },
            items: { $splice: spliceArr },
        });
    },
    insertUsersReducer = (state, { payload }) => {
        const inserted = payload.map((user) => ({ ...user, isNew: true })),
            {
                entities: { users },
                result,
            } = normalize(inserted, userListSchema);
        if (state.entities) {
            return update(state, {
                entities: { $merge: users },
                items: { $splice: [[0, 0, ...result]] },
            });
        }
        return update(state, {
            entities: { $set: users },
            items: { $set: result },
        });
    },
    insertAllNonSystemAdminsReducer = (state, { payload }) => {
        const inserted = payload.map((systemAdmin) => ({
                ...systemAdmin,
                isNew: true,
            })),
            {
                entities: { systemAdmins },
                result,
            } = normalize(inserted, userListSchema);
        if (state.entities) {
            return update(state, {
                entities: { $merge: systemAdmins },
                items: { $splice: [[0, 0, ...result]] },
            });
        }
        return update(state, {
            entities: { $set: systemAdmins },
            items: { $set: result },
        });
    },
    /* Action types that cause a pending flag */
    fetchableTypes = [
        epics.actionTypes.fetchUsers,
        epics.actionTypes.fetchAllNonSystemAdmins,
        epics.actionTypes.fetchAllSystemAdmins,
    ];

export const redux = createReducerScenario({
    updateUsers: { type: "USERS_UPDATE", reducer: updateUsersReducer },
    registerError: { type: "USERS_REDUCER_ERROR", reducer: errorReducer },
    isFetching: { multiType: fetchableTypes, reducer: pendingReducer },
    deleteUsers: { type: "USER_DELETE", reducer: deleteUsersReducer },
    insertUsers: { type: "USER_INSERT", reducer: insertUsersReducer },
    insertAllNonSystemAdmins: {
        type: "USER_ALL_INSERT",
        reducer: insertAllNonSystemAdminsReducer,
    },
    updateAllNonSystemAdmins: {
        type: "USERS_ALL_UPDATE",
        reducer: updateAllNonSystemAdminsReducer,
    },
    resetPendingAndError: {
        type: "USER_REDUCER_RESET_ERROR_PENDING",
        reducer: resetPendingAndErrorReducer,
    },
    updateAllSystemAdmins: {
        type: "SUPER_USERS_ALL_UPDATE",
        reducer: updateAllSystemAdminsReducer,
    },
});

export const reducer = { users: redux.getReducer(initialState) };
// ========================= Reducers - END

// ========================= Selectors - START
export const getUsersReducer = (state) => state.users;
export const getEntities = (state) => getUsersReducer(state).entities || {};
export const getItems = (state) => getUsersReducer(state).items || [];
export const getUsersLastUpdated = (state) =>
    getUsersReducer(state).lastUpdated;
export const getUsersError = (state) =>
    getError(getUsersReducer(state), epics.actionTypes.fetchUsers);
export const getUsersPendingStatus = (state) =>
    getPending(getUsersReducer(state), epics.actionTypes.fetchUsers);
export const getUsers = createSelector(
    getEntities,
    getItems,
    (entities, items) => items.map((id) => entities[id])
);
export const getUserById = (state, id) => getEntities(state)[id];

export const getAllNonSystemAdmins = (state) =>
    getUsersReducer(state).systemAdmins;
export const getAllNonSystemAdminsError = (state) =>
    getError(getUsersReducer(state), epics.actionTypes.fetchAllNonSystemAdmins);
export const getAllNonSystemAdminsPendingStatus = (state) =>
    getPending(
        getUsersReducer(state),
        epics.actionTypes.fetchAllNonSystemAdmins
    );

export const getAllSystemAdmins = (state) =>
    getUsersReducer(state).currentSystemAdmins;
export const getAllSystemAdminsError = (state) =>
    getError(getUsersReducer(state), epics.actionTypes.fetchAllSystemAdmins);
export const getAllSystemAdminsPendingStatus = (state) =>
    getPending(getUsersReducer(state), epics.actionTypes.fetchAllSystemAdmins);
// ========================= Selectors - END
