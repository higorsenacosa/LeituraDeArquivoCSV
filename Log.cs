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
        //Constante dos tipos de logs existentes
        const string info = "info";
        const string erro = "erro";
        const string warn = "warn";
        const string fatal = "fatal";
        private string iniciandoLog = "Log criado as " + DateTime.Now;
        /// <summary>
        /// Criação do sistema de Log para o nosso sistema
        /// </summary>
        public Log()
        {
            //Obtendo o caminho de pasta temporaria no Windows
            string temp = Path.GetTempPath();

            //Realizando o tratamento para futuro salvamento na pasta temporaria
            temp = temp.Replace("\\", "\\\\");
            
            //Criação do caminho para o arquivo que será salvo
            base.caminho = temp + "Log do dia - " + DateTime.Today.ToString()
                .Substring(0,10).Replace("/",".") + ".log";

            //Verifica se o arquivo existe e cria um cabeçalho para ele
            if(!File.Exists(base.caminho))
                salvarArquivo("");           
        }

        /// <summary>
        /// Criação de um log
        /// </summary>
        /// <param name="tipo">Qual é o nivel do log:
        ///<para> 0 - Info</para>
        ///<para> 1 - Warn </para>
        ///<para> 2 - Erro </para>
        ///<para> 3 - Fatal</para>
        ///</param>
        /// <param name="log">Qual a ocorrencia</param>
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
            //Inserindo informação de log quando existe um arquivo de log
            if (File.Exists(base.caminho))
            {
                string logAntigo = File.ReadAllText(base.caminho);
                logAntigo += txt;
                StreamWriter salvarArquivo = new StreamWriter(base.caminho);
                salvarArquivo.WriteLine(logAntigo);
                salvarArquivo.Close();
            }
            //Inserindo o cabeçalho de logs
            else
            {
                StreamWriter salvarArquivo = new StreamWriter(base.caminho);
                salvarArquivo.WriteLine(iniciandoLog);
                salvarArquivo.Close();
            }
        }
        
    }
}
