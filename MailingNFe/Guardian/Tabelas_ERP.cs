using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingNFe.Guardian
{
    class Tabelas_ERP
    {
#if (DEBUG)
            public static String Add { get; set; } = "990";
#else
        public static String Add { get; set; } = "010";
#endif

        /// <summary>
        /// Cadastro de Fornecedor
        /// </summary>        
        public static String SA2 { get; set; } = "SA2" + Add;

        /// <summary>
        /// Documento de Entrada
        /// </summary>
        public static String SF1 { get; set; } = "SF1" + Add;

        /// <summary>
        /// Item do Documento de Entrada
        /// </summary>
        public static String SD1 { get; set; } = "SD1" + Add;
    }
}

