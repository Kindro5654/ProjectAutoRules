using System;
using System.Threading.Tasks;

namespace DevProj
{
    class Program
    {
        private const string apiAddress = "https://graph.facebook.com/v5.0/";
        static async Task Main(string[] args)
        {
            Console.WriteLine("Программа для скачивания/загрузки автоправил в Facebook by Даниил Выголов.");
            var re = GetConfiguredRequestExecutor(apiAddress);
            var arc = new Action(re);
            var navigator = new Navigator(re);

            var operation = Operation.None;
            while (operation != Operation.Exit)
            {
                operation = MainMenu.SelectOperation();

                switch (operation)
                {
                    case Operation.DownloadRules:
                        {
                            var bmId = await navigator.SelectBusinessManagerAsync();
                            var accId = await navigator.SelectAdAccountAsync(bmId, true);
                            await arc.DownloadAsync(accId);
                            break;
                        }
                    case Operation.UploadRules:
                        {
                            var bmId = await navigator.SelectBusinessManagerAsync();
                            var accId = await navigator.SelectAdAccountAsync(bmId, true);
                            await arc.UploadAsync(accId);
                            break;
                        }
                    case Operation.ClearRules:
                        {
                            var bmId = await navigator.SelectBusinessManagerAsync();
                            var accId = await navigator.SelectAdAccountAsync(bmId, true);
                            await arc.ClearAsync(accId);
                            break;
                        }
                }
            }
        }

        private static Request GetConfiguredRequestExecutor(string apiAddress)
        {
            Console.Write("Введите access token аккаунта:");
            var ac = Console.ReadLine();
            Console.Write("Введита ip-адрес прокси (Enter,если не нужен):");
            var proxyAddress = Console.ReadLine();
            if (string.IsNullOrEmpty(proxyAddress))
            {
                Console.WriteLine("Не используем прокси!");
                return new Request(apiAddress, ac);
            }
            Console.Write("Введите порт прокси:");
            var proxyPort = Console.ReadLine();
            Console.Write("Введите логин прокси:");
            var proxyLogin = Console.ReadLine();
            Console.Write("Введите пароль прокси:");
            var proxyPassword = Console.ReadLine();
            return new Request(apiAddress, ac, proxyAddress, proxyPort, proxyLogin, proxyPassword);
        }
    }
}