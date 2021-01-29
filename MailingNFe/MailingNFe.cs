using MailingNFe.Config;
using MailingNFe.Guardian;
using MailingNFe.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MailingNFe
{
    public partial class MailingNFe : ServiceBase
    {
        private System.Threading.Timer timerPrincipal;
        public bool UploadDadosHabilitado { get; set; } = false;

        public MailingNFe()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Main.IdCiclo = "Controle_Servico";
            Guardian_LogTxt.LogControle(TipoControle.Serviço_Iniciado);
            Service_Config.CarregarConfiguracoes();
            StartTimer();
        }

        protected override void OnStop()
        {
            Main.IdCiclo = "Controle_Servico";
            Guardian_LogTxt.LogControle(TipoControle.Serviço_Finalizado);
            StopTimer();
        }
        private void StartTimer()
        {
            try
            {
                if (timerPrincipal == null)
                {
                    TimeSpan tsInterval = new TimeSpan(0, Convert.ToInt32(Service_Config.DelayCiclo), 0);
                    timerPrincipal = new System.Threading.Timer(new System.Threading.TimerCallback(Timer_Tick), null, tsInterval, tsInterval);
                }
            }
            catch (Exception ex)
            {
                Guardian_LogTxt.LogAplicacao("StartTimer", "Exceção: " + ex.ToString());
            }
        }
        private void StopTimer()
        {
            try
            {
                if (timerPrincipal != null)
                {
                    timerPrincipal.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                    timerPrincipal.Dispose();
                    timerPrincipal = null;
                }
            }
            catch (Exception ex)
            {
                Guardian_LogTxt.LogAplicacao("StopTimer", "Exceção: " + ex.ToString());
            }
        }

        private void Timer_Tick(object state)
        {
            try
            {
                if (Service_Config.UploadHoraInicio <= DateTime.Now && Service_Config.UploadHoraFim >= DateTime.Now)
                {
                    if (!UploadDadosHabilitado)
                    {
                        UploadDadosHabilitado = true;
                        timerPrincipal.Change(TimeSpan.FromMinutes(Service_Config.DelayUpload), TimeSpan.FromMinutes(Service_Config.DelayUpload));
                        Guardian_LogTxt.LogAplicacao(Service_Config.NomeServico, "Delay de Upload definido para " + Service_Config.DelayUpload + "min");
                    }
                }
                else
                {
                    if (UploadDadosHabilitado)
                    {
                        UploadDadosHabilitado = false;
                        timerPrincipal.Change(TimeSpan.FromMinutes(Service_Config.DelayCiclo), TimeSpan.FromMinutes(Service_Config.DelayCiclo));
                        Guardian_LogTxt.LogAplicacao(Service_Config.NomeServico, "Delay de Ciclo definido para " + Service_Config.DelayCiclo + "min");
                    }
                    else
                    {
                        Guardian_LogTxt.LogAplicacao(Service_Config.NomeServico, "Delay: " + Service_Config.DelayCiclo + " min");
                    }
                }

                Main main = new Main();
                main.ExecucaoServico();
            }
            catch (Exception ex)
            {
                Guardian_LogTxt.LogAplicacao("Timer_Tick", "Exceção: " + ex.ToString());
            }
        }
    }
}
