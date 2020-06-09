using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ScheduleTemplateModel
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum enumValue)
        {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            var attributes =
                (DescriptionAttribute[]) fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

            return attributes.Length > 0
                ? attributes[0].Description
                : enumValue.ToString();
        }

        public static (int id, string description)[] GetEnumValuesIdAndName<TE>() where TE : Enum
        {
            return Enum.GetValues(typeof(TE))
                .Cast<TE>()
                .Select(e => (Convert.ToInt32(e), e.GetDescription())).ToArray();
        }
    }
}