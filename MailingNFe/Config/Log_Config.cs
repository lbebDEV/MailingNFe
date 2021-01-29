using MailingNFe.Guardian;
using System.Data.SqlClient;

namespace MailingNFe.Config
{
    class Log_Config
    {
        public static bool Status { get; set; } = false;

        public static bool LogTxt { get; set; } = false;

        public static bool LogOcorrencia { get; set; } = false;
        public static bool LogEmail { get; set; } = false;
        public static bool LogRotina { get; set; } = false;
        public static bool LogAuditoria { get; set; } = false;

        public static bool CarregarConfiguracoes()
        {
            string query = "SELECT REGISTRO_TXT, LOG_OCORRENCIA, LOG_EMAIL, LOG_ROTINA, LOG_AUDITORIA " +
                "FROM " + Tabelas_Guardian.ConfigLog + "";

            using (SqlConnection conexao = new SqlConnection(ConexaoGuardian.Conexao()))
            {
                using (SqlCommand command = new SqlCommand(query, conexao))
                {
                    conexao.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["REGISTRO_TXT"].ToString().TrimStart().TrimEnd() == "S")
                                LogTxt = true;

                            if (reader["LOG_OCORRENCIA"].ToString().TrimStart().TrimEnd() == "S")
                                LogOcorrencia = true;

                            if (reader["LOG_EMAIL"].ToString().TrimStart().TrimEnd() == "S")
                                LogEmail = true;

                            if (reader["LOG_ROTINA"].ToString().TrimStart().TrimEnd() == "S")
                                LogRotina = true;

                            if (reader["LOG_AUDITORIA"].ToString().TrimStart().TrimEnd() == "S")
                                LogAuditoria = true;
                        }
                    }
                }
            }

            Status = true;

            return Status;
        }
    }
}
