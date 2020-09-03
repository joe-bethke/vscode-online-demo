// <copyright file="EmailParametersDictionaryConverter.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mmm.Iot.AsaManager.Services.JsonConverters
{
    public class EmailParametersDictionaryConverter : JsonConverter
    {
        private const string RecipientsKey = "Recipients";

        public override bool CanWrite => false;

        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Dictionary<string, object>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);

            // Convert to a case-insensitive dictionary for case insensitive look up.
            Dictionary<string, object> returnDictionary =
                new Dictionary<string, object>(jsonObject.ToObject<Dictionary<string, object>>(), StringComparer.OrdinalIgnoreCase);

            if (returnDictionary.ContainsKey(RecipientsKey) && returnDictionary[RecipientsKey] != null)
            {
                returnDictionary[RecipientsKey] = ((JArray)returnDictionary[RecipientsKey]).ToObject<List<string>>();
            }

            return returnDictionary;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Use default implementation for writing to the field.");
        }
    }
}