using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tochka.Abstraction.Models;

namespace Tochka.Abstraction
{
    public interface IPostService
    {
        Task CreatePostAsync(string text, string username);
        Task<IEnumerable<PostInfo>> GetLastPostInfoAsync(string username, ulong countPost);
    }
}
