// <copyright file="DiagnosticsEventModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

namespace Mmm.Iot.Diagnostics.Services.Models
{
    public class DiagnosticsEventModel
    {
        public DiagnosticsEventModel()
        {
        }

        public string EventType { get; set; }

        public string SessionId { get; set; }

        public object EventProperties { get; set; }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(this.EventType) && string.IsNullOrEmpty(this.SessionId);
        }
    }
}