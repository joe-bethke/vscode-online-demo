// <copyright file="QueryBuilderTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.Helpers;
using Mmm.Iot.Common.TestHelpers;
using Xunit;

namespace Mmm.Iot.Common.Services.Test
{
    public class QueryBuilderTest
    {
        private const string DeviceMsgReceivedKey = "deviceMsgReceived";
        private const string RuleIdKey = "ruleId";
        private const string AlarmKey = "alarm";
        private const string AscKey = "asc";
        private const string DeviceIdKey = "deviceId";
        private const string StatusKey = "status";
        private const string Chiller01 = "chiller-01.0";
        private const string Chiller02 = "chiller-02.0";

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void GetDocumentsSql_WithValidInput()
        {
            // Arrange
            var from = DateTimeOffset.Now.AddHours(-1);
            var to = DateTimeOffset.Now;

            var id = "bef978d4-54f6-429f-bda5-db2494b833ef";

            // Act
            var querySpec = QueryBuilder.GetDocumentsSql(
                AlarmKey,
                id,
                RuleIdKey,
                from,
                DeviceMsgReceivedKey,
                to,
                DeviceMsgReceivedKey,
                AscKey,
                DeviceMsgReceivedKey,
                0,
                100,
                new string[] { Chiller01, Chiller02 },
                DeviceIdKey);

            // Assert
            Assert.Equal($"SELECT TOP @top * FROM c WHERE (c[\"_schema\"] = @schemaName AND c[@devicesProperty] IN (@devicesParameterName0,@devicesParameterName1) AND c[@byIdProperty] = @byId AND c[@fromProperty] >= {from.ToUnixTimeMilliseconds()} AND c[@toProperty] <= {to.ToUnixTimeMilliseconds()}) ORDER BY c[@orderProperty] ASC", querySpec.QueryText);
            Assert.Equal(100, querySpec.Parameters[0].Value);
            Assert.Equal(AlarmKey, querySpec.Parameters[1].Value);
            Assert.Equal(DeviceIdKey, querySpec.Parameters[2].Value);
            Assert.Equal(Chiller01, querySpec.Parameters[3].Value);
            Assert.Equal(Chiller02, querySpec.Parameters[4].Value);
            Assert.Equal(RuleIdKey, querySpec.Parameters[5].Value);
            Assert.Equal(id, querySpec.Parameters[6].Value);
            Assert.Equal(DeviceMsgReceivedKey, querySpec.Parameters[7].Value);
            Assert.Equal(DeviceMsgReceivedKey, querySpec.Parameters[8].Value);
            Assert.Equal(DeviceMsgReceivedKey, querySpec.Parameters[9].Value);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void GetDocumentsSql_WithNullIdProperty()
        {
            // Arrange
            var from = DateTimeOffset.Now.AddHours(-1);
            var to = DateTimeOffset.Now;

            // Act
            var querySpec = QueryBuilder.GetDocumentsSql(
                AlarmKey,
                null,
                null,
                from,
                DeviceMsgReceivedKey,
                to,
                DeviceMsgReceivedKey,
                AscKey,
                DeviceMsgReceivedKey,
                0,
                100,
                new string[] { Chiller01, Chiller02 },
                DeviceIdKey);

            // Assert
            Assert.Equal($"SELECT TOP @top * FROM c WHERE (c[\"_schema\"] = @schemaName AND c[@devicesProperty] IN (@devicesParameterName0,@devicesParameterName1) AND c[@fromProperty] >= {from.ToUnixTimeMilliseconds()} AND c[@toProperty] <= {to.ToUnixTimeMilliseconds()}) ORDER BY c[@orderProperty] ASC", querySpec.QueryText);
            Assert.Equal(100, querySpec.Parameters[0].Value);
            Assert.Equal(AlarmKey, querySpec.Parameters[1].Value);
            Assert.Equal(DeviceIdKey, querySpec.Parameters[2].Value);
            Assert.Equal(Chiller01, querySpec.Parameters[3].Value);
            Assert.Equal(Chiller02, querySpec.Parameters[4].Value);
            Assert.Equal(DeviceMsgReceivedKey, querySpec.Parameters[5].Value);
            Assert.Equal(DeviceMsgReceivedKey, querySpec.Parameters[6].Value);
            Assert.Equal(DeviceMsgReceivedKey, querySpec.Parameters[7].Value);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void FailToGetDocumentsSql_WithInvalidInput()
        {
            // Arrange
            var from = DateTimeOffset.Now.AddHours(-1);
            var to = DateTimeOffset.Now;

            // Assert
            Assert.Throws<InvalidInputException>(() => QueryBuilder.GetDocumentsSql(
                "alarm's",
                "bef978d4-54f6-429f-bda5-db2494b833ef",
                RuleIdKey,
                from,
                DeviceMsgReceivedKey,
                to,
                DeviceMsgReceivedKey,
                AscKey,
                DeviceMsgReceivedKey,
                0,
                100,
                new string[] { Chiller01, Chiller02 },
                DeviceIdKey));
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void GetTopDeviceDocumentsSql_WithValidInput()
        {
            // Act
            var querySpec = QueryBuilder.GetTopDeviceDocumentsSql(
                "message",
                100,
                "chiller-01.0",
                "device.id");

            // Assert
            Assert.Equal($"SELECT TOP @top * FROM c WHERE (c[\"_schema\"] = @schemaName AND c[@devicesProperty] = \"chiller-01.0\") ORDER BY c[\"_timeReceived\"] DESC", querySpec.QueryText);
            Assert.Equal(100, querySpec.Parameters[0].Value);
            Assert.Equal("message", querySpec.Parameters[1].Value);
            Assert.Equal("device.id", querySpec.Parameters[2].Value);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void FailToGetGetTopDeviceDocumentsSql_WithValidInput()
        {
            // Arrange
            var from = DateTimeOffset.Now.AddHours(-1);
            var to = DateTimeOffset.Now;

            // Assert
            Assert.Throws<InvalidInputException>(() => QueryBuilder.GetTopDeviceDocumentsSql(
                "message's",
                100,
                "chiller-02.0",
                "deviceId"));
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void GetCountSql_WithValidInput()
        {
            // Arrange
            var from = DateTimeOffset.Now.AddHours(-1);
            var to = DateTimeOffset.Now;

            var id = "bef978d4-54f6-429f-bda5-db2494b833ef";

            // Act
            var querySpec = QueryBuilder.GetCountSql(
                AlarmKey,
                id,
                RuleIdKey,
                from,
                DeviceMsgReceivedKey,
                to,
                DeviceMsgReceivedKey,
                new string[] { Chiller01, Chiller02 },
                DeviceIdKey,
                new string[] { "open", "acknowledged" },
                StatusKey);

            // Assert
            Assert.Equal($"SELECT VALUE COUNT(1) FROM c WHERE (c[\"_schema\"] = @schemaName AND c[@devicesProperty] IN (@devicesParameterName0,@devicesParameterName1) AND c[@byIdProperty] = @byId AND c[@fromProperty] >= {from.ToUnixTimeMilliseconds()} AND c[@toProperty] <= {to.ToUnixTimeMilliseconds()} AND c[@filterProperty] IN (@filterParameterName0,@filterParameterName1))", querySpec.QueryText);
            Assert.Equal(AlarmKey, querySpec.Parameters[0].Value);
            Assert.Equal(DeviceIdKey, querySpec.Parameters[1].Value);
            Assert.Equal(Chiller01, querySpec.Parameters[2].Value);
            Assert.Equal(Chiller02, querySpec.Parameters[3].Value);
            Assert.Equal(RuleIdKey, querySpec.Parameters[4].Value);
            Assert.Equal(id, querySpec.Parameters[5].Value);
            Assert.Equal(DeviceMsgReceivedKey, querySpec.Parameters[6].Value);
            Assert.Equal(DeviceMsgReceivedKey, querySpec.Parameters[7].Value);
            Assert.Equal(StatusKey, querySpec.Parameters[8].Value);
            Assert.Equal("open", querySpec.Parameters[9].Value);
            Assert.Equal("acknowledged", querySpec.Parameters[10].Value);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void GetCountSql_WithNullIdProperty()
        {
            // Arrange
            var from = DateTimeOffset.Now.AddHours(-1);
            var to = DateTimeOffset.Now;

            // Act
            var querySpec = QueryBuilder.GetCountSql(
                AlarmKey,
                null,
                null,
                from,
                DeviceMsgReceivedKey,
                to,
                DeviceMsgReceivedKey,
                new string[] { Chiller01, Chiller02 },
                DeviceIdKey,
                new string[] { "open", "acknowledged" },
                StatusKey);

            // Assert
            Assert.Equal($"SELECT VALUE COUNT(1) FROM c WHERE (c[\"_schema\"] = @schemaName AND c[@devicesProperty] IN (@devicesParameterName0,@devicesParameterName1) AND c[@fromProperty] >= {from.ToUnixTimeMilliseconds()} AND c[@toProperty] <= {to.ToUnixTimeMilliseconds()} AND c[@filterProperty] IN (@filterParameterName0,@filterParameterName1))", querySpec.QueryText);
            Assert.Equal(AlarmKey, querySpec.Parameters[0].Value);
            Assert.Equal(DeviceIdKey, querySpec.Parameters[1].Value);
            Assert.Equal(Chiller01, querySpec.Parameters[2].Value);
            Assert.Equal(Chiller02, querySpec.Parameters[3].Value);
            Assert.Equal(DeviceMsgReceivedKey, querySpec.Parameters[4].Value);
            Assert.Equal(DeviceMsgReceivedKey, querySpec.Parameters[5].Value);
            Assert.Equal(StatusKey, querySpec.Parameters[6].Value);
            Assert.Equal("open", querySpec.Parameters[7].Value);
            Assert.Equal("acknowledged", querySpec.Parameters[8].Value);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void FailToGetCountSql_WithInvalidInput()
        {
            // Arrange
            var from = DateTimeOffset.Now.AddHours(-1);
            var to = DateTimeOffset.Now;

            // Assert
            Assert.Throws<InvalidInputException>(() => QueryBuilder.GetCountSql(
                AlarmKey,
                "'chiller-01' or 1=1",
                RuleIdKey,
                from,
                DeviceMsgReceivedKey,
                to,
                DeviceMsgReceivedKey,
                new string[] { Chiller01, Chiller02 },
                DeviceIdKey,
                new string[] { "open", "acknowledged" },
                StatusKey));
        }
    }
}