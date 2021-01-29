using System;
using System.IO;

namespace MailingNFe.Guardian
{
    class Guardian_TxtFile
    {
        public void DefinirArquivo(string caminho, string nomeArquivo)
        {
            try
            {
                if (!File.Exists(caminho + nomeArquivo))
                {
                    using (StreamWriter sw = File.CreateText(caminho + nomeArquivo))
                    {
                        sw.WriteLine("-------------------------------------------------------------------------------------------------------");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DefinirTexto(string caminho, string nomeArquivo, string texto)
        {
            try
            {
                DefinirArquivo(caminho, nomeArquivo);

                using (StreamWriter writer = new StreamWriter(caminho + nomeArquivo, true))
                {
                    writer.Write(Environment.NewLine + texto);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
