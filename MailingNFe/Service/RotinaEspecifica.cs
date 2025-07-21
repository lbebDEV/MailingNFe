using MailingNFe.Guardian;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingNFe.Service
{
    abstract class RotinaEspecifica : RotinaServico
    {
        public int Hora { get; set; }
        public string Descricao { get; set; }
        public DateTime UltimaExecucao { get; set; }
        public bool ExecutarRotina { get; set; } = false;

        public void ValidarExecucao()
        {
            ControleRotina controleRotina = new ControleRotina();

            ExecutarRotina = false;

            if (AtrasoExecucao(1))
            {
                ExecutarRotina = true;
            }
            else
            {
                if (controleRotina.ControleHora(Hora))
                {
                    if (controleRotina.ControleFlag(UltimaExecucao))
                    {
                        ExecutarRotina = true;
                    }
                }
            }
        }

        public void RegistrarExecucao()
        {
            string query =
                "INSERT INTO " + Tabelas_Guardian.RegistroRotina + " " +
                "(NOME, DATA_REGISTRO, DESCRICAO) " +
                "VALUES('" + Nome + "', '" + DateTime.Now.ToString("yyyyMMdd") + "', '" + Descricao + "')";

            using (SqlConnection connection = new SqlConnection(ConexaoGuardian.Conexao()))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public DateTime BuscarUltimaExecucao()
        {
            DateTime dtUltimaExecucao = new DateTime();

            string query =
            "SELECT TOP 1 DATA_REGISTRO " +
            "FROM " + Tabelas_Guardian.RegistroRotina + " " +
            "WHERE NOME = '" + Nome + "' " +
            "ORDER BY DATA_REGISTRO DESC ";

            using (SqlConnection connection = new SqlConnection(ConexaoGuardian.Conexao()))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandTimeout = 500;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dtUltimaExecucao = DateTime.ParseExact(reader["DATA_REGISTRO"].ToString() + "235959", "yyyyMMddHHmmss", new CultureInfo("pt-BR", false));
                        }
                        else
                        {
                            dtUltimaExecucao = DateTime.Now;
                        }
                    }
                }
            }

            return dtUltimaExecucao;
        }

        public bool AtrasoExecucao(int limiteDias)
        {
            if (UltimaExecucao == DateTime.MinValue)
                return true;

            int diasAtraso = (DateTime.Now - UltimaExecucao).Days;
            return diasAtraso >= limiteDias;
        }

    }
}
