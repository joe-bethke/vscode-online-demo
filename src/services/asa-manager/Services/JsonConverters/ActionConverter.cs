// <copyright file="ActionConverter.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Mmm.Iot.AsaManager.Services.Models.Rules;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mmm.Iot.AsaManager.Services.JsonConverters
{
    public class ActionConverter : JsonConverter
    {
        private const string TypeKey = "Type";

        public override bool CanWrite => false;

        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IActionModel);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var action = default(EmailActionModel);
            var actionType = Enum.Parse(
                typeof(ActionType),
                jsonObject.GetValue(TypeKey).Value<string>(),
                true);

            switch (actionType)
            {
                case ActionType.Email:
                    action = new EmailActionModel();
                    break;
            }

            serializer.Populate(jsonObject.CreateReader(), action);
            return action;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Use default implementation for writing to the field.");
        }
    }
}