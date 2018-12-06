using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Tochka.Abstraction;
using Tochka.VK.Services.Options;
using VkNet;

namespace Tochka.VK.Services
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterVKService(this IServiceCollection services, Action<VKOptions> options)
        {
            services.Configure(options);
            services.AddScoped<VkApi>();
            services.AddTransient<IPostService, PostService>();
            return services;
        }
    }
}
