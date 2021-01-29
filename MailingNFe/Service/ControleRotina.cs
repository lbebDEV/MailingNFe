using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingNFe.Service
{
    class ControleRotina
    {
        public bool ControleFlag(DateTime data)
        {
            if (data <= DateTime.Now)
                return true;

            return false;
        }

        public bool ControleHora(int hora)
        {
            if (DateTime.Now.Hour >= hora)
                return true;

            return false;
        }

        public bool ControleDiaMes(int diaMes)
        {
            if (DateTime.Now.Day == diaMes)
                return true;

            return false;
        }

        public bool ControleDiaSemana(int diaSemana)
        {
            if ((int)DateTime.Now.DayOfWeek == diaSemana)
                return true;

            return false;
        }
    }
}
