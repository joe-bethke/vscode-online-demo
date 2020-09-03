// <copyright file="ExceptionsFilterAttributeTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.Services.Filters;
using Mmm.Iot.Common.TestHelpers;
using Moq;
using Xunit;

namespace Mmm.Iot.Common.Services.Test.Filters
{
    public class ExceptionsFilterAttributeTest
    {
        private readonly ExceptionsFilterAttribute target;
        private readonly Mock<ILogger<ExceptionsFilterAttribute>> logger;

        public ExceptionsFilterAttributeTest()
        {
            this.logger = new Mock<ILogger<ExceptionsFilterAttribute>>();
            this.target = new ExceptionsFilterAttribute(this.logger.Object);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void Doesnt_Fail_When_StackTraces_AreNull()
        {
            // Arrange
            var internalException = new Mock<Exception>();
            var exception = new Exception(string.Empty, internalException.Object);
            internalException.SetupGet(x => x.StackTrace).Returns((string)null);

            var context = new ExceptionContext(
                new ActionContext(
                    new DefaultHttpContext(),
                    new RouteData(),
                    new ActionDescriptor(),
                    new ModelStateDictionary()),
                new List<IFilterMetadata>())
            { Exception = exception };

            // Act
            this.target.OnException(context);

            // Assert
            var result = (ObjectResult)context.Result;
            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode.Value);

            var content = (Dictionary<string, object>)result.Value;
            Assert.True(content.ContainsKey("StackTrace"));
            Assert.True(content.ContainsKey("InnerExceptionStackTrace"));
            Assert.Null(content["StackTrace"]);
            Assert.Null(content["InnerExceptionStackTrace"]);
        }
    }
}