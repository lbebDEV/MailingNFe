using MailingNFe.Config;
using MailingNFe.Controllers;
using MailingNFe.Guardian;
using MailingNFe.ServiceLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingNFe.Service
{
    class Main
    {
        public static string IdCiclo { get; set; }

        public void ExecucaoServico()
        {
            IdCiclo = DateTime.Now.ToString("yyyyMMddHHmmss");

            try
            {
                Service_Config.CarregarConfiguracoes();
                Guardian_LogTxt.LogControle(TipoControle.Ciclo_Iniciado);
                Guardian_Log.Log_Rotina("", Service_Config.NomeServico, Tipo.Iniciado);
                if (Service_Config.Status)
                {
                    EnvioNFeController envioNFeController = new EnvioNFeController();
                    envioNFeController.Executar();
                }

                Guardian_LogTxt.LogControle(TipoControle.Ciclo_Finalizado);
                Guardian_Log.Log_Rotina("", Service_Config.NomeServico, Tipo.Finalizado);
            }
            catch (Exception ex)
            {
                Guardian_LogTxt.LogAplicacao(Service_Config.NomeServico, ex.ToString());
                Guardian_Log.Log_Ocorrencia(Service_Config.NomeServico, "Erro ao executar o serviço.", ex.ToString(), "");
            }
        }
    }
}
