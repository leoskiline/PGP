using Engenharia2.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.Models
{
    public class Livro
    {
        private int id;
        private string nome;
        private Editora editora;
        private Administrador administrador;
        private List<Autor> autor; // List<Autor>
        private int qtd;

        public int Id { get => id; set => id = value; }
        public string Nome { get => nome; set => nome = value; }
        public Editora Editora { get => editora; set => editora = value; }
        public Administrador Administrador { get => administrador; set => administrador = value; }
        public List<Autor> Autor { get => autor; set => autor = value; }
        public int Qtd { get => qtd; set => qtd = value; }

        public Livro()
        {
            
        }

        public Livro(int id, string nome, Editora editora, Administrador administrador, List<Autor> autor, int qtd)
        {
            this.id = id;
            this.nome = nome;
            this.editora = editora;
            this.administrador = administrador;
            this.autor = autor;
            this.qtd = qtd;
        }

        public Livro(string nome, Editora editora, Administrador administrador, List<Autor> autor, int qtd)
        {
            Nome = nome;
            Editora = editora;
            Administrador = administrador;
            Autor = autor;
            Qtd = qtd;
        }

        public List<Livro> obterTodosLivros()
        {
            return new DAL.LivroDAL().selecionarTodos();
        }

        public string Gravar(string nome,List<Autor> autor,Editora editora,Administrador adm,int qtd)
        {
            string msg = "Falha ao Gravar Livro!";
            LivroDAL livrodal = new LivroDAL();
            Livro livro = new Livro(nome, editora, adm, autor, qtd);
            msg = livrodal.gravar(livro);
            return msg;
        }

        public string Alterar(string nome, List<Autor> autor, Editora editora, Administrador adm, int qtd)
        {
            string msg = "Falha ao Gravar Livro!";
            LivroDAL livrodal = new LivroDAL();
            Livro livro = new Livro(nome, editora, adm, autor, qtd);
            msg = livrodal.alterar(livro);
            return msg;
        }

        public List<Livro> PesquisarLivros(string termo,string tipo)
        {
            List<Livro> livros = new LivroDAL().pesquisarLivros(termo, tipo);

            return livros;
        }

        public string DeletarLivro(int id)
        {
            return new LivroDAL().deletar(id);
        }

        public Livro buscarLivroPorID(int id)
        {
            return new LivroDAL().seleciona(id);
        }
    }
}
