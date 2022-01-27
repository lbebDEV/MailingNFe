using MailingNFe.Config;
using MailingNFe.DAO;
using MailingNFe.Email;
using MailingNFe.Guardian;
using MailingNFe.Model;
using MailingNFe.Service;
using MailingNFe.ServiceLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingNFe.Controllers
{
    class EnvioNFeController : RotinaEspecifica
    {
        public static bool EnvioHabilitado { get; set; } = false;
        public static int HoraRotina;

        ConfigBancoDAO configBancoDAO = new ConfigBancoDAO();
        NfClassificadaDAO nfClassificadaDAO = new NfClassificadaDAO();
        NfNaoIntegradaDAO nfIntegradaDAO = new NfNaoIntegradaDAO();

        public EnvioNFeController()
        {
            Nome = "Mailing Nfe";
            Sigla = "MAINFE";
        }

        public override void CarregarConfig()
        {
            configBancoDAO.BuscarConfigs(true);
            Hora = HoraRotina;
        }

        public override void Executar()
        {
            if (Service_Config.CadastroHabilitado)
            {
                CarregarConfig();
                UltimaExecucao = BuscarUltimaExecucao();
                Descricao = "Hora Definida: " + Hora;
                ValidarExecucao();

                try
                {
                    if (Service_Config.CadastroHabilitado && ExecutarRotina)
                    {
                        if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
                        {
                            Guardian_Log.Log_Rotina(Sigla, Nome, Tipo.Iniciado);

                            List<NfNaoIntegrada> nfNaoIntegradas = nfIntegradaDAO.BuscarNotas();

                            List<NfNaoClassificada> nfNaoClassificadas = nfClassificadaDAO.BuscarNotas();

                            if (nfNaoIntegradas.Count > 0)
                            {
                                PortalEmail portalEmail = new PortalEmail();
                                portalEmail.EnviarDados(nfNaoIntegradas, nfNaoClassificadas);
                            }

                            RegistrarExecucao();
                            Guardian_Log.Log_Rotina(Sigla, Nome, Tipo.Finalizado);
                        }
                       
                    }
                }
                catch (Exception ex)
                {
                    Guardian_LogTxt.LogAplicacao(Service_Config.NomeServico, ex.ToString());
                    Guardian_Log.Log_Ocorrencia(Service_Config.NomeServico, "Erro ao executar o serviço.", ex.ToString(), "");
                }
            }
        }
    }
}
