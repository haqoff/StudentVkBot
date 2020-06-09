using System.Collections.Generic;

namespace StudentBotCore.Model
{
    public class Tag
    {
        public ulong Id { get; set; }
        public string Name { get; set; }

        public List<Event> Events { get; set; }
    }
}
