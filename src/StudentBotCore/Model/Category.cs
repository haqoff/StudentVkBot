using System.Collections.Generic;

namespace StudentBotCore.Model
{
    public class Category
    {
        public ulong Id { get; set; }
        public ulong ChatId { get; set; }
        public string Name { get; set; }


        public VkChat Chat { get; set; }
        public List<Event> Events { get; set; }
    }
}
