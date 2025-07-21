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
                Guardian_LogTxt.LogAplicacao(Service_Config.NomeServico, $"ValidarExecucao(): ExecutarRotina={ExecutarRotina}");

                try
                {
                    if (Service_Config.CadastroHabilitado && ExecutarRotina)
                    {
                        Guardian_Log.Log_Rotina(Sigla, Nome, Tipo.Iniciado);

                        Guardian_LogTxt.LogAplicacao(Service_Config.NomeServico, "Buscando notas não integradas...");
                        List<NfNaoIntegrada> nfNaoIntegradas = nfIntegradaDAO.BuscarNotas();
                        Guardian_LogTxt.LogAplicacao(Service_Config.NomeServico, $"Notas não integradas encontradas: {nfNaoIntegradas.Count}");

                        Guardian_LogTxt.LogAplicacao(Service_Config.NomeServico, "Buscando notas não classificadas...");
                        List<NfNaoClassificada> nfNaoClassificadas = nfClassificadaDAO.BuscarNotas();
                        Guardian_LogTxt.LogAplicacao(Service_Config.NomeServico, $"Notas não classificadas encontradas: {nfNaoClassificadas.Count}");

                        if (nfNaoIntegradas.Count > 0)
                        {
                            Guardian_LogTxt.LogAplicacao(Service_Config.NomeServico, "Enviando dados para o e-mail...");
                            PortalEmail portalEmail = new PortalEmail();
                            portalEmail.EnviarDados(nfNaoIntegradas, nfNaoClassificadas);
                            Guardian_LogTxt.LogAplicacao(Service_Config.NomeServico, "Dados enviados com sucesso.");
                        }
                        else
                        {
                            Guardian_LogTxt.LogAplicacao(Service_Config.NomeServico, "Nenhuma nota para enviar.");
                        }

                        RegistrarExecucao();
                        Guardian_Log.Log_Rotina(Sigla, Nome, Tipo.Finalizado);
                        Guardian_LogTxt.LogAplicacao(Service_Config.NomeServico, $"Hoje é {DateTime.Now.DayOfWeek}, rotina {(DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Thursday ? "será" : "não será")} executada.");
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
