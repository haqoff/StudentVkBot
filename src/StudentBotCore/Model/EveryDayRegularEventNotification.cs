using System;

namespace StudentBotCore.Model
{
    public class EveryDayRegularEventNotification
    {
        public ulong ChatId { get; set; }
        public TimeSpan StartUtcTime { get; set; }
        public TimeSpan Scope { get; set; }

        public VkChat Chat { get; set; }
    }
}