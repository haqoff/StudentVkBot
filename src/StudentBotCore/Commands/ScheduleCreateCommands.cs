using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ScheduleTemplateModel;
using StudentBotCore.Helpers;
using StudentBotCore.Repository;
using StudentBotCore.Service;
using StudentBotCore.Service.ScheduleGenerator;
using VkBotHelper.Command;
using VkNet.Abstractions;
using VkNet.Enums;
using VkNet.Model.Attachments;

namespace StudentBotCore.Commands
{
    internal class ScheduleCreateCommands
    {
        private readonly IVkApi _api;
        private readonly INetworkService _network;
        private readonly IScheduleGenerateService _generator;
        private readonly IRegularScheduleRepository _scheduleRepository;
        private readonly IEventRepository _eventRepository;

        public ScheduleCreateCommands(IVkApi api, INetworkService network, IScheduleGenerateService generator,
            IRegularScheduleRepository scheduleRepository, IEventRepository eventRepository)
        {
            _api = api;
            _network = network;
            _generator = generator;
            _scheduleRepository = scheduleRepository;
            _eventRepository = eventRepository;
        }

        [Command(".расп установить", true)]
        public async Task SetSchedule(CommandArgs args)
        {
            var msg = args.SourceMessage;
            var chatId = args.SentFromChatULong();

            if (msg.Attachments.Count != 1)
                return;

            Attachment at = msg.Attachments[0];

            if (at.Instance is Document doc &&
                (doc.Type == DocumentTypeEnum.Text || doc.Type == DocumentTypeEnum.Unknown))
            {
                try
                {
                    var str = await _network.LoadStringAsync(doc.Uri);
                    var template = JsonConvert.DeserializeObject<ScheduleTemplate>(str);

                    ScheduleGenerateResult result = _generator.Generate(chatId, template);

                    if (!result.HasErrors)
                    {
                        await _eventRepository.ClearAllEvents(chatId);
                        await _scheduleRepository.ClearSchedule(chatId);

                        await _eventRepository.AddEvents(result.Events);
                        await _scheduleRepository.AddSchedule(result.RegularSchedules);

                        _api.Messages.SendTo(args.SourceMessage, "Расписание было успешно установлено.");
                    }
                    else
                    {
                        _api.Messages.SendTo(args.SourceMessage, "Шаблон имеет ошибки.");
                    }
                }
                catch (Exception e)
                {
                    _api.Messages.SendTo(args.SourceMessage, e.Message);
                    _api.Messages.SendTo(args.SourceMessage, e.StackTrace);
                    if (e.InnerException != null)
                    {
                        _api.Messages.SendTo(args.SourceMessage, e.InnerException.Message);
                    }
                }
            }
        }
    }
}