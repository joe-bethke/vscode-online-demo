// <copyright file="StreamAnalyticsHelperTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Management.StreamAnalytics;
using Microsoft.Azure.Management.StreamAnalytics.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Rest.Azure;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.Helpers;
using Mmm.Iot.Common.TestHelpers;
using Mmm.Iot.TenantManager.Services.Helpers;
using Mmm.Iot.TenantManager.Services.Models;
using Moq;
using TestStack.Dossier;
using TestStack.Dossier.EquivalenceClasses;
using Xunit;

namespace Mmm.Iot.TenantManager.Services.Test
{
    public class StreamAnalyticsHelperTest
    {
        private const string MockSubscriptionId = "mocksub";
        private const string MockResourceGroup = "mockrg";
        private const string MockJobName = "mockjob";

        private readonly Mock<AppConfig> mockAppConfig;
        private readonly Mock<GlobalConfig> mockGlobalConfig;
        private readonly Mock<ITokenHelper> mockTokenHelper;
        private readonly Mock<StreamAnalyticsHelper> mockHelper;
        private readonly Mock<IStreamAnalyticsManagementClient> mockClient;
        private readonly Mock<IStreamingJobsOperations> mockStreamingJobs;
        private readonly IStreamAnalyticsHelper helper;

        private Random random = new Random();

        public StreamAnalyticsHelperTest()
        {
            this.random = new Random();

            this.mockGlobalConfig = new Mock<GlobalConfig>();
            this.mockGlobalConfig
                .Setup(x => x.ResourceGroup)
                .Returns(MockResourceGroup);

            this.mockAppConfig = new Mock<AppConfig>();
            this.mockAppConfig
                .Setup(x => x.Global)
                .Returns(this.mockGlobalConfig.Object);

            this.mockTokenHelper = new Mock<ITokenHelper>();

            this.mockStreamingJobs = new Mock<IStreamingJobsOperations>();
            this.mockClient = new Mock<IStreamAnalyticsManagementClient>();
            this.mockClient
                .Setup(x => x.StreamingJobs)
                .Returns(this.mockStreamingJobs.Object);

            this.mockHelper = new Mock<StreamAnalyticsHelper>(this.mockAppConfig.Object, this.mockTokenHelper.Object);
            this.mockHelper
                .Setup(x => x.GetClientAsync())
                .ReturnsAsync(this.mockClient.Object);

            this.helper = this.mockHelper.Object;
        }

        public static IEnumerable<object[]> StreamingJobsWithJobState()
        {
            yield return new object[] { new StreamingJob(jobState: "Running"), true };
            yield return new object[] { new StreamingJob(jobState: "Starting"), true };
            yield return new object[] { new StreamingJob(jobState: "Stopping"), false };
            yield return new object[] { new StreamingJob(jobState: "Stopped"), false };
            yield return new object[] { new StreamingJob(jobState: string.Empty), false };
            yield return new object[] { new StreamingJob(), false };
        }

        [Fact]
        public async Task GetJobAsyncReturnsExpectedJobTest()
        {
            var jobName = this.random.NextString();
            var jobId = this.random.NextString();
            var job = new StreamingJob(id: jobId, name: jobName);
            var jobHeaders = new StreamingJobsGetHeaders();
            var clientResponse = new AzureOperationResponse<StreamingJob, StreamingJobsGetHeaders>
            {
                Body = job,
                Headers = jobHeaders,
            };

            this.mockStreamingJobs
                .Setup(x => x.GetWithHttpMessagesAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, List<string>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientResponse);

            var jobResponse = await this.helper.GetJobAsync(jobName);

            this.mockStreamingJobs
                .Verify(
                    x => x.GetWithHttpMessagesAsync(
                        It.Is<string>(s => s == MockResourceGroup),
                        It.Is<string>(s => s == jobName),
                        It.IsAny<string>(),
                        It.IsAny<Dictionary<string, List<string>>>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);

            Assert.Equal(jobName, jobResponse.Name);
            Assert.Equal(jobId, jobResponse.Id);
        }

        [Fact]
        public async Task GetJobAsyncThrowsResourceNotFoundExceptionBasedOnCloudExceptionMessageTest()
        {
            var jobName = this.random.NextString();

            this.mockStreamingJobs
                .Setup(x => x.GetWithHttpMessagesAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, List<string>>>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CloudException("job was not found"));

            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await this.helper.GetJobAsync(jobName));

            this.mockStreamingJobs
                .Verify(
                    x => x.GetWithHttpMessagesAsync(
                        It.Is<string>(s => s == MockResourceGroup),
                        It.Is<string>(s => s == jobName),
                        It.IsAny<string>(),
                        It.IsAny<Dictionary<string, List<string>>>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
        }

        [Fact]
        public async Task GetJobAsyncThrowsCloudExceptionOnNonSpecificCloudExceptionMessageTest()
        {
            var jobName = this.random.NextString();

            this.mockStreamingJobs
                .Setup(x => x.GetWithHttpMessagesAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, List<string>>>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CloudException());

            await Assert.ThrowsAsync<CloudException>(async () => await this.helper.GetJobAsync(jobName));

            this.mockStreamingJobs
                .Verify(
                    x => x.GetWithHttpMessagesAsync(
                        It.Is<string>(s => s == MockResourceGroup),
                        It.Is<string>(s => s == jobName),
                        It.IsAny<string>(),
                        It.IsAny<Dictionary<string, List<string>>>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
        }

        [Fact]
        public async Task GetJobAsyncThrowsExceptionOnNonCloudExceptionTest()
        {
            var jobName = this.random.NextString();

            this.mockStreamingJobs
                .Setup(x => x.GetWithHttpMessagesAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, List<string>>>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await this.helper.GetJobAsync(jobName));

            this.mockStreamingJobs
                .Verify(
                    x => x.GetWithHttpMessagesAsync(
                        It.Is<string>(s => s == MockResourceGroup),
                        It.Is<string>(s => s == jobName),
                        It.IsAny<string>(),
                        It.IsAny<Dictionary<string, List<string>>>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
        }

        [Theory]
        [MemberData(nameof(StreamingJobsWithJobState))]
        public void JobIsActiveReturnsExpectedValueTest(StreamingJob job, bool expectedValue)
        {
            Assert.Equal(expectedValue, this.helper.JobIsActive(job));
        }
    }
}