namespace StudentBotCore.Model
{
    public class VkChatAdmin
    {
        public ulong ChatId { get; set; }
        public ulong VkUserId { get; set; }
        public bool IsSuperAdmin { get; set; }

        public VkChat Chat { get; set; }
        public VkUser VkUser { get; set; }
    }
}
