using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentBotCore.Helpers;
using StudentBotCore.Model;
using StudentBotCore.Repository;
using VkBotHelper.Command;
using VkNet.Abstractions;

namespace StudentBotCore.Commands
{
    public class StartCommand
    {
        private readonly IVkApi _api;
        private readonly IChatRepository _chatRepository;

        public StartCommand(IVkApi api, IChatRepository chatRepository)
        {
            _api = api;
            _chatRepository = chatRepository;
        }

        [Command(".старт #время", true)]
        public async Task Start(CommandArgs args)
        {
            var id = args.SentFromChatULong();

            if (await _chatRepository.Exists(id))
                return;

            var offset = args.ValueContainer.Get<TimeSpan>(0);

            var vkUser = new VkUser()
            {
                Id = args.SentFromUserULong()
            };
            var admin = new VkChatAdmin()
            {
                IsSuperAdmin = true,
                VkUser = vkUser
            };
            var chat = new VkChat()
            {
                Id = id,
                TimeOffset = offset,
                Admins = new List<VkChatAdmin>() {admin}
            };

            await _chatRepository.AddChat(chat);

            _api.Messages.SendTo(args.SourceMessage, "Поздравляем, вы успешно используете бота!");
        }
    }
}