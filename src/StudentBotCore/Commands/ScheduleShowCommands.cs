using System;
using System.Threading.Tasks;
using StudentBotCore.Helpers;
using StudentBotCore.Repository;
using StudentBotCore.Service;
using VkBotHelper.Command;
using VkBotHelper.Parser.Tokens.Values;
using VkNet.Abstractions;

namespace StudentBotCore.Commands
{
    public class ScheduleShowCommands
    {
        private readonly IVkApi _api;
        private readonly IEventRepository _eventRepository;
        private readonly IRegularScheduleRepository _scheduleRepository;
        private readonly IScheduleFormatService _formatService;

        public ScheduleShowCommands(IVkApi api, IEventRepository eventRepository, IRegularScheduleRepository scheduleRepository,
            IScheduleFormatService formatService)
        {
            _api = api;
            _eventRepository = eventRepository;
            _scheduleRepository = scheduleRepository;
            _formatService = formatService;
        }

        private async Task ShowCore(ulong chatId, DateTime? date)
        {
            var events = await _eventRepository.GetEvents(chatId, date);
            var schedule = await _scheduleRepository.GetSchedule(chatId);

            var info = _formatService.GetDayEventsInfo(events, schedule);
            var str = _formatService.FormatScheduleAndEvents(info, date);

            _api.Messages.SendTo(chatId, str);
        }

        [Command(".расп", true)]
        public async Task ShowToday(CommandArgs args)
        {
            var chatId = args.SentFromChatULong();
            var today = DateTime.Now;

            await ShowCore(chatId, today);
        }

        [Command(".расп #дата", true)]
        public async Task ShowByDate(CommandArgs args)
        {
            var chatId = args.SentFromChatULong();
            var date = args.ValueContainer.Get<Date>(0);

            await ShowCore(chatId, date.ToDateTime());
        }
    }
}