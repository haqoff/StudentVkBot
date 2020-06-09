using System;

namespace ScheduleTemplateModel
{
    public class PersonTemplate
    {
        private string _firstName;
        private string _lastName;

        public string FirstName
        {
            get => _firstName;
            set => _firstName = value ?? throw new ArgumentNullException(nameof(FirstName));
        }

        public string LastName
        {
            get => _lastName;
            set => _lastName = value ?? throw new ArgumentNullException(nameof(LastName));
        }

        public string Patronymic { get; set; }
        public string Email { get; set; }
        public ulong? VkUserId { get; set; }

        protected bool Equals(PersonTemplate other)
        {
            return FirstName == other.FirstName
                   && LastName == other.LastName
                   && Patronymic == other.Patronymic
                   && Email == other.Email
                   && VkUserId == other.VkUserId;
        }
    }
}