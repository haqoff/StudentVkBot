using System.Collections.Generic;

namespace StudentBotCore.Model
{
    public class Person
    {
        public ulong Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public ulong? VkUserId { get; set; }

        public VkUser VkUser { get; set; }
        public List<EventOrganizer> OrganizerInEvents { get; set; }
        public List<EventParticipant> ParticipantInEvents { get; set; }
    }
}