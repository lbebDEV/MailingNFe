using MailingNFe.Config;
using MailingNFe.Guardian;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingNFe.ServiceLog
{
    class LogDAO
    {
        public void RegistrarLogRotina(LogRotina logRotina, string conexao)
        {
            if (!Log_Config.LogRotina)
                return;

            string query =
                "INSERT INTO " + Tabelas_Guardian.LogRotina + " " +
                "(ID_LOG, ID_CICLO, ROTINA, TIPO, DATA, HORA, APLICACAO, CLIENTE) " +
                "VALUES (" +
                "'" + logRotina.IdLog + "', " + // ID LOG
                "'" + logRotina.IdCiclo + "', " + // ID CICLO
                "'" + logRotina.Rotina + "', " +
                "'" + logRotina.Tipo + "', " +
                "'" + logRotina.Data + "', " +
                "'" + logRotina.Hora + "', " +
                "'" + logRotina.Aplicacao + "', " +
                "'" + logRotina.Cliente + "' " +
                ")";

            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Guardian_LogTxt.LogAplicacao("Registrar Log Rotina ", "Erro " + ex.ToString() + Environment.NewLine + " Status: " + Log_Config.LogRotina + " Query: " + query);
            }
        }

        public void RegistrarLogOcorrencia(LogOcorrencia logOcorrencia, string conexao)
        {
            if (!Log_Config.LogOcorrencia)
                return;

            string query =
              "INSERT INTO " + Tabelas_Guardian.LogOcorrencia + " " +
              "(NOME_ROTINA, DATA, HORA, DESCRICAO, DESCRICAO_TECNICA, INFORMACAO_ADICIONAL, APLICACAO, CLIENTE) " +
              "VALUES (" +
              "'" + logOcorrencia.NomeRotina + "', '" + logOcorrencia.Data + "',  " +
              "'" + logOcorrencia.Hora + "', '" + logOcorrencia.Descricao.Replace("'", "|") + "', '" + logOcorrencia.DescricaoTecnica.Replace("'", "|") + "', " +
              "'" + logOcorrencia.InformacaoAdicional.Replace("'", "|") + "', '" + logOcorrencia.Aplicacao + "', '" + logOcorrencia.Cliente + "')";

            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Guardian_LogTxt.LogAplicacao("Registrar Log Ocorrencia ", "Erro " + ex.ToString() + Environment.NewLine + " Status: " + Log_Config.LogOcorrencia + " Query: " + query);
            }
        }

        public void RegistrarLogAuditoria(LogAuditoria logAuditoria, string conexao)
        {
            if (!Log_Config.LogAuditoria)
                return;

            string query =
             "INSERT INTO " + Tabelas_Guardian.LogAuditoria + " " +
             "(NOME_ROTINA, DATA, HORA, ACAO, VALOR, CLIENTE) " +
             "VALUES ( " +
             "'" + logAuditoria.NomeRotina + "', " +
             "'" + logAuditoria.Data + "'," +
             "'" + logAuditoria.Hora + "', " +
             "'" + logAuditoria.Acao + "', " +
             "'" + Convert.ToDouble(logAuditoria.Valor) + "', " +
             "'" + logAuditoria.Cliente + "' )";

            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Guardian_LogTxt.LogAplicacao("Registrar Log Auditoria ", "Erro " + ex.ToString() + Environment.NewLine + " Status: " + Log_Config.LogAuditoria + " Query: " + query);
            }
        }

        public void RegistrarLogEmail(LogEmail logEmail, string conexao)
        {
            if (!Log_Config.LogEmail)
                return;

            string query =
            "INSERT INTO " + Tabelas_Guardian.LogEmail + " " +
            "(DATA, HORA, EMAIL, ROTINA, STATUS, INFO_ADICIONAIS, APLICACAO) " +
            "VALUES (" +
            " '" + logEmail.Data + "', " +
            " '" + logEmail.Hora + "', " +
            " '" + logEmail.Email + "', " +
            " '" + logEmail.Rotina + "', " +
            " '" + logEmail.Status + "', " +
            " '" + logEmail.InfoAdicionais + "', " +
            " '" + logEmail.Aplicacao + "' " +
            ")";

            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Guardian_LogTxt.LogAplicacao("Registrar Log Email ", "Erro " + ex.ToString() + Environment.NewLine + " Status: " + Log_Config.LogEmail + " Query: " + query);
            }
        }

        public int DeletarLog(string tabela, int quantDias, string conexao)
        {
            int countDeletado = 0;

            string query =
                "DELETE FROM " + tabela + " " +
                "WHERE DATA <= '" + DateTime.Now.AddDays(-quantDias).ToString("yyyyMMdd") + "' ";

            using (SqlConnection connection = new SqlConnection(conexao))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    countDeletado = command.ExecuteNonQuery();
                }
            }

            return countDeletado;
        }
    }
}
