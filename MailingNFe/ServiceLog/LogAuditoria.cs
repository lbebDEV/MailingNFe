using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingNFe.ServiceLog
{
    class LogAuditoria
    {
        public string NomeRotina { get; set; }
        public string Data { get; set; }
        public string Hora { get; set; }
        public string Acao { get; set; }
        public double Valor { get; set; }
        public string Cliente { get; set; }
    }
}
