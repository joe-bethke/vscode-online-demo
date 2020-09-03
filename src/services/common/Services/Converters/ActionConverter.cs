// <copyright file="ActionConverter.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mmm.Iot.Common.Services.Converters
{
    public class ActionConverter : JsonConverter
    {
        private const string ActionTypeKey = "Type";
        private const string ParametersKey = "Parameters";

        public override bool CanWrite => false;

        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IAction);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);

            var actionType = Enum.Parse(
                typeof(ActionType),
                jsonObject.GetValue(ActionTypeKey).Value<string>(),
                true);

            var parameters = jsonObject.GetValue(ParametersKey).ToString();

            switch (actionType)
            {
                case ActionType.Email:
                    Dictionary<string, object> emailParameters =
                        JsonConvert.DeserializeObject<Dictionary<string, object>>(
                            parameters,
                            new EmailParametersConverter());
                    return new EmailAction(emailParameters);
            }

            // If could not deserialize, throw exception
            throw new InvalidInputException($"Could not deseriailize action with type {actionType}");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Use default implementation for writing to the field.");
        }
    }
}