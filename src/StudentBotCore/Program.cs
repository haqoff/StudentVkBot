using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudentBotCore.Commands;
using StudentBotCore.Model;
using StudentBotCore.Repository;
using StudentBotCore.Service;
using StudentBotCore.Service.ScheduleGenerator;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using VkBotHelper.Helper;

namespace StudentBotCore
{
    internal class Program
    {
        private static void Main()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, false)
                .Build();

            var groupAccessToken = config["accessToken"];
            var botGroupId = ulong.Parse(config["botGroupId"]);
            var dbConnectionString = config["dbConnection"];

            using (var context = new StuDbContext(dbConnectionString))
            {
                context.Database.Migrate();
            }

            VkBot.StartNewCommandBot(groupAccessToken, botGroupId, builder =>
                {
                    builder
                        .Register<StartCommand>()
                        .Register<ScheduleCreateCommands>()
                        .Register<ScheduleShowCommands>();
                },
                container =>
                {
                    container
                        //сервисы
                        .RegisterType<IScheduleGenerateService, ScheduleGenerateService>()
                        .RegisterType<IScheduleFormatService, ScheduleFormatService>()
                        .RegisterType<INetworkService, NetworkService>()

                        //репозитории
                        .RegisterType<IChatRepository, ChatRepository>(new TransientLifetimeManager())
                        .RegisterType<IEventRepository, EventRepository>(new TransientLifetimeManager())
                        .RegisterType<IRegularScheduleRepository, RegularScheduleRepository>(new TransientLifetimeManager())

                        //бд
                        .RegisterFactory<Func<StuDbContext>>(
                            (_) => new Func<StuDbContext>(() => new StuDbContext(dbConnectionString)),
                            new TransientLifetimeManager());
                });
        }
    }
}