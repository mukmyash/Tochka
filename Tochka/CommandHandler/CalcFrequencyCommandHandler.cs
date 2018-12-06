using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tochka.Abstraction;
using Tochka.Algoritms;
using Tochka.Command;

namespace Tochka.CommandHandler
{
    public class CalcFrequencyCommandHandler : MediatR.IRequestHandler<CalcFrequencyCommand, string>
    {
        private const int COUNT_POST = 5;
        IPostService _postService;

        public CalcFrequencyCommandHandler(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<string> Handle(CalcFrequencyCommand request, CancellationToken cancellationToken)
        {
            var posts = await _postService.GetLastPostInfoAsync(request.UserName, COUNT_POST);

            var resultFrequency = new LetterFrequency().Calc(posts.Select(p => p.Text));
            var result = FormatterFactory.GetFormatter().PrepareString(resultFrequency.allLetter, resultFrequency.letterCounter);

            var statisticMessage = $"{request.UserName}, статистика для последних {COUNT_POST} постов: {result}";
            await _postService.CreatePostAsync(statisticMessage, request.UserName);

            return result;
        }


        /// TODO: вынести в отдельные файлы
        private interface IFormatter
        {
            string PrepareString(long allLetter, Dictionary<char, int> letterCounter);
        }

        private static class FormatterFactory
        {
            public static IFormatter GetFormatter()
            {
                return new JSONFormater();
            }
        }

        private class JSONFormater : IFormatter
        {
            public string PrepareString(long allLetter, Dictionary<char, int> letterCounter)
            {
                StringBuilder result = new StringBuilder();
                result.Append("{");
                foreach (var letterCount in letterCounter)
                {
                    result.Append($"\"{letterCount.Key}\":{ Math.Round((decimal)letterCount.Value / allLetter, 2)}");
                    result.Append(",");
                }
                result.Remove(result.Length - 1, 1);
                result.Append("}");

                return result.ToString();
            }
        }
    }
}
