using MailingNFe.DAO;
using MailingNFe.Guardian;
using MailingNFe.ServiceLog;
using System;
using System.Globalization;
using System.Xml;

namespace MailingNFe.Config
{
    class Service_Config
    {
        public static bool Status { get; set; } = true;

        public static bool CadastroHabilitado { get; set; } = false;

        public static string NomeServico { get; set; } = "Mailing NFe";

        public static string NomeCliente { get; set; } = "LBeB";

        public static double DelayCiclo { get; set; } = 5;

        public static int DataValidade { get; set; } = 0;

        public static DateTime UploadHoraInicio { get; set; }

        public static double DelayUpload { get; set; } = 2;

        public static DateTime UploadHoraFim { get; set; }

        public static DateTime DataUpload { get; set; }

        public static string EmailValidacao { get; set; }

        public static string TipoUpload { get; set; }

        public static string ValorUpload { get; set; }

        public static string TopRegistros { get; set; }

#if (DEBUG)
        public const string ArquivoConfig = "Portal_Config_Debug.xml";
#else
        public const string ArquivoConfig = "Portal_Config.xml";
#endif

        public static bool CarregarConfiguracoes()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                ConfigBancoDAO bancoDAO = new ConfigBancoDAO();

                #region Conexão SQL
                xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + ArquivoConfig);
                XmlElement conexao = xmlDoc.GetElementsByTagName("Configuracoes")[0]["Conexao"];

                ConexaoPortal.Servidor = conexao["Portal"]["Servidor"].InnerText;
                ConexaoPortal.Banco = conexao["Portal"]["Banco"].InnerText;
                ConexaoPortal.Login = conexao["Portal"]["Login"].InnerText;
                ConexaoPortal.Senha = Criptografia.Descriptografar(conexao["Portal"]["Senha"].InnerText);

                ConexaoGuardian.Servidor = conexao["Guardian"]["Servidor"].InnerText;
                ConexaoGuardian.Banco = conexao["Guardian"]["Banco"].InnerText;
                ConexaoGuardian.Login = conexao["Guardian"]["Login"].InnerText;
                ConexaoGuardian.Senha = Criptografia.Descriptografar(conexao["Guardian"]["Senha"].InnerText);

                ConexaoERP.Servidor = conexao["ERP"]["Servidor"].InnerText;
                ConexaoERP.Banco = conexao["ERP"]["Banco"].InnerText;
                ConexaoERP.Login = conexao["ERP"]["Login"].InnerText;
                ConexaoERP.Senha = Criptografia.Descriptografar(conexao["ERP"]["Senha"].InnerText);

                ConexaoGestor.Servidor = conexao["Gestor"]["Servidor"].InnerText;
                ConexaoGestor.Banco = conexao["Gestor"]["Banco"].InnerText;
                ConexaoGestor.Login = conexao["Gestor"]["Login"].InnerText;
                ConexaoGestor.Senha = Criptografia.Descriptografar(conexao["Gestor"]["Senha"].InnerText);

                #endregion
                
                #region Carregar Configurações do Serviço

                bancoDAO.BuscarConfigs(false);
                
                #endregion

                #region Carregar Configurações de Log

                Log_Config.CarregarConfiguracoes();

                #endregion
            }
            catch (Exception ex)
            {
                Status = false;
                Guardian_LogTxt.LogAplicacao(NomeServico, "Erro ao executar a busca das configurações." + ex.ToString() + ex.StackTrace);
                Guardian_Log.Log_Ocorrencia(NomeServico, "Erro ao executar a busca das configurações.", ex.ToString(), "");
            }
            return Status;
        }


        public static DateTime DefinirDataUpload(string tipo, string valor)
        {
            DateTime dataUpload = new DateTime();

            switch (tipo)
            {
                case "A":
                    dataUpload = new DateTime(DateTime.Now.AddYears(-int.Parse(valor)).Year, 1, 1);
                    break;

                case "M":
                    dataUpload = DateTime.Now.AddMonths(-int.Parse(valor));
                    break;

                case "D":
                    dataUpload = DateTime.ParseExact(valor, "yyyyMMdd", new CultureInfo("pt-BR", false));
                    break;

                default:
                    throw new Exception();
            }

            return dataUpload;
        }
    }
}
