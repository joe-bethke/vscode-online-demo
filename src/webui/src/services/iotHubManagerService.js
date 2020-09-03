// Copyright (c) Microsoft. All rights reserved.

import { Observable } from "rxjs";

import Config from "app.config";
import { stringify } from "query-string";
import { HttpClient } from "utilities/httpClient";
import {
    toDevicesModel,
    toDeviceModel,
    toModuleFieldsModel,
    toJobsModel,
    toJobStatusModel,
    toDevicePropertiesModel,
    toDeploymentModel,
    toDeploymentsModel,
    toDeploymentRequestModel,
    toEdgeAgentsModel,
} from "./models";

const ENDPOINT = Config.serviceUrls.iotHubManager;

/** Contains methods for calling the Device service */
export class IoTHubManagerService {
    /** Returns a list of devices */
    static getDevices(conditions = []) {
        const query = encodeURIComponent(JSON.stringify(conditions));
        return HttpClient.get(`${ENDPOINT}devices?query=${query}`).map(
            toDevicesModel
        );
    }

    /** Returns a list of all modules message schema fields */
    static getModulesFields(query) {
        return HttpClient.post(`${ENDPOINT}modules/query`, `"${query}"`).map(
            toModuleFieldsModel
        );
    }

    /** Returns a list of all jobs */
    static getJobs(params) {
        return HttpClient.get(`${ENDPOINT}jobs?${stringify(params)}`).map(
            toJobsModel
        );
    }

    /** Submits a job */
    static submitJob(body) {
        return HttpClient.post(`${ENDPOINT}jobs`, body).map(toJobStatusModel);
    }

    /** Get returns the status details for a particular job */
    static getJobStatus(jobId) {
        return HttpClient.get(
            `${ENDPOINT}jobs/${jobId}?includeDeviceDetails=true`
        ).map(toJobStatusModel);
    }

    /** Provisions a device */
    static provisionDevice(body) {
        return HttpClient.post(`${ENDPOINT}devices`, body).map(toDeviceModel);
    }

    /** Deletes a device */
    static deleteDevice(id) {
        return HttpClient.delete(`${ENDPOINT}devices/${id}`).map(() => ({
            deletedDeviceId: id,
        }));
    }

    /** Returns the account's device group filters */
    static getDeviceProperties() {
        // return Observable
        //   .forkJoin(
        //     HttpClient.get(`${ENDPOINT}deviceproperties`),
        //     HttpClient.get(`${Config.serviceUrls.deviceSimulation}devicemodelproperties`)
        //   )
        //   .map(([iotResponse, dsResponse]) => toDevicePropertiesModel(iotResponse, dsResponse));

        // Stop Gap until Device Sim is online
        return Observable.forkJoin(
            HttpClient.get(`${ENDPOINT}deviceproperties`) //,
            //HttpClient.get(`${Config.serviceUrls.deviceSimulation}devicemodelproperties`)
        ).map(([iotResponse]) =>
            toDevicePropertiesModel(iotResponse, iotResponse)
        );
    }

    /** Returns deployments */
    static getDeployments() {
        return HttpClient.get(`${ENDPOINT}deployments`).map(toDeploymentsModel);
    }

    /** Returns deployment */
    static getDeployment(id, isLatest) {
        return HttpClient.get(
            `${ENDPOINT}deployments/${id}?includeDeviceStatus=true&isLatest=${isLatest}`
        ).map(toDeploymentModel);
    }

    /** Queries EdgeAgent */
    static getModulesByQuery(query) {
        return HttpClient.post(`${ENDPOINT}modules/query`, query).map(
            toEdgeAgentsModel
        );
    }

    /** Queries Devices */
    static getDevicesByQuery(query) {
        return HttpClient.post(`${ENDPOINT}devices/query`, query).map(
            toDevicesModel
        );
    }

    static getDevicesByQueryForDeployment(id, query, isLatest) {
        return HttpClient.post(
            `${ENDPOINT}deployments/devices/${id}?isLatest=${isLatest}`,
            query
        ).map(toDevicesModel);
    }

    /** Create a deployment */
    static createDeployment(deploymentModel) {
        return HttpClient.post(
            `${ENDPOINT}deployments`,
            toDeploymentRequestModel(deploymentModel)
        ).map(toDeploymentModel);
    }

    /** Delete a deployment */
    static deleteDeployment(id, isDelete = true) {
        return HttpClient.delete(
            `${ENDPOINT}deployments/${id}?isDelete=${isDelete}`
        ).map(() => id);
    }

    static reactivateDeployment(id) {
        return HttpClient.put(
            `${ENDPOINT}deployments/${id}`,
            {},
            { timeout: 120000 }
        );
    }

    /** Returns deployments */
    static getDeploymentDetails(query) {
        return HttpClient.post(`${ENDPOINT}modules`, query).map(
            toDeploymentsModel
        );
    }

    /** Sends Cloud to device message */
    static sendCloudToDeviceMessages(id, message) {
        return HttpClient.post(
            `${ENDPOINT}devices/${id}/c2dmessage`,
            '"' + message + '"'
        );
    }
}
