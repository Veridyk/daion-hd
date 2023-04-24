using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisDeck.Services
{
    public interface IMailService
    {
        void SendMail(List<string> receivers, string title, string subject, string body);
    }
}
