using MailingNFe.Guardian;
using MailingNFe.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingNFe.DAO
{
    class NfClassificadaDAO
    {

        public List<NfNaoClassificada> BuscarNotas()
        {
            List<NfNaoClassificada> notas = new List<NfNaoClassificada>();

            string query =
                  "SELECT SF1.F1_EMISSAO AS 'EMISSAO', SF1.F1_SERIE AS 'SERIE', SF1.F1_DOC AS 'NUMERO', SA2.A2_CGC AS 'CNPJ', " +
                  "SA2.A2_NOME AS 'NOME', SUM(SD1.D1_QUANT) AS 'QUANTIDADE', SF1.F1_VALBRUT AS 'VALOR' " +
                  "FROM " + Tabelas_ERP.SF1 + " AS SF1 " +
                  "INNER JOIN " + Tabelas_ERP.SD1 + " AS SD1 ON SF1.F1_DOC = SD1.D1_DOC AND SF1.F1_SERIE = SD1.D1_SERIE AND SF1.F1_FILIAL = SD1.D1_FILIAL AND SF1.F1_LOJA = SD1.D1_LOJA AND SF1.F1_FORNECE = SD1.D1_FORNECE " +
                  "LEFT JOIN " + Tabelas_ERP.SA2 + " AS SA2 ON SF1.F1_FORNECE = SA2.A2_COD " +
                  "WHERE SF1.D_E_L_E_T_ = '' AND SD1.D_E_L_E_T_ = '' AND SD1.D1_TES = '' " +
                  "GROUP BY SF1.F1_EMISSAO, SF1.F1_SERIE, SF1.F1_DOC, SA2.A2_CGC, SA2.A2_NOME, SF1.F1_VALBRUT " +
                  "ORDER BY SF1.F1_EMISSAO ";

            using (SqlConnection connection = new SqlConnection(ConexaoERP.Conexao()))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandTimeout = 240;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NfNaoClassificada nota = new NfNaoClassificada
                            {
                                Emissao = reader["EMISSAO"].ToString().Trim(),
                                Serie = reader["SERIE"].ToString().Trim(),
                                Numero = reader["NUMERO"].ToString().Trim(),
                                Cnpj = reader["CNPJ"].ToString().Trim(),
                                Nome = reader["NOME"].ToString().Trim(),
                                Quantidade = reader["QUANTIDADE"].ToString().Trim(),
                                Valor = reader["VALOR"].ToString().Trim(),
                            };

                            notas.Add(nota);
                        }
                    }
                }
            }

            return notas;
        }
    }
}
