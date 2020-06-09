using System.ComponentModel;

namespace ScheduleTemplateModel
{
    /// <summary>
    /// Перечисление, представляемое собой тэг события.
    /// </summary>
    /// <remarks>
    /// Перечисление связано с таблицей в базе данных.
    /// </remarks>
    public enum TagTemplate
    {
        [Description("Лекция")] Lecture = 1,

        [Description("Практика")] Practice = 2,

        [Description("Лабораторная")] Laboratory = 3,

        [Description("Факультатив")] Optional = 4,

        [Description("Урок")] Lesson = 5,

        [Description("Экзамен")] Exam = 6,

        [Description("Консультация")] Consultation = 7,

        [Description("Другое")] Other = 8
    }
}