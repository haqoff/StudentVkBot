using System.Text;

namespace StudentBotCore.Helpers
{
    public static class VkStr
    {
        public const string Tab = "&#12288;";


        public static string Repeat(this string str, int count)
        {
            var sb = new StringBuilder(str.Length * count);
            for (var i = 0; i < count; i++) sb.Append(str);

            return sb.ToString();
        }
    }
}