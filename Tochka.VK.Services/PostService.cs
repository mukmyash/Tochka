using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tochka.Abstraction;
using Tochka.Abstraction.Models;
using Tochka.VK.Services.Options;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace Tochka.VK.Services
{
    internal class PostService : ServiceBase, IPostService
    {
        public PostService(VkApi api, IOptions<VKOptions> options) : base(api, options)
        {
        }

        public async Task CreatePostAsync(string text, string username)
        {
            var userId = await base.GetUserId(username);

            var requestParams = new WallPostParams()
            {
                Message = text,
                OwnerId = userId

            };

            await api.Wall.PostAsync(requestParams);
        }

        public async Task<IEnumerable<PostInfo>> GetLastPostInfoAsync(string username, ulong countPost)
        {
            var userId = await base.GetUserId(username);

            var requestParams = new WallGetParams()
            {
                Count = countPost,
                OwnerId = userId
            };

            var response = await api.Wall.GetAsync(requestParams);
            return response.WallPosts.Select(p => new PostInfo()
            {
                Text = p.Text
            }).ToList();
        }
    }
}