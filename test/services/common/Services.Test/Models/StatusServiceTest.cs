// <copyright file="StatusServiceTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.External;
using Mmm.Iot.Common.Services.Models;
using Moq;

namespace Mmm.Iot.Common.Services.Test.Models
{
    public class StatusServiceTest : StatusServiceBase
    {
        public StatusServiceTest(
            AppConfig config)
            : base(config)
        {
            var mockClient = new Mock<ExternalServiceClient>();
            mockClient
                .Setup(t => t.StatusAsync())
                .Returns(Task.FromResult(new StatusResultServiceModel(true, "Ein Apfel a day keeps the doctor away")));
            var mockClientFail = new Mock<ExternalServiceClient>();
            mockClientFail
                .Setup(t => t.StatusAsync())
                .Returns(Task.FromResult(new StatusResultServiceModel(false, "Oops")));
            this.Dependencies = new Dictionary<string, IStatusOperation>
            {
                { "Test Service 1", mockClient.Object },
                { "Test Service 2", mockClient.Object },
                { "Test Service 3", mockClientFail.Object },
            };
        }

        public override IDictionary<string, IStatusOperation> Dependencies { get; set; }
    }
}