using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeituraDeArquivoCSV
{
    public partial class MainForm : Form
    {
        //Criando minhas propridades para manipular por todo o Código
        GerenciandoBD BD;
        Log log = new Log();
        public MainForm()
        {
            
            //Colocando minhas configurações iniciais do sistema
            InitializeComponent();
            limparControles();
            log.log(0, "Criação dos elementos visuais");
            
            //Obtendo minha base fixa
            try
            {               
                BD = new GerenciandoBD("C:\\Users\\higor.vmorais\\Documents\\tabela_de_higor.csv");
                lboPessoas.Items.Clear();
                lboPessoas.Items.AddRange(BD.leituraBaseComCaminho());
                btnLeitura.Text = "Salvar Dados";
                log.log(0, "Obtendo dados de uma base fixa");

            }
            catch (Exception)
            {
                BD = new GerenciandoBD();
                log.log(2, "Não foi possivel obter dados de uma base fixa");
            }
        }
        /// <summary>
        /// Setando a configuração padrão dos meus itens do WindowsForms
        /// </summary>
        private void limparControles()
        {
            btnRemover.Visible = false;
            btnEditar.Visible = false;
            txtNome.Enabled = true;
            txtSobrenome.Enabled = true;
            dtpNascimento.Enabled = true;
            txtNome.Clear();
            txtSobrenome.Clear();
            dtpNascimento.Value = DateTime.Now;
            txtTelefone.Clear();
            txtEmail.Clear();
            btnAdd.Text = "Adicionar";

        }
        /// <summary>
        /// Inserção da base de dados no ListBox
        /// </summary>
        private void btnLeitura_Click(object sender, EventArgs e)
        {
            if (btnLeitura.Text == "Inserir Dados")
            {
                //Limpa o ListBox e tenta adicionar a BD no ListBox
                try
                {
                    lboPessoas.Items.Clear();
                    lboPessoas.Items.AddRange(BD.leituraBase());
                    btnLeitura.Text = "Salvar Dados";
                }
                //Manda para a guia de erro
                catch (Exception E)
                {
                    tbPrincipal.SelectedIndex = 1;
                    lblErro.Text = E.Message;
                    return;
                }
            }
            else
            {
                try
                {
                    BD.salvandoArquivo();
                }
                catch(Exception E)
                {
                    tbPrincipal.SelectedIndex = 1;
                    lblErro.Text = E.Message;
                    return;
                }
            }
        }
        
        /// <summary>
        ///  Adição de itens em meu listbox e cancelar seleção no listbox
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Evento de adição no ListBox
            if (btnAdd.Text == "Adicionar")
            {
                //Criando meus campos para pessoa
                string nome = txtNome.Text, sobrenome = txtSobrenome.Text,
                    telefone = txtTelefone.Text, email = txtEmail.Text;
                DateTime dtNascimento = dtpNascimento.Value;

                //Fazendo a instancia da classe
                Pessoa pessoa = new Pessoa(nome, sobrenome, dtNascimento, telefone, email);
                //Recebendo a nova atualização da Base de Dados
                Pessoa[] pessoas = BD.adicionar(pessoa);
                //Limpando minha listBox
                lboPessoas.Items.Clear();
                //Atualizando a listBox
                lboPessoas.Items.AddRange(pessoas);
                //Limpar os controles
                limparControles();
            }
            //Evento de cancelamento do listBox
            else
            {
                lboPessoas.ClearSelected();
                limparControles();
            }
        }

        /// <summary>
        /// Evento de remoção do listBox
        /// </summary>
        private void btnRemover_Click(object sender, EventArgs e)
        {
            Pessoa pessoa = lboPessoas.SelectedItem as Pessoa;
            lboPessoas.ClearSelected();
            lboPessoas.Items.Clear();
            lboPessoas.Items.AddRange(BD.remover(pessoa));
            limparControles();
        }

        /// <summary>
        /// Evento de Edição de um item no ListBox
        /// </summary>
        private void btnEditar_Click(object sender, EventArgs e)
        {
            Pessoa pessoa = (Pessoa)lboPessoas.SelectedItem;
            
            //Criação e invoção dos campos Telefone e E-mail
            string telefone = txtTelefone.Text, email = txtEmail.Text;
            pessoa.Email = email;
            pessoa.Telefone = telefone;

            //Atualização do ListBox e da Classe BD
            lboPessoas.ClearSelected();
            lboPessoas.Items.Clear();
            lboPessoas.Items.AddRange(BD.updateItem(pessoa));
            limparControles();
        }

        /// <summary>
        /// Limpeza da Lista e do BD
        /// </summary>
        private void btnLimpeza_Click(object sender, EventArgs e)
        {
            lboPessoas.Items.Clear();
            BD.limparBD();
            limparControles();
        }

        /// <summary>
        /// Evento de Erro para o usuario
        /// </summary>
        private void btnErro_Click(object sender, EventArgs e)
        {
            tbPrincipal.SelectedIndex = 0;
        }

        /// <summary>
        /// Setando meus controles para edição ou remoção
        /// </summary>
        private void lboPessoas_SelectedValueChanged(object sender, EventArgs e)
        {
            //Impede que um item nulo venha para edição
            if (lboPessoas.SelectedItem == null) return;

            //Bloqueando controles para não modificar
            txtNome.Enabled = false;
            txtSobrenome.Enabled = false;
            dtpNascimento.Enabled = false;

            //Resgatar as informações do elemento
            //Faça a conversão entre classes
            Pessoa pessoa = lboPessoas.SelectedItem as Pessoa;
            txtNome.Text = pessoa.Nome;
            txtSobrenome.Text = pessoa.Sobrenome;
            txtTelefone.Text = pessoa.Telefone;
            txtEmail.Text = pessoa.Email;
            dtpNascimento.Value = pessoa.DtNascimento;

            //Criando controles
            btnEditar.Visible = true;
            btnRemover.Visible = true;
            btnAdd.Text = "Cancelar";


        }
    }
}
