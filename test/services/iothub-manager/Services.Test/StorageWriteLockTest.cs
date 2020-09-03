// <copyright file="StorageWriteLockTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.External.StorageAdapter;
using Mmm.Iot.Common.TestHelpers;
using Mmm.Iot.IoTHubManager.Services.Helpers;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Mmm.Iot.IoTHubManager.Services.Test
{
    public partial class StorageWriteLockTest
    {
        private const string COLL = "coll";
        private const string KEY = "key";
        private readonly Random rand;
        private readonly Mock<IStorageAdapterClient> mockClient;

        public StorageWriteLockTest()
        {
            this.rand = new Random();

            this.mockClient = new Mock<IStorageAdapterClient>();
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task NormalLoopTest()
        {
            var etagOriginal = this.rand.NextString();
            var etagLocked = this.rand.NextString();
            var etagUpdated = this.rand.NextString();

            var model = new ValueModel
            {
                Value = this.rand.NextString(),
            };

            model.Locked = false;
            var dataOriginal = JsonConvert.SerializeObject(model);

            model.Locked = true;
            var dataLocked = JsonConvert.SerializeObject(model);

            model.Value = this.rand.NextString();
            model.Locked = false;
            var dataUpdated = JsonConvert.SerializeObject(model);

            var @lock = new StorageWriteLock<ValueModel>(
                this.mockClient.Object,
                COLL,
                KEY,
                (v, b) => v.Locked = b,
                m => !JsonConvert.DeserializeObject<ValueModel>(m.Data).Locked);

            this.mockClient
                .Setup(x => x.GetAsync(
                    It.Is<string>(s => s == COLL),
                    It.Is<string>(s => s == KEY)))
                .ReturnsAsync(new ValueApiModel
                {
                    Data = dataOriginal,
                    ETag = etagOriginal,
                });

            this.mockClient
                .Setup(x => x.UpdateAsync(
                    It.Is<string>(s => s == COLL),
                    It.Is<string>(s => s == KEY),
                    It.Is<string>(s => s == dataLocked),
                    It.Is<string>(s => s == etagOriginal)))
                .ReturnsAsync(new ValueApiModel
                {
                    ETag = etagLocked,
                });

            var lockResult = await @lock.TryLockAsync();
            Assert.True(lockResult.Value);

            this.mockClient
                .Verify(
                    x => x.GetAsync(
                        It.Is<string>(s => s == COLL),
                        It.Is<string>(s => s == KEY)),
                    Times.Once);

            this.mockClient
                .Verify(
                    x => x.GetAsync(
                        It.Is<string>(s => s == COLL),
                        It.Is<string>(s => s == KEY)),
                    Times.Once);

            this.mockClient
                .Verify(
                    x => x.UpdateAsync(
                        It.Is<string>(s => s == COLL),
                        It.Is<string>(s => s == KEY),
                        It.Is<string>(s => s == dataLocked),
                        It.Is<string>(s => s == etagOriginal)),
                    Times.Once);

            this.mockClient
                .Setup(x => x.UpdateAsync(
                    It.Is<string>(s => s == COLL),
                    It.Is<string>(s => s == KEY),
                    It.Is<string>(s => s == dataUpdated),
                    It.Is<string>(s => s == etagLocked)))
                .ReturnsAsync(new ValueApiModel
                {
                    ETag = etagUpdated,
                });

            var writeResult = await @lock.WriteAndReleaseAsync(model);
            Assert.True(writeResult);

            this.mockClient
                .Verify(
                    x => x.UpdateAsync(
                        It.Is<string>(s => s == COLL),
                        It.Is<string>(s => s == KEY),
                        It.Is<string>(s => s == dataUpdated),
                        It.Is<string>(s => s == etagLocked)),
                    Times.Once);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task LockFailTest()
        {
            var etagOriginal = this.rand.NextString();

            var model = new ValueModel
            {
                Value = this.rand.NextString(),
            };

            model.Locked = true;
            var dataOriginal = JsonConvert.SerializeObject(model);

            var @lock = new StorageWriteLock<ValueModel>(
                this.mockClient.Object,
                COLL,
                KEY,
                (v, b) => v.Locked = b,
                m => !JsonConvert.DeserializeObject<ValueModel>(m.Data).Locked);

            this.mockClient
                .Setup(x => x.GetAsync(
                    It.Is<string>(s => s == COLL),
                    It.Is<string>(s => s == KEY)))
                .ReturnsAsync(new ValueApiModel
                {
                    Data = dataOriginal,
                    ETag = etagOriginal,
                });

            var result = await @lock.TryLockAsync();
            Assert.False(result.Value);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task LockConflictTest()
        {
            var etagOriginal = this.rand.NextString();

            var model = new ValueModel
            {
                Value = this.rand.NextString(),
            };

            model.Locked = false;
            var dataOriginal = JsonConvert.SerializeObject(model);

            model.Locked = true;
            var dataLocked = JsonConvert.SerializeObject(model);

            var @lock = new StorageWriteLock<ValueModel>(
                this.mockClient.Object,
                COLL,
                KEY,
                (v, b) => v.Locked = b,
                m => !JsonConvert.DeserializeObject<ValueModel>(m.Data).Locked);

            this.mockClient
                .Setup(x => x.GetAsync(
                    It.Is<string>(s => s == COLL),
                    It.Is<string>(s => s == KEY)))
                .ReturnsAsync(new ValueApiModel
                {
                    Data = dataOriginal,
                    ETag = etagOriginal,
                });

            this.mockClient
                .Setup(x => x.UpdateAsync(
                    It.Is<string>(s => s == COLL),
                    It.Is<string>(s => s == KEY),
                    It.Is<string>(s => s == dataLocked),
                    It.Is<string>(s => s == etagOriginal)))
                .ThrowsAsync(new ConflictingResourceException());

            var result = await @lock.TryLockAsync();
            Assert.Null(result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task WriteConflictTest()
        {
            var etagOriginal = this.rand.NextString();
            var etagLocked = this.rand.NextString();

            var model = new ValueModel
            {
                Value = this.rand.NextString(),
            };

            model.Locked = false;
            var dataOriginal = JsonConvert.SerializeObject(model);

            model.Locked = true;
            var dataLocked = JsonConvert.SerializeObject(model);

            model.Value = this.rand.NextString();
            model.Locked = false;
            var dataUpdated = JsonConvert.SerializeObject(model);

            var @lock = new StorageWriteLock<ValueModel>(
                this.mockClient.Object,
                COLL,
                KEY,
                (v, b) => v.Locked = b,
                m => !JsonConvert.DeserializeObject<ValueModel>(m.Data).Locked);

            this.mockClient
                .Setup(x => x.GetAsync(
                    It.Is<string>(s => s == COLL),
                    It.Is<string>(s => s == KEY)))
                .ReturnsAsync(new ValueApiModel
                {
                    Data = dataOriginal,
                    ETag = etagOriginal,
                });

            this.mockClient
                .Setup(x => x.UpdateAsync(
                    It.Is<string>(s => s == COLL),
                    It.Is<string>(s => s == KEY),
                    It.Is<string>(s => s == dataLocked),
                    It.Is<string>(s => s == etagOriginal)))
                .ReturnsAsync(new ValueApiModel
                {
                    ETag = etagLocked,
                });

            var lockResult = await @lock.TryLockAsync();
            Assert.True(lockResult.Value);

            this.mockClient
                .Verify(
                    x => x.GetAsync(
                        It.Is<string>(s => s == COLL),
                        It.Is<string>(s => s == KEY)),
                    Times.Once);

            this.mockClient
                .Verify(
                    x => x.GetAsync(
                        It.Is<string>(s => s == COLL),
                        It.Is<string>(s => s == KEY)),
                    Times.Once);

            this.mockClient
                .Verify(
                    x => x.UpdateAsync(
                        It.Is<string>(s => s == COLL),
                        It.Is<string>(s => s == KEY),
                        It.Is<string>(s => s == dataLocked),
                        It.Is<string>(s => s == etagOriginal)),
                    Times.Once);

            this.mockClient
                .Setup(x => x.UpdateAsync(
                    It.Is<string>(s => s == COLL),
                    It.Is<string>(s => s == KEY),
                    It.Is<string>(s => s == dataUpdated),
                    It.Is<string>(s => s == etagLocked)))
                .ThrowsAsync(new ConflictingResourceException());

            var writeResult = await @lock.WriteAndReleaseAsync(model);
            Assert.False(writeResult);

            this.mockClient
                .Verify(
                    x => x.UpdateAsync(
                        It.Is<string>(s => s == COLL),
                        It.Is<string>(s => s == KEY),
                        It.Is<string>(s => s == dataUpdated),
                        It.Is<string>(s => s == etagLocked)),
                    Times.Once);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task ReleaseAsyncWithoutLockTest()
        {
            var @lock = new StorageWriteLock<ValueModel>(
                this.mockClient.Object,
                COLL,
                KEY,
                (v, b) => v.Locked = b,
                m => true);

            await Assert.ThrowsAsync<ResourceOutOfDateException>(() => @lock.ReleaseAsync());
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task ReleaseAndWriteAsyncWithoutLockTest()
        {
            var @lock = new StorageWriteLock<ValueModel>(
                this.mockClient.Object,
                COLL,
                KEY,
                (v, b) => v.Locked = b,
                m => true);

            await Assert.ThrowsAsync<ResourceOutOfDateException>(() => @lock.WriteAndReleaseAsync(null));
        }
    }
}