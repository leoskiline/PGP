using Engenharia2.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.Models
{
    public class Autor
    {
        private int id;
        private string nome;
        private Administrador administrador;

        public Autor(int id, string nome, Administrador administrador)
        {
            this.Id = id;
            this.Nome = nome;
            this.Administrador = administrador;
        }

        public Autor()
        {
            this.Id = 0;
            this.Nome = "";
            this.Administrador = null;
        }

        public int Id { get => id; set => id = value; }
        public string Nome { get => nome; set => nome = value; }
        public Administrador Administrador { get => administrador; set => administrador = value; }

        public List<Autor> obterTodosAutores()
        {
            return new DAL.AutorDAL().selecionarTodos();
        }

        public List<Autor> obterAutoresPorListID(List<int> id)
        {
            return new DAL.AutorDAL().obterAutoresPorListID(id);
        }

        public string Gravar(System.Text.Json.JsonElement dados)
        {
            string msg = "Falha ao Gravar Autor!";
            AutorDAL autordal = new AutorDAL();
            Autor autor = autordal.BuscaAutorPorId(Convert.ToInt32(dados.GetProperty("id").ToString()));
            
            

            if(autor == null)
            {
                autor = new Autor();
                autor.Id = Convert.ToInt32(dados.GetProperty("id").ToString());
                autor.Nome = dados.GetProperty("nome").ToString();
                if (autor.Nome.Length > 0)
                    msg = autordal.gravar(autor);
                else
                    msg = "Preencha Todos os Campos";
            }
            else
            {
                autor = new Autor();
                autor.Id = Convert.ToInt32(dados.GetProperty("id").ToString());
                autor.Nome = dados.GetProperty("nome").ToString();
                if (autor.Nome.Length > 0)
                    msg = autordal.alterar(autor);
                else
                    msg = "Preencha Todos os Campos";
            }
            return msg;
        }

        public string Deletar(int id)
        {
            return new AutorDAL().deletar(id);
        }

        public Autor obterAutorPorID(int id)
        {
            return new DAL.AutorDAL().BuscaAutorPorId(id);
        }

        public List<Autor> BuscarAutorPorNome(string nome)
        {
            return new DAL.AutorDAL().BuscaAutorPorNome(nome);
        }

    }
}
