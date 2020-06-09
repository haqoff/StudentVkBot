using System;
using System.Collections.Generic;

namespace StudentBotCore.Model
{
    public class Event
    {
        public ulong Id { get; set; }
        public ulong CategoryId { get; set; }
        public ulong? TagId { get; set; }

        public string Description { get; set; }
        public string Location { get; set; }

        public DateTime? StartUtcDateTime { get; set; }
        public TimeSpan? Duration { get; set; }

        public Category Category { get; set; }
        public Tag Tag { get; set; }

        public List<EventParticipant> Participants { get; set; }
        public List<EventOrganizer> Organizers { get; set; }
    }
}