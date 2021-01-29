using MailingNFe.Config;
using MailingNFe.Guardian;
using MailingNFe.Service;
using System;

namespace MailingNFe.ServiceLog
{
    public enum Tipo
    {
        Iniciado, Finalizado
    }

    public enum Acao
    {
        Cadastro, Atualização, Deleção, Importação, DeleçãoAntigo, Atendido, Integração
    }

    public enum Status
    {
        Sucesso, Falha
    }

    public class Guardian_Log
    {
        public static void Log_Rotina(string siglaRotina, string nomeRotina, Tipo tipo)
        {
            LogRotina logRotina = new LogRotina
            {
                IdLog = DateTime.Now.ToString("yyyyMMddHHmmss") + siglaRotina,
                IdCiclo = Main.IdCiclo,
                Rotina = nomeRotina,
                Tipo = tipo.ToString(),
                Data = DateTime.Now.ToString("yyyyMMdd"),
                Hora = DateTime.Now.ToString("HH:mm:ss.fff"),
                Aplicacao = Service_Config.NomeServico,
                Cliente = Service_Config.NomeCliente
            };

            LogDAO logDAO = new LogDAO();
            logDAO.RegistrarLogRotina(logRotina, ConexaoGuardian.Conexao());
        }

        public static void Log_Ocorrencia(string nomeRotina, string descricao, string descricaoTecnica, string informacoesAdicionais)
        {
            LogOcorrencia logOcorrencia = new LogOcorrencia
            {
                NomeRotina = nomeRotina,
                Data = DateTime.Now.ToString("yyyyMMdd"),
                Hora = DateTime.Now.ToString("HH:mm:ss.fff"),
                Descricao = descricao.Replace("'", "|"),
                DescricaoTecnica = descricaoTecnica.Replace("'", "|"),
                InformacaoAdicional = informacoesAdicionais.Replace("'", "|")
            };
            informacoesAdicionais.Replace("'", "|");
            logOcorrencia.Aplicacao = Service_Config.NomeServico;
            logOcorrencia.Cliente = Service_Config.NomeCliente;

            LogDAO logDAO = new LogDAO();
            logDAO.RegistrarLogOcorrencia(logOcorrencia, ConexaoGuardian.Conexao());
        }

        public static void Log_Auditoria(string nomeRotina, Acao acao, double valor)
        {
            LogAuditoria logAuditoria = new LogAuditoria
            {
                NomeRotina = nomeRotina,
                Data = DateTime.Now.ToString("yyyyMMdd"),
                Hora = DateTime.Now.ToString("HH:mm:ss.fff"),
                Acao = acao.ToString(),
                Valor = valor,
                Cliente = Service_Config.NomeCliente
            };

            LogDAO logDAO = new LogDAO();
            logDAO.RegistrarLogAuditoria(logAuditoria, ConexaoGuardian.Conexao());
        }

        public static void Log_Email(string email, string rotina, Status status, string infoAdicionais)
        {
            LogEmail logEmail = new LogEmail
            {
                Data = DateTime.Now.ToString("yyyyMMdd"),
                Hora = DateTime.Now.ToString("HH:mm:ss.fff"),
                Email = email,
                Rotina = rotina,
                Status = status.ToString(),
                InfoAdicionais = infoAdicionais,
                Aplicacao = Service_Config.NomeServico
            };

            LogDAO logDAO = new LogDAO();
            logDAO.RegistrarLogEmail(logEmail, ConexaoGuardian.Conexao());
        }
    }
}
