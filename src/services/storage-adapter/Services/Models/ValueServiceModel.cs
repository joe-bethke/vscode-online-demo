// <copyright file="ValueServiceModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace Mmm.Iot.StorageAdapter.Services.Models
{
    public class ValueServiceModel
    {
        public ValueServiceModel()
        {
        }

        public ValueServiceModel(Document document)
        {
            if (document == null)
            {
                return;
            }

            this.CollectionId = document.GetPropertyValue<string>("CollectionId");
            this.Key = document.GetPropertyValue<string>("Key");
            this.Data = document.GetPropertyValue<string>("Data");
            this.ETag = document.ETag;
            this.Timestamp = document.Timestamp;
        }

        internal ValueServiceModel(KeyValueDocument document)
        {
            if (document == null)
            {
                return;
            }

            this.CollectionId = document.CollectionId;
            this.Key = document.Key;
            this.Data = document.Data;
            this.ETag = document.ETag;
            this.Timestamp = document.Timestamp;
        }

        public string CollectionId { get; set; }

        public string Key { get; set; }

        public string Data { get; set; }

        public string ETag { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}