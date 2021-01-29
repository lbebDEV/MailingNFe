using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingNFe.Guardian
{
    class Tabelas_Guardian
    {
        public static string LogOcorrencia { get; set; } = "LOG_OCORRENCIA";

        public static string LogRotina { get; set; } = "LOG_ROTINA";

        public static string LogEmail { get; set; } = "LOG_EMAIL";

        public static string LogAuditoria { get; set; } = "LOG_AUDITORIA";

        public static string RegistroRotina { get; set; } = "REGISTRO_ROTINA";

        public static string ConfigUpload { get; set; } = "CONFIG_UPLOAD";

        public static string ConfigLog { get; set; } = "CONFIG_LOG";

        public static string Emails { get; set; } = "EMAILS";
    }
}
