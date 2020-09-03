// <copyright file="SchemaModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Mmm.Iot.Common.Services.Exceptions;
using Newtonsoft.Json;

namespace Mmm.Iot.Common.Services.External.TimeSeries
{
    public class SchemaModel
    {
        private const string DeviceIdKey = "iothub-connection-device-id";
        private readonly HashSet<string> excludeProperties;

        public SchemaModel()
        {
            // List of properties from Time Series that should be
            // excluded in conversion to message model
            this.excludeProperties = new HashSet<string>
            {
                "$$ContentType",
                "$$CreationTimeUtc",
                "$$MessageSchema",
                "content-encoding",
                "content-type",
                "iothub-connection-auth-generation-id",
                "iothub-connection-auth-method",
                "iothub-connection-device-id",
                "iothub-creation-time-utc",
                "iothub-enqueuedtime",
                "iothub-message-schema",
                "iothub-message-source",
            };
        }

        [JsonProperty("rid")]
        public long RowId { get; set; }

        [JsonProperty("$esn")]
        public string EventSourceName { get; set; }

        [JsonProperty("properties")]
        public List<PropertyModel> Properties { get; set; }

        public Dictionary<string, int> PropertiesByIndex()
        {
            var result = new Dictionary<string, int>();

            for (int i = 0; i < this.Properties.Count; i++)
            {
                var property = this.Properties[i];

                if (!this.excludeProperties.Contains(property.Name))
                {
                    result.Add(property.Name, i);
                }
            }

            return result;
        }

        public int GetDeviceIdIndex()
        {
            for (int i = 0; i < this.Properties.Count; i++)
            {
                if (this.Properties[i].Name.Equals(DeviceIdKey, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            throw new TimeSeriesParseException("No device id found in message schema from Time Series Insights. " +
                                            $"Device id property '{DeviceIdKey}' is missing.");
        }
    }
}