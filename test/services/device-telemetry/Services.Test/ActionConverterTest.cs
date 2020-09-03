// <copyright file="ActionConverterTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Mmm.Iot.Common.Services.Models;
using Mmm.Iot.Common.TestHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Mmm.Iot.DeviceTelemetry.Services.Test
{
    public class ActionConverterTest
    {
        private const string ParameterNotes = "Chiller pressure is at 250 which is high";
        private const string ParameterSubject = "Alert Notification";
        private const string ParameterRecipients = "sampleEmail@gmail.com";
        private const string ParameterNotesKey = "Notes";
        private const string ParameterRecipientsKey = "Recipients";

        public ActionConverterTest()
        {
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void ItReturnsEmailAction_WhenEmailActionJsonPassed()
        {
            // Arrange
            const string SAMPLE_JSON = "[{\"Type\":\"Email\"," +
                                 "\"Parameters\":{\"Notes\":\"" + ParameterNotes +
                                 "\",\"Subject\":\"" + ParameterSubject +
                                 "\",\"Recipients\":[\"" + ParameterRecipients + "\"]}}]";

            // Act
            var rulesList = JsonConvert.DeserializeObject<List<IAction>>(SAMPLE_JSON);

            // Assert
            Assert.NotEmpty(rulesList);
            Assert.Equal(ActionType.Email, rulesList[0].Type);
            Assert.Equal(ParameterNotes, rulesList[0].Parameters[ParameterNotesKey]);
            Assert.Equal(new JArray { ParameterRecipients }, rulesList[0].Parameters[ParameterRecipientsKey]);
        }
    }
}