using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingNFe.ServiceLog
{
    class LogOcorrencia
    {
        public string NomeRotina { get; set; }
        public string Data { get; set; }
        public string Hora { get; set; }
        public string Descricao { get; set; }
        public string DescricaoTecnica { get; set; }
        public string InformacaoAdicional { get; set; }
        public string Aplicacao { get; set; }
        public string Cliente { get; set; }
    }
}
