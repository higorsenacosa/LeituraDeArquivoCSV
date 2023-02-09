using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace LeituraDeArquivoCSV
{
    public class Log : ManipulacaoDeArquivo
    {
        const string info = "info";
        const string erro = "erro";
        const string warn = "warn";
        const string fatal = "fatal";

        public Log()
        {
            string temp = Path.GetTempPath();
                                 
            temp = temp.Replace("\\", "\\\\");
            
            base.caminho = temp + "Log do dia - " + DateTime.Today.ToString()
                .Substring(0,10).Replace("/",".") + ".log";

            if(!File.Exists(base.caminho))
                salvarArquivo("");           
        }
        public void log(int tipo, string log)
        {
            string logando = "";
            switch (tipo)
            {
                case 0:
                    logando += Log.info + " - " + log;
                    break;
                case 1:
                    logando += Log.warn + " - " + log;
                    break;
                case 2:
                    logando += Log.erro + " - " + log;
                    break;
                case 3:
                    logando += Log.fatal + " - " + log;
                    break;
                default:
                    return;
            }
            salvarArquivo(logando);
        }
        /// <summary>
        /// Salvando o arquivo de log com novas informações
        /// </summary>
        /// <param name="txt">Novo log</param>
        protected override void salvarArquivo(string txt)
        {
            if (File.Exists(base.caminho))
            {
                string logAntigo = File.ReadAllText(base.caminho);
                logAntigo += txt;
                StreamWriter salvarArquivo = new StreamWriter(base.caminho);
                salvarArquivo.WriteLine(logAntigo);
                salvarArquivo.Close();
            }
            else
            {
                StreamWriter salvarArquivo = new StreamWriter(base.caminho);
                salvarArquivo.WriteLine(iniciandoLog());
                salvarArquivo.Close();
            }
        }
        private string iniciandoLog()
        {
            return "Log criado as " + DateTime.Now;
        }
    }
}
