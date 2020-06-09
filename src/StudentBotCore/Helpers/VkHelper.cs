using System;
using VkBotHelper.Command;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace StudentBotCore.Helpers
{
    public static class VkHelper
    {
        /// <summary>
        /// Получает id чата, из которого было отправлено сообщение.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static ulong SentFromChatULong(this CommandArgs args)
        {
            return (ulong) args.SentFromChat();
        }

        public static long SentFromChat(this CommandArgs args)
        {
            // PeerId гарантированно имеет значение, так команда запускается на основе него.
            // ReSharper disable once PossibleInvalidOperationException
            return args.SourceMessage.PeerId.Value;
        }

        /// <summary>
        /// Получает идентификатор пользователя, отправившего сообщение.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static ulong SentFromUserULong(this CommandArgs args)
        {
            return (ulong) args.SentFromUser();
        }

        /// <summary>
        /// Получает идентификатор пользователя, отправившего сообщение.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static long SentFromUser(this CommandArgs args)
        {
            return args.SourceMessage.UserId ?? args.SentFromChat();
        }

        /// <summary>
        /// Посылает указанное сообщение в чат, указанный в исходном сообщении <paramref name="source"/>.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="source">Исходное сообщение.</param>
        /// <param name="msg">Новое сообщение.</param>
        public static void SendTo(this IMessagesCategory category, Message source, string msg)
        {
            var id = source.PeerId;
            // ReSharper disable once PossibleInvalidOperationException
            category.SendTo(id.Value, msg);
        }

        public static void SendTo(this IMessagesCategory category, long id, string msg)
        {
            category.Send(new MessagesSendParams()
            {
                Message = msg,
                PeerId = id,
                RandomId = new Random().Next()
            });
        }

        public static void SendTo(this IMessagesCategory category, ulong id, string msg)
        {
            category.SendTo((long) id, msg);
        }
    }
}