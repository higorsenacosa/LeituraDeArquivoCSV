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
        /// Criando meu banco com uma base fixa
        /// </summary>
        /// <param name="caminho">Caminho no sistema que contem a base</param>
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
        /// Remove uma pessoa da base de dados
        /// </summary>
        /// <param name="pessoa">Pessoa a ser removida</param>
        /// <returns>Devolve a base atualizada</returns>
        public Pessoa[] remover(Pessoa pessoa)
        {
            pessoas.Remove(pessoa);
            return pessoas.ToArray();
        }

        /// <summary>
        /// Atualiza uma pessoa na base de dados
        /// </summary>
        /// <param name="pessoa">Pessoa que seus dados serão atualizados</param>
        /// <returns></returns>
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
        /// <returns>Devolve a base atualizada</returns>
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
        /// Salvando nossa base de dados em um arquivo .CSV
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
            // Faça o salvamento quando se tem uma base de dados fixa
            // Estou chamando o método dentro de ManipulacaoDeArquivo
            // chamado salvarArquivo que requer o argumento de texto
            //base.salvarArquivo("");
        }

        /// <summary>
        /// Operação de salvamento do arquivo exclusivo da classe GerenciandoBD
        /// </summary>
        protected override void salvarArquivo()
        {
            //Caixa de dialogo para salvamento
            SaveFileDialog salvamento = new SaveFileDialog();

            //Manipulação dos dados
            string txt = "Nome;Sobrenome;Data de Nascimento; Telefone;Email\n";
            foreach (Pessoa pessoa in pessoas)
            {
                txt += pessoa.Nome + ";";
                txt += pessoa.Sobrenome + ";";
                txt += pessoa.DtNascimento + ";";
                txt += pessoa.Telefone + ";";
                txt += pessoa.Email + "\n";
            }

            //Abrindo meu Dialogo para salvamento
            salvamento.Filter = "Arquivo CSV|*.csv";
            salvamento.Title = "Salvar Arquivo";
            if (salvamento.ShowDialog() != DialogResult.OK
                && salvamento.FileName == null)
            {
                new Exception("Erro ao salvar o arquivo");
                return;
            }

            //Salvando o arquivo
            FileStream abrirArquivo = (FileStream)salvamento.OpenFile();
            StreamWriter salvandoArquivo = new StreamWriter(abrirArquivo);
            salvandoArquivo.WriteLine(txt);

            //Fechando o arquivo que foi aberto para salvamento
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
        /// Leitura da base com um caminho fixo
        /// </summary>
        /// <returns> Devolve a base de dados inicial</returns>
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
        /// Limpando nossa base de dados
        /// </summary>
        public void limparBD()
        {
            pessoas.Clear();
        }

    }
}
