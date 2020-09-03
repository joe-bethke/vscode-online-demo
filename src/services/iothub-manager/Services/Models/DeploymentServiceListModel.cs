// <copyright file="DeploymentServiceListModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Mmm.Iot.IoTHubManager.Services.Models
{
    public class DeploymentServiceListModel
    {
        public DeploymentServiceListModel(List<DeploymentServiceModel> items)
        {
            this.Items = items;
        }

        public List<DeploymentServiceModel> Items { get; set; }
    }
}