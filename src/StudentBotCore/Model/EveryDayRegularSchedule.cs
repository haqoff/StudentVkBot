using System;

namespace StudentBotCore.Model
{
    public class EveryDayRegularSchedule
    {
        public ulong ChatId { get; set; }

        public TimeSpan StartUtcTime { get; set; }
        public TimeSpan Duration { get; set; }

        public VkChat Chat { get; set; }
    }
}
