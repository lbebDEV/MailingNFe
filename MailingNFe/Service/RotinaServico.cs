using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingNFe.Service
{
    abstract class RotinaServico
    {
        public bool Habilitado { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }

        public abstract void CarregarConfig();
        public abstract void Executar();
    }
}
