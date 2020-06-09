using System;
using System.Collections.Generic;

namespace StudentBotCore.Model
{
    public class VkChat
    {
        public ulong Id { get; set; }
        public TimeSpan TimeOffset { get; set; }

        public List<VkChatAdmin> Admins { get; set; }
        public List<Category> EventCategories { get; set; }
        public List<EveryDayRegularEventNotification> Notifications { get; set; }
        public List<EveryDayRegularSchedule> ScheduleOrders { get; set; }
    }
}
