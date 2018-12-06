using MediatR;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using System;
using Tochka.Command;
using Tochka.VK.Services;
using Tochka.VK.Services.Options;

namespace Tochka
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineApplication commandLineApplication = new CommandLineApplication();

            CommandOption applicationId = commandLineApplication.Option(
              "-id |--appliaction_id <greeting>",
              "ApplicationId вашего приложения в VK",
              CommandOptionType.SingleValue);

            CommandOption login = commandLineApplication.Option(
              "-l | --login", "Логин пользователя для подключения к VK.",
              CommandOptionType.SingleValue);

            CommandOption password = commandLineApplication.Option(
              "-p | --password", "Пароль пользователя для подключения к VK.",
              CommandOptionType.SingleValue);

            commandLineApplication.HelpOption("-? | -h | --help");

            commandLineApplication.OnExecute(() =>
            {
                if (!applicationId.HasValue())
                {
                    throw new ArgumentException("applicationId", "Не передан ApplicationId вашего приложения в VK");
                }
                if (!login.HasValue())
                {
                    throw new ArgumentException("login", "Не передан Логин пользователя для подключения к VK.");
                }
                if (!password.HasValue())
                {
                    throw new ArgumentException("password", "Не передан Пароль пользователя для подключения к VK.");
                }

                if (!ulong.TryParse(applicationId.Value(), out var appId))
                    throw new FormatException("Неверный формат ApplicationID, допускаются только цифирки.");

                CalcFrequencyCommand(appId, login.Value(), password.Value());
                return 0;
            });

            commandLineApplication.Execute(args);
        }

        private static void CalcFrequencyCommand(ulong appId, string login, string password)
        {
            var provider = GetServices(options =>
            {
                options.ApplicationId = appId;
                options.Login = login;
                options.Password = password;
            });

            var mediator = provider.GetRequiredService<IMediator>();
            while (true)
            {
                Console.Write("Введите идентификатор пользователя: ");
                var userInfo = Console.ReadLine();
                if (string.IsNullOrEmpty(userInfo))
                    return;
                try
                {
                    var resultJSON = mediator.Send(new CalcFrequencyCommand(userInfo)).GetAwaiter().GetResult();
                    Console.WriteLine(resultJSON);
                }catch(Exception e)
                {
                    Console.WriteLine("Что-то пошло не так....");
                    Console.WriteLine(e.Message);
                }
            }
        }
        private static IServiceProvider GetServices(Action<VKOptions> options)
        {
            var services = new ServiceCollection()
                .AddMediatR()
                .RegisterVKService(options);

            return services.BuildServiceProvider();
        }
    }
}
