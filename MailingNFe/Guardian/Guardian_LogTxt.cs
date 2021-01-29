using MailingNFe.Config;
using MailingNFe.Service;
using System;
using System.IO;

namespace MailingNFe.Guardian
{
    enum TipoControle
    {
        Serviço_Iniciado,
        Serviço_Finalizado,
        Ciclo_Iniciado,
        Ciclo_Finalizado
    }

    class Guardian_LogTxt
    {
        /// <summary>
        /// Registrar log de rotina da aplicação
        /// </summary>
        /// <param name="rotina">Nome da rotina</param>
        /// <param name="descricao">Descrição relacionada a rotina</param>
        public static void LogAplicacao(string rotina, string descricao)
        {
            try
            {
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Log\"))
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Log\");

                string textoRegistro = "";
                if (!string.IsNullOrEmpty(rotina))
                    textoRegistro += DateTime.Now.ToString("dd/MM/yyyy | HH:mm:ss.fff") + " | " + rotina;
                if (!string.IsNullOrEmpty(descricao))
                    textoRegistro += Environment.NewLine + "=> " + descricao; ;

                Guardian_TxtFile guardian_Txt = new Guardian_TxtFile();
                guardian_Txt.DefinirTexto(AppDomain.CurrentDomain.BaseDirectory + @"Log\", "Guardian_Log_" + Main.IdCiclo + ".txt", textoRegistro);
            }
            catch (Exception ex)
            {
                string excecaoTxt = ex.Message;
            }
        }

        public static void LogControle(TipoControle tipoControle)
        {
            try
            {
                LogAplicacao(Service_Config.NomeServico, tipoControle.ToString().Replace("_", " "));
            }
            catch (Exception ex)
            {
                string excecaoTxt = ex.Message;
            }
        }
    }
}
