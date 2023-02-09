using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeituraDeArquivoCSV
{
    public class GerenciandoBD : ManipulacaoDeArquivo
    {
        // Atributo/Propriedades da classe GerenciandoBD
        private List<Pessoa> pessoas;
        /// <summary>
        /// Construindo minha base generica
        /// </summary>
        public GerenciandoBD()
        {
            pessoas = new List<Pessoa>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="caminho"></param>
        public GerenciandoBD(string caminho)
        {
            //Está chamando o atributo de ManipulacaoDeArquivo
            //Chamado caminho
            base.caminho = caminho;
            pessoas = new List<Pessoa>();
        }
        /// <summary>
        /// Adicionando novas entradas de dados
        /// </summary>
        /// <param name="pessoa">A atualização da base de dados</param>
        /// <returns>Retornando a atualização da base de dados</returns>
        public Pessoa[] adicionar(Pessoa pessoa)
        {
            pessoas.Add(pessoa);
            return pessoas.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pessoa"></param>
        /// <returns></returns>
        public Pessoa[] remover(Pessoa pessoa)
        {
            pessoas.Remove(pessoa);
            return pessoas.ToArray();
        }
        public Pessoa[] updateItem(Pessoa pessoa)
        {
            for(int i = 0; i<pessoas.Count; i++)
            {
                if (pessoas[i] == pessoa) pessoas[i] = pessoa;
            }
            return pessoas.ToArray();
        }
        /// <summary>
        /// Sobreescreve o método obterDados
        /// Com um caminho definido
        /// </summary>
        /// <returns></returns>
        protected override string obterDados()
        {
            //Ele tenta fazer a leitura do arquivo, caso não consiga
            //Impede uma excessão, retornando nulo;
            try
            {
                string textoLido = File.ReadAllText(caminho);
                return textoLido;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void salvandoArquivo()
        {
            if (base.caminho == null)
            {
                string txt = "Nome;Sobrenome;Data de Nascimento; Telefone;Email\n";
                foreach (Pessoa pessoa in pessoas)
                {
                    txt += pessoa.Nome + ";";
                    txt += pessoa.Sobrenome + ";";
                    txt += pessoa.DtNascimento + ";";
                    txt += pessoa.Telefone + ";";
                    txt += pessoa.Email + "\n";
                }
                base.salvarArquivo(txt);
            }
            // Estou chamando o método dentro de ManipulacaoDeArquivo
            // chamado salvarArquivo que requer o argumento de texto
            //base.salvarArquivo("");
        }
        protected override void salvarArquivo()
        {
            SaveFileDialog salvamento = new SaveFileDialog();
            string txt = "Nome;Sobrenome;Data de Nascimento; Telefone;Email\n";
            foreach (Pessoa pessoa in pessoas)
            {
                txt += pessoa.Nome + ";";
                txt += pessoa.Sobrenome + ";";
                txt += pessoa.DtNascimento + ";";
                txt += pessoa.Telefone + ";";
                txt += pessoa.Email + "\n";
            }

            salvamento.Filter = "Arquivo CSV|*.csv";
            salvamento.Title = "Salvar Arquivo";
            if (salvamento.ShowDialog() != DialogResult.OK
                && salvamento.FileName == null)
            {
                new Exception("Erro ao salvar o arquivo");
                return;
            }
            FileStream abrirArquivo = (FileStream)salvamento.OpenFile();
            StreamWriter salvandoArquivo = new StreamWriter(abrirArquivo);
            salvandoArquivo.WriteLine(txt);
            salvandoArquivo.Close();
            abrirArquivo.Close();
        }
        /// <summary>
        /// Leitura da base e organização dos dados
        /// </summary>
        /// <returns>Retorna minha base de dados</returns>
        public Pessoa[] leituraBase()
        {
            //Obtendo o arquivo - ManipulacaoDeArquivo
            string textoLido = base.obterDados();
            //Lanço uma excessão se não foi possivel ler o arquivo
            if (textoLido == null) throw new Exception("" +
                "Erro na leitura do arquivo");
            //Tratando os dados que estão no arquivo
            pessoas.Clear();
            int i = 0;
            foreach (var linha in textoLido.Split('\n'))
            {
                if (linha == "" || linha == "\r") break;
                if (i != 0)
                {
                    //Tratando dos dados
                    string[] tratamento = linha.Split(';');
                    //Criando o objeto de Pessoa
                    Pessoa ps = new Pessoa(tratamento[0], tratamento[1], DateTime.Parse(tratamento[2]), tratamento[3], tratamento[4]);
                    //Adicionando em pessoas as informações do arquivo
                    pessoas.Add(ps);
                }
                i++;
            }
            //Retornando os dados que foram tratados
            return pessoas.ToArray();
        }
        /// <summary>
        /// Comente
        /// </summary>
        /// <returns></returns>
        public Pessoa[] leituraBaseComCaminho()
        {
            //Obtendo o arquivo - ManipulacaoDeArquivo
            string textoLido = obterDados();
            //Lanço uma excessão se não foi possivel ler o arquivo
            if (textoLido == null) throw new Exception("" +
                "Erro na leitura do arquivo");
            //Tratando os dados que estão no arquivo
            pessoas.Clear();
            int i = 0;
            foreach (var linha in textoLido.Split('\n'))
            {
                if (linha == "" || linha == "\r") break;
                if (i != 0)
                {
                    //Tratando dos dados
                    string[] tratamento = linha.Split(';');
                    //Criando o objeto de Pessoa
                    Pessoa ps = new Pessoa(tratamento[0], tratamento[1], DateTime.Parse(tratamento[2]), tratamento[3], tratamento[4]);
                    //Adicionando em pessoas as informações do arquivo
                    pessoas.Add(ps);
                }
                i++;
            }
            //Retornando os dados que foram tratados
            return pessoas.ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        public void limparBD()
        {
            pessoas.Clear();
        }

    }
}
