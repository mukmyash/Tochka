using System;
using System.Collections.Generic;
using System.Text;

namespace Tochka.Command
{
    public class CalcFrequencyCommand : MediatR.IRequest<string>
    {
        public CalcFrequencyCommand(string userInfo)
        {
            UserName = userInfo;
        }

        public string UserName { get; }
    }
}
