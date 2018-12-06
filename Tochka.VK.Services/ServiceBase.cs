using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tochka.VK.Services.Options;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace Tochka.VK.Services
{
    internal abstract class ServiceBase
    {
        protected VkApi api;
        VKOptions _options;

        protected ServiceBase(VkApi api, IOptions<VKOptions> options)
        {
            this.api = api;
            _options = options.Value ?? throw new ArgumentNullException("options", "Не переданы настройки для работы с VK");
        }

        protected async Task<long> GetUserId(string username)
        {
            await AuthorizeAsync();

            var userInfo = await api.Users.GetAsync(new string[] { username });

            if (userInfo.Count == 0)
                throw new ArgumentException("username", "по указаному username не найдено пользователя");
            if(userInfo.Count>1)
                throw new ArgumentException("username", "по указаному username найдено несколько пользователей");

            return userInfo.First().Id;
        }

        public async Task AuthorizeAsync()
        {
            if (api.IsAuthorized)
                return;

            await api.AuthorizeAsync(new ApiAuthParams()
            {
                ApplicationId = _options.ApplicationId,
                Login = _options.Login,
                Password = _options.Password,
                Settings = Settings.Wall
            });
        }
    }
}
