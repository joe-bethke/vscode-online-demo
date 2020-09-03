// <copyright file="EmailParametersConverter.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mmm.Iot.Common.Services.Converters
{
    public class EmailParametersConverter : JsonConverter
    {
        private const string RecipientsKey = "Recipients";

        public override bool CanWrite => false;

        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IDictionary<string, object>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);

            // Convert to a case-insensitive dictionary for case insensitive look up.
            Dictionary<string, object> result =
                new Dictionary<string, object>(jsonObject.ToObject<Dictionary<string, object>>(), StringComparer.OrdinalIgnoreCase);

            if (result.ContainsKey(RecipientsKey) && result[RecipientsKey] != null)
            {
                result[RecipientsKey] = ((JArray)result[RecipientsKey]).ToObject<List<string>>();
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Use default implementation for writing to the field.");
        }
    }
}