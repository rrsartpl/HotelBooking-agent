// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;

namespace HotelBot.Models.Facebook
{
    /// <summary>
    /// Defines a Facebook recipient.
    /// </summary>
    public class FacebookRecipient
    {
        /// <summary>
        /// The Facebook Id of the recipient.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
