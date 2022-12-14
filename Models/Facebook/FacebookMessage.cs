// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;

namespace HotelBot.Models.Facebook
{
    /// <summary>
    /// A Facebook message payload.
    /// </summary>
    public class FacebookMessage
    {
        /// <summary>
        /// Gets or sets the message Id from Facebook.
        /// </summary>
        [JsonProperty("mid")]
        public string MessageId { get; set; }

        /// <summary>
        /// Gets or sets the message text.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets whether the message is an echo message.
        /// See <see cref="">Echo Message</see> in the Facebook Developer Documentation.
        /// </summary>
        [JsonProperty("is_echo")]
        public bool IsEcho { get; set; }

        /// <summary>
        /// Gets or sets the quick reply.
        /// </summary>
        [JsonProperty("quick_reply")]
        public FacebookQuickReply QuickReply { get; set; }

        [JsonProperty("quick_replies")]
        public System.Collections.Generic.List<FacebookQuickReply> QuickReplies { get; set; }

        [JsonProperty ("attachments")]
        

        public FacebookAttachment[] Attachments { get; set; }

        [JsonProperty ("attachment")]
        public FacebookAttachment Attachment { get; set; }
    }
}
