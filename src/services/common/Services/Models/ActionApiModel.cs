// <copyright file="ActionApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Mmm.Iot.Common.Services.Exceptions;
using Newtonsoft.Json;

namespace Mmm.Iot.Common.Services.Models
{
    public class ActionApiModel
    {
        public ActionApiModel(string type, Dictionary<string, object> parameters)
        {
            this.Type = type;

            try
            {
                this.Parameters = new Dictionary<string, object>(parameters, StringComparer.OrdinalIgnoreCase);
            }
            catch (Exception e)
            {
                var msg = $"Error, duplicate parameters provided for the {this.Type} action. " +
                          "Parameters are case-insensitive.";
                throw new InvalidInputException(msg, e);
            }
        }

        public ActionApiModel(IAction action)
        {
            this.Type = action.Type.ToString();
            this.Parameters = action.Parameters;
        }

        public ActionApiModel()
        {
            this.Type = ActionType.Email.ToString();
            this.Parameters = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        [JsonProperty(PropertyName = "Type")]
        public string Type { get; }

        // Note: Parameters dictionary should always be initialized as case-insensitive.
        [JsonProperty(PropertyName = "Parameters")]
        public IDictionary<string, object> Parameters { get; }

        public IAction ToServiceModel()
        {
            if (!Enum.TryParse(this.Type, true, out ActionType action))
            {
                var validActionsList = string.Join(", ", Enum.GetNames(typeof(ActionType)).ToList());
                throw new InvalidInputException($"The action type '{this.Type}' is not valid." +
                                                $"Valid action types: [{validActionsList}]");
            }

            switch (action)
            {
                case ActionType.Email:
                    return new EmailAction(this.Parameters);
                default:
                    var validActionsList = string.Join(", ", Enum.GetNames(typeof(ActionType)).ToList());
                    throw new InvalidInputException($"The action type '{this.Type}' is not valid" +
                                                    $"Valid action types: [{validActionsList}]");
            }
        }
    }
}