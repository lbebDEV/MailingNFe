using MailingNFe.Config;
using MailingNFe.DAO;
using MailingNFe.Guardian;
using MailingNFe.Model;
using MailingNFe.ServiceLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace MailingNFe.Email
{
    class PortalEmail
    {
        public void EnviarDados(List<NfNaoIntegrada> naoIntegrada, List<NfNaoClassificada> naoClassificada)
        {
            ConfigBancoDAO bancoDAO = new ConfigBancoDAO();
            List<string> emailsDestino = new List<string>();
            emailsDestino = bancoDAO.BuscarEmails();

            emailsDestino.Add("GuardianGroup@lbeb.com.br");
            //emailsDestino.Add("guardian.developer004@lbeb.com.br");

            try
            {
                GuardianEmail email = Email_Config.CarregarConfiguracoes();

                using (StreamReader html = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/Template/LayoutComercial.html"))
                {
                    email.Mensagem = html.ReadToEnd();
                }

                string notasNaoIntegradas = "";
                string notasNaoClassificadas = "";
                int count = 0;
                int countClass = 0;
                double countEmiss = 0;
                string cor = "";
                foreach (var item in naoIntegrada)
                {
                    DateTime emiss = DateTime.ParseExact(item.Emissao, "yyyyMMdd", CultureInfo.CreateSpecificCulture("pt-BR"));
                    countEmiss = emiss.Subtract(DateTime.Now).TotalDays;
                    countEmiss = Math.Abs(countEmiss);

                    if (countEmiss <= 30)
                        cor = "#eefdec";
                    else if (countEmiss >= 31 && countEmiss <= 60)
                        cor = "#ffffdf";
                    else
                        cor = "#ffdedb";

                    notasNaoIntegradas +=
                 "<tr style='vertical-align: top;background-color: " + cor + "' align='center'> " +
                     "<td colspan='1' align='center' style='font-weight:normal; font-size: 12px; border: thin solid black; border-right: 0px;'>" +
                        Guardian_Util.FormatarData(item.Emissao) +
                    "</td>" +
                     "<td colspan='1' align='center' style='font-weight:normal; font-size: 12px; border: thin solid black; border-right: 0px;'>" +
                         item.Serie + " - " + item.Numero +
                     "</td>" +
                     "<td colspan='1' align='center' style='font-weight:normal; font-size: 12px; border: thin solid black; border-right: 0px; '>" +
                         item.Cnpj +
                     "</td>" +
                     "<td colspan='1' align='center' style='font-weight:normal; font-size: 12px; border: thin solid black; border-right: 0px;'>" +
                         item.Nome +
                     "</td>" +
                     "<td colspan='1' align='center' style='font-weight:normal; font-size: 12px; border: thin solid black; border-right: 0px;'>" +
                         item.Natureza +
                     "</td>" +
                     "<td colspan='1' align='center' style='font-weight:normal; font-size: 12px; border: thin solid black;'>" +
                         item.Valor +
                     "</td>" +
                 "</tr>";
                    count++;
                }

                foreach (var item in naoClassificada)
                {
                    DateTime emiss = DateTime.ParseExact(item.Emissao, "yyyyMMdd", CultureInfo.CreateSpecificCulture("pt-BR"));
                    countEmiss = emiss.Subtract(DateTime.Now).TotalDays;
                    countEmiss = Math.Abs(countEmiss);

                    if (countEmiss <= 30)
                        cor = "#eefdec";
                    else if (countEmiss >= 31 && countEmiss <= 60)
                        cor = "#ffffdf";
                    else
                        cor = "#ffdedb";

                    notasNaoClassificadas +=
               "<tr style='vertical-align: top; background-color: " + cor + ";' align='center'> " +
                   "<td colspan='1' align='center' style='font-weight:normal; font-size: 12px; border: thin solid black; border-right: 0px;'>" +
                      Guardian_Util.FormatarData(item.Emissao) +
                  "</td>" +
                   "<td colspan='1' align='center' style='font-weight:normal; font-size: 12px; border: thin solid black; border-right: 0px;'>" +
                       item.Serie + " - " + item.Numero +
                   "</td>" +
                   "<td colspan='1' align='center' style='font-weight:normal; font-size: 12px; border: thin solid black; border-right: 0px; '>" +
                       item.Cnpj +
                   "</td>" +
                   "<td colspan='1' align='center' style='font-weight:normal; font-size: 12px; border: thin solid black; border-right: 0px;'>" +
                       item.Nome +
                   "</td>" +
                   "<td colspan='1' align='center' style='font-weight:normal; font-size: 12px; border: thin solid black; border-right: 0px;'>" +
                       item.Quantidade +
                   "</td>" +
                   "<td colspan='1' align='center' style='font-weight:normal; font-size: 12px; border: thin solid black;'>" +
                       item.Valor +
                   "</td>" +
               "</tr>";
                    countClass++;
                }

                email.Mensagem = email.Mensagem.Replace("#DataEnvio", DateTime.Now.ToString("dd/MM/yyyy"));

                email.Mensagem = email.Mensagem.Replace("#TabelaNotas", notasNaoIntegradas);
                email.Mensagem = email.Mensagem.Replace("#notas", count.ToString());

                email.Mensagem = email.Mensagem.Replace("#TabelaNaoClassificadas", notasNaoClassificadas);
                email.Mensagem = email.Mensagem.Replace("#notaClassificadas", countClass.ToString());

                email.Assunto = "Mailing Recebimento Fiscal | " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                email.Mensagem = email.Mensagem;
                email.EmailsDestinatario = emailsDestino;

                if (!String.IsNullOrEmpty(Service_Config.EmailValidacao))
                    email.EmailsDestinatario = new List<string> { Service_Config.EmailValidacao };

                string emailsLog = "";
                foreach (string item in email.EmailsDestinatario)
                {
                    emailsLog += item + "; ";
                }

                if (email.Enviar())
                    Guardian_Log.Log_Email(emailsLog, "Envio de Mailing do Dia", Status.Sucesso, "Mailing Recebimento Fiscal");
                else
                    Guardian_Log.Log_Email(emailsLog, "Envio de Mailing do Dia", Status.Falha, "Mailing Recebimento Fiscal");
            }
            catch (Exception ex)
            {
                Guardian_Log.Log_Ocorrencia("Enviar Mailing Comercial", "Erro ao enviar email", ex.ToString(), "Email: " + emailsDestino[0]);
            }
        }
    }
}
