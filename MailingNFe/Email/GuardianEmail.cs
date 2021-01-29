using MailingNFe.Guardian;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MailingNFe.Email
{
    class GuardianEmail
    {
        public string Servidor { get; set; }
        public int Porta { get; set; }
        public bool Ssl { get; set; } = false;
        public string Login { get; set; }
        public string Senha { get; set; }

        public string EmailRemetente { get; set; }
        public List<string> EmailsDestinatario { get; set; }
        public string Assunto { get; set; }
        public string Mensagem { get; set; }
        public List<Attachment> Anexos { get; set; }

        public bool Enviar()
        {
            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.Host = Servidor;
                smtp.Port = Porta;
                smtp.EnableSsl = Ssl;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(Login, Senha);

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(EmailRemetente);

                    foreach (string email in EmailsDestinatario)
                    {
                        if (Guardian_Util.ValidarEmail(email))
                            mail.To.Add(new MailAddress(email));
                    }

                    mail.Subject = Assunto;
                    mail.Body = Mensagem;
                    mail.IsBodyHtml = true;

                    if (Anexos != null)
                    {
                        foreach (Attachment anexo in Anexos)
                        {
                            mail.Attachments.Add(anexo);
                        }
                    }

                    smtp.Send(mail);
                }
            }

            return true;
        }

    }
}
