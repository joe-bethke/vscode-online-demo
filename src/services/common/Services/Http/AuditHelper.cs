// <copyright file="AuditHelper.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Mmm.Iot.Common.Services.Models;

namespace Mmm.Iot.Common.Services.Helpers
{
    public class AuditHelper
    {
        public const string DateFormat = "yyyy-MM-dd'T'HH:mm:sszzz";

        public static void AddAuditingData(List<Audit> dbEntities, string userId)
        {
            if (dbEntities != null)
            {
                foreach (var entity in dbEntities)
                {
                    AddAuditingData(entity, userId);
                }
            }
        }

        public static void AddAuditingData(Audit entity, string userId)
        {
            if (entity != null)
            {
                AddCreatedAuditingData(entity, userId);
            }
        }

        public static void UpdateAuditingData(List<Audit> dbEntities, string userId)
        {
            if (dbEntities != null)
            {
                foreach (var entity in dbEntities)
                {
                    UpdateAuditingData(entity, userId);
                }
            }
        }

        public static void UpdateAuditingData(Audit entity, string userId)
        {
            if (entity != null)
            {
                AddModifiedAuditingData(entity, userId);
            }
        }

        private static void AddCreatedAuditingData(Audit entity, string userId)
        {
            entity.CreatedDateTime = DateTimeOffset.UtcNow;
            entity.CreatedBy = userId;
        }

        private static void AddModifiedAuditingData(Audit entity, string userId)
        {
            entity.ModifiedDateTime = DateTimeOffset.UtcNow;
            entity.ModifiedBy = userId;
        }
    }
}