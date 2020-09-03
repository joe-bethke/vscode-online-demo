// <copyright file="ActionTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.Models;
using Mmm.Iot.Common.TestHelpers;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Mmm.Iot.DeviceTelemetry.Services.Test
{
    public class ActionTest
    {
        private const string ParameterNotes = "Chiller pressure is at 250 which is high";
        private const string ParameterSubject = "Alert Notification";
        private const string ParameterRecipients = "sampleEmail@gmail.com";
        private const string ParameterSubjectKey = "Subject";
        private const string ParameterNotesKey = "Notes";
        private const string ParameterRecipientsKey = "Recipients";
        private readonly JArray emailArray;

        public ActionTest()
        {
            this.emailArray = new JArray { ParameterRecipients };
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void Should_ReturnActionModel_When_ValidActionType()
        {
            // Arrange
            var parameters = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                { ParameterSubjectKey, ParameterSubject },
                { ParameterNotesKey, ParameterNotes },
                { ParameterRecipientsKey, this.emailArray },
            };

            // Act
            var result = new EmailAction(parameters);

            // Assert
            Assert.Equal(ActionType.Email, result.Type);
            Assert.Equal(ParameterNotes, result.Parameters[ParameterNotesKey]);
            Assert.Equal(this.emailArray, result.Parameters[ParameterRecipientsKey]);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void Should_ThrowInvalidInputException_When_ActionTypeIsEmailAndInvalidEmail()
        {
            // Arrange
            var parameters = new Dictionary<string, object>()
            {
                { ParameterSubjectKey, ParameterSubject },
                { ParameterNotesKey, ParameterNotes },
                { ParameterRecipientsKey, new JArray() { "sampleEmailgmail.com" } },
            };

            // Act and Assert
            Assert.Throws<InvalidInputException>(() => new EmailAction(parameters));
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void Should_Throw_InvalidInputException_WhenActionTypeIsEmailAndNoRecipients()
        {
            // Arrange
            var parameters = new Dictionary<string, object>()
            {
                { ParameterSubjectKey, ParameterSubject },
                { ParameterNotesKey, ParameterNotes },
            };

            // Act and Assert
            Assert.Throws<InvalidInputException>(() => new EmailAction(parameters));
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void Should_ThrowInvalidInputException_When_ActionTypeIsEmailAndEmailIsString()
        {
            // Arrange
            var parameters = new Dictionary<string, object>()
            {
                { ParameterSubjectKey, ParameterSubject },
                { ParameterNotesKey, ParameterNotes },
                { ParameterRecipientsKey, ParameterRecipients },
            };

            // Act and Assert
            Assert.Throws<InvalidInputException>(() => new EmailAction(parameters));
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void Should_ReturnActionModel_When_ValidActionTypeParametersIsCaseInsensitive()
        {
            // Arrange
            var parameters = new Dictionary<string, object>()
            {
                { "subject", ParameterSubject },
                { "nOtEs", ParameterNotes },
                { "rEcipiEnts", this.emailArray },
            };

            // Act
            var result = new EmailAction(parameters);

            // Assert
            Assert.Equal(ActionType.Email, result.Type);
            Assert.Equal(ParameterNotes, result.Parameters[ParameterNotesKey]);
            Assert.Equal(this.emailArray, result.Parameters[ParameterRecipientsKey]);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void Should_CreateAction_When_OptionalNotesAreMissing()
        {
            // Arrange
            var parameters = new Dictionary<string, object>()
            {
                { ParameterSubjectKey, ParameterSubject },
                { ParameterRecipientsKey, this.emailArray },
            };

            // Act
            var result = new EmailAction(parameters);

            // Assert
            Assert.Equal(ActionType.Email, result.Type);
            Assert.Equal(string.Empty, result.Parameters[ParameterNotesKey]);
            Assert.Equal(this.emailArray, result.Parameters[ParameterRecipientsKey]);
        }
    }
}