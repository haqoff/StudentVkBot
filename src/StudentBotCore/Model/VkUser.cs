using System.Collections.Generic;

namespace StudentBotCore.Model
{
    public class VkUser
    {
        public ulong Id { get; set; }

        public Person Person { get; set; }
        public List<VkChatAdmin> AdminInChats { get; set; }
    }
}
