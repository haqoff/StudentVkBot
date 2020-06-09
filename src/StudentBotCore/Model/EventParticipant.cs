namespace StudentBotCore.Model
{
    public class EventParticipant
    {
        public ulong EventId { get; set; }
        public ulong PersonId { get; set; }

        public Event Event { get; set; }
        public Person Person { get; set; }
    }
}
