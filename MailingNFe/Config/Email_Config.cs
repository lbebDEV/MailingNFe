using MailingNFe.Email;
using MailingNFe.Guardian;
using System.Data.SqlClient;

namespace MailingNFe.Config
{
    class Email_Config
    {
        public static GuardianEmail CarregarConfiguracoes()
        {
            GuardianEmail email = new GuardianEmail();

            string query =
                " SELECT TOP 1 HOST, PORTA, SSL, LOGIN, SENHA, EMAIL, EMAIL_VALIDACAO " +
                " FROM " + Tabelas_Portal.ConfigEmail + " ";

            using (SqlConnection connection = new SqlConnection(ConexaoGestor.Conexao()))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            email.Servidor = reader["HOST"].ToString().Trim();
                            email.Porta = int.Parse(reader["PORTA"].ToString().Trim());
                            if (reader["SSL"].ToString().Trim().Equals("S"))
                                email.Ssl = true;
                            else
                                email.Ssl = false;
                            email.Login = reader["LOGIN"].ToString().Trim();
                            email.Senha = Criptografia.Descriptografar(reader["SENHA"].ToString().Trim());
                            email.EmailRemetente = reader["EMAIL"].ToString().Trim();
                        }
                    }
                }
            }

            return email;
        }
    }
}
