namespace StudentBotCore.Model
{
    public class EventOrganizer
    {
        public ulong EventId { get; set; }
        public ulong PersonId { get; set; }

        public Event Event { get; set; }
        public Person Person { get; set; }
    }
}
