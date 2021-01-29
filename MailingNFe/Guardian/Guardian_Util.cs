using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MailingNFe.Guardian
{
    class Guardian_Util
    {
        static CultureInfo culture = new CultureInfo("pt-BR");

        public static string EliminarAspas(string nome)
        {
            if (nome.TrimStart().TrimEnd().Contains("'"))
            {
                string itemFormatado = nome.TrimStart().TrimEnd().Replace("'", "");
                nome = itemFormatado;
            }

            return nome;
        }

        public static bool ValidarEmail(string email)
        {
            email = email.Replace("'", "");

            if (email.Contains(";"))
            {
                string[] emailaux = email.Split(';');
                email = emailaux[0].ToString();
            }


            bool emailValido = false;
            int indexArroba = email.IndexOf("@");
            if (indexArroba > 0)
            {
                // Multiplos "@"
                if (email.IndexOf("@", indexArroba + 1) > 0)
                {
                    emailValido = false;
                }
                else
                {
                    int indexPonto = email.IndexOf(".", indexArroba);
                    if (indexPonto - 1 > indexArroba)
                    {
                        if (indexPonto + 1 < email.Length)
                        {
                            string indexDot2 = email.Substring(indexPonto + 1, 1);
                            if (indexDot2 != ".")
                            {
                                emailValido = true;
                            }
                        }
                    }
                    Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");
                    if (!rg.IsMatch(email))
                        emailValido = false;
                }

            }

            return emailValido;
        }

        public static string GerarSenha(int intervalo)
        {
            Random random = new Random();
            int valor = random.Next(1, intervalo);
            string senha = Criptografia.Criptografar(valor.ToString()).Substring(0, 7);

            return senha;
        }

        public static string StringMesExtenso(string mes)
        {
            string nomeMes;
            try
            {
                DateTime dataAuxiliar = new DateTime(2010, Convert.ToInt32(mes), 1);
                nomeMes = dataAuxiliar.ToString("MMMM").Substring(0, 1).ToUpper() + dataAuxiliar.ToString("MMMM").Substring(1);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return nomeMes;
        }

        public static string GerarSequenciaID(int idAnterior, int tamanho)
        {
            string ID = "";

            idAnterior++;

            for (int i = 0; i < (tamanho - idAnterior.ToString().Length); i++)
            {
                ID += "0";
            }
            ID += idAnterior.ToString();

            return ID;
        }

        public static DateTime GerarData(string horaMin)
        {
            return DateTime.ParseExact(DateTime.Now.ToString("yyyyMMdd") + horaMin + "00", "yyyyMMddHH:mmss", new CultureInfo("pt-BR", false));
        }

        public static string FormatarCaracterMEMO(string txt)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (int i = 0; i < comAcentos.Length; i++)
            {
                txt = txt.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }

            string validos = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789,.-/ÇÃÂÁÀÊÉÈÎÍÌÕÔÓÒÛÚÙûúùõôóòîíìêéèãâáàç";

            try
            {
                foreach (char c in txt)
                {
                    if (c == (char)13 || c == (char)10 || c == ' ')
                    {
                        continue;
                    }
                    else if (!validos.Contains(c.ToString()))
                    {
                        txt = txt.Replace(c, '-');
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return txt;
        }

        public static string FormatarMoeda(string valor, bool simbolo = true)
        {
            double valorFloat;
            if (valor.Trim() != "0")
            {
                if (!string.IsNullOrEmpty(valor.Trim()))
                {
                    if (simbolo)
                    {
                        valorFloat = Convert.ToDouble(valor.Replace('.', ','), culture);
                        valor = String.Format(culture, "{0:C}", valorFloat);
                    }
                    else
                    {
                        valorFloat = Convert.ToDouble(valor.Replace('.', ','), culture);
                        valor = String.Format(culture, "{0:00.00}", valorFloat);
                    }
                }
            }
            else
            {
                if (simbolo)
                    valor = "R$ " + valor + ",00";
                else
                    valor = valor + ",00";

            }

            return valor;
        }

        public static string FormatarCNPJ(string cnpj)
        {
            cnpj = cnpj.Trim();

            if (cnpj == "" || cnpj == null)
            {
                return " ";
            }

            if (cnpj.Length > 11)
            {
                cnpj = cnpj.Substring(0, 2) + "." + cnpj.Substring(2, 3) + "." + cnpj.Substring(5, 3) + "/" + cnpj.Substring(8, 4) + "-" + cnpj.Substring(12, 2);
            }
            else
            {
                cnpj = cnpj.Substring(0, 3) + "." + cnpj.Substring(3, 3) + "." + cnpj.Substring(6, 3) + "-" + cnpj.Substring(9, 2);
            }

            return cnpj;
        }

        public static string FormatarData(string data)
        {
            data = data.Trim();

            DateTime dT;

            if (data.Length > 6)
            {
                if (DateTime.TryParseExact(data, "yyyy-MM-dd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out dT))
                {
                    // the string was successfully parsed into theDate
                    data = dT.ToString("dd'/'MM'/'yyyy");
                }
                else if (DateTime.TryParseExact(data, "yyyyMMdd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out dT))
                {
                    // the string was successfully parsed into theDate
                    data = dT.ToString("dd'/'MM'/'yyyy");
                }
            }
            else
            {
                if (DateTime.TryParseExact(data, "yy-MM-dd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out dT))
                {
                    // the string was successfully parsed into theDate
                    data = dT.ToString("dd'/'MM'/'yyyy");
                }
            }

            return data;
        }

        public static string FormatarHora(string hora)
        {
            hora = hora.Trim();

            if (hora.Length == 4)
            {
                hora = hora.Substring(0, 2) + ":" + hora.Substring(2, 2);
            }

            return hora;
        }

        public static string FormatarTelefone(string telefone)
        {
            telefone = telefone.Replace("-", "").Replace("(", "").Replace(")", "");
            int tamanho = telefone.Length;
            string telefoneFormatado = "";

            switch (tamanho)
            {
                case 8:
                    telefoneFormatado = long.Parse(telefone).ToString(@"0000-0000");
                    break;
                case 9:
                    telefoneFormatado = long.Parse(telefone).ToString(@"00000-0000");
                    break;
                case 10:
                    telefoneFormatado = long.Parse(telefone).ToString(@"(00) 0000-0000");
                    break;
                case 11:
                    telefoneFormatado = long.Parse(telefone).ToString(@"(00) 00000-0000");
                    break;
                default:
                    telefoneFormatado = telefone;
                    break;
            }

            return telefoneFormatado;
        }


    }
}
