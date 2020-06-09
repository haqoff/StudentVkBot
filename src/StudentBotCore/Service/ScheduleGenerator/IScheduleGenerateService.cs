using ScheduleTemplateModel;

namespace StudentBotCore.Service.ScheduleGenerator
{
    /// <summary>
    /// Предоставляет возможности генерации расписания на основе шаблона.
    /// </summary>
    public interface IScheduleGenerateService
    {
        ScheduleGenerateResult Generate(ulong chatId, ScheduleTemplate template);
    }
}