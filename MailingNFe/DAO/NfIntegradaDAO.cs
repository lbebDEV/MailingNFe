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
    class NfNaoIntegradaDAO
    {

        public List<NfNaoIntegrada> BuscarNotas()
        {
            List<NfNaoIntegrada> notas = new List<NfNaoIntegrada>();

            string query =
                  "SELECT dEmi, serie, nNF, CNPJ_emit, xNome_emit, natOp, vNF as 'valor' FROM NFe " +
                  "INNER JOIN CABECALHO_NFE WITH(NOLOCK) ON CABECALHO_NFE.Id = NFe.Id " +
                  "WHERE NFe.operacao = 'E' " +
                  "AND SUBSTRING(NFe.Id, 4, 50) NOT IN " +
                  "(SELECT ChaveNFe FROM ConsultasSefaz WITH(NOLOCK) " +
                  "WHERE cStatus = '101') " +
                  "AND(NFe.integracao IS NULL OR NFe.integracao = 'GUARDIAN') " +
                  "ORDER BY dEmi";

            using (SqlConnection connection = new SqlConnection(ConexaoGuardian.Conexao()))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandTimeout = 240;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NfNaoIntegrada nota = new NfNaoIntegrada
                            {
                                Emissao = reader["dEmi"].ToString().Trim(),
                                Serie = reader["serie"].ToString().Trim(),
                                Numero = reader["nNF"].ToString().Trim(),
                                Cnpj = reader["CNPJ_emit"].ToString().Trim(),
                                Nome = reader["xNome_emit"].ToString().Trim(),
                                Natureza = reader["natOp"].ToString().Trim(),
                                Valor = reader["valor"].ToString().Trim(),
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
