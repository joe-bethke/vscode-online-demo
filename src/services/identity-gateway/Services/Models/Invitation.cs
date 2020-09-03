// <copyright file="Invitation.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace Mmm.Iot.IdentityGateway.Services.Models
{
    public class Invitation
    {
        [JsonProperty("email_address")]
        public string EmailAddress { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("email_subject")]
        public string EmailSubject { get; set; }

        [JsonProperty("invite_uri")]
        public string InviteUri { get; set; }

        [JsonProperty("from_email")]
        public string FromEmail { get; set; }

        [JsonProperty("from_message")]
        public string FromMessage { get; set; }

        [JsonProperty("invite_user_meta_data")]
        public string InviteUserMetaData { get; set; }

        [JsonProperty("message_body")]
        public string MessageBody { get; set; }
    }
}