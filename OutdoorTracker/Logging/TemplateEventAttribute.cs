﻿using System;

using Microsoft.Diagnostics.Tracing;

namespace OutdoorTracker.Logging
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TemplateEventAttribute : Attribute
    {
        /// <summary>
        ///   Initializes a new instance of the the TemplateEventAttribute class with the specified event
        ///   identifier.
        /// </summary>
        /// <param name="eventId">The event identifier. This value should be between 0 and 65535.</param>
        public TemplateEventAttribute(int eventId)
        {
            EventId = eventId;
        }

        /// <summary>
        ///   Gets the identifier for the event.
        /// </summary>
        public int EventId { get; }

        /// <summary>Gets or sets the keywords for the event.</summary>
        /// <value>A bitwise combination of the enumeration values.</value>
        public EventKeywords Keywords { get; set; }

        /// <summary>Gets or sets the level for the event.</summary>
        /// <value>One of the enumeration values that specifies the level for the event.</value>
        public EventLevel Level { get; set; }

        /// <summary>Gets or sets the message for the event. The message for the event.</summary>
        /// <value>The message for the event.</value>
        public string Message { get; set; }

        /// <summary>Gets or sets the operation code for the event.</summary>
        /// <value>One of the enumeration values that specifies the operation code.</value>
        public EventOpcode Opcode { get; set; }

        /// <summary>Gets or sets the task for the event.</summary>
        /// <value>The task for the event.</value>
        public EventTask Task { get; set; }

        /// <summary>Gets or sets the version of the event.</summary>
        /// <value>The version of the event.</value>
        public byte Version { get; set; }

        /// <summary>
        ///   Allows fine control over the Activity IDs generated by start and stop events.
        /// </summary>
        public EventActivityOptions ActivityOptions { get; set; }

        /// <summary>
        ///   Event's channel: defines an event log as an additional destination for the event.
        /// </summary>
        public EventChannel Channel { get; set; }

        /// <summary>
        ///   User defined options associated with the event. These do not have meaning to the
        ///   EventSource but are passed through to listeners which given them semantics.
        /// </summary>
        public EventTags Tags { get; set; }
    }
}