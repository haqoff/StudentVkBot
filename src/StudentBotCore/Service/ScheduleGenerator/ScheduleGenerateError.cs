namespace StudentBotCore.Service.ScheduleGenerator
{
    public enum ScheduleGenerateError
    {
        /// <summary>
        /// Для шаблона события не задано расписание.
        /// </summary>
        NoScheduleForEvent,

        /// <summary>
        /// Для шаблона события не задана категория.
        /// </summary>
        NoCategoryForEvent,

        /// <summary>
        /// Для шаблона организатора события не задан человек.
        /// </summary>
        NoPersonForEventOrganizer,

        /// <summary>
        /// Для шаблона участника события не задан человек.
        /// </summary>
        NoPersonForEventParticipant,

        /// <summary>
        /// Превышен лимит категорий.
        /// </summary>
        ExceedsLimitCategories,

        /// <summary>
        /// Превышен лимит человек.
        /// </summary>
        ExceedsLimitPersons,

        /// <summary>
        /// Превышен лимит регулярного расписания.
        /// </summary>
        ExceedsLimitRegularSchedule,

        /// <summary>
        /// Превышен лимит количества событий в день.
        /// </summary>
        ExceedsLimitEventTemplatesPerDay,

        /// <summary>
        /// Превышен лимит количества организаторов в событии.
        /// </summary>
        ExceedsLimitOrganizersPerEvent,

        /// <summary>
        /// Превышен лимит количества участников в событии.
        /// </summary>
        ExceedsLimitParticipantsPerEvent,

        /// <summary>
        /// Превышен лимит периода недели события.
        /// </summary>
        ExceedsLimitWeeksPerEvent
    }
}
