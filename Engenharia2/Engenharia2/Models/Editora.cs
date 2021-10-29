using Engenharia2.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.Models
{
    public class Editora
    {
        private int id;
        private string nome;
        private string descricao;
        private string telefone;
        private Administrador administrador;

        public Editora(int id, string nome, string descricao, string telefone, Administrador administrador)
        {
            this.Id = id;
            this.Nome = nome;
            this.Descricao = descricao;
            this.Telefone = telefone;
            this.Administrador = administrador;
        }
        public Editora()
        {
            this.Id = 0;
            this.Nome = "";
            this.Descricao = "";
            this.Telefone = "";
            this.Administrador = null;
        }

        public int Id { get => id; set => id = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Telefone { get => telefone; set => telefone = value; }
        public Administrador Administrador { get => administrador; set => administrador = value; }

        
        public string Gravar(System.Text.Json.JsonElement dados)
        {
            string msg = "Falha ao Gravar Editora!";
            EditoraDAL editoradal = new EditoraDAL();
            Editora editora = editoradal.BuscaEditoraPorId(Convert.ToInt32(dados.GetProperty("id").ToString()));

            if(editora == null)
            {
                editora = new Editora();
                editora.Nome = dados.GetProperty("nome").ToString();
                editora.Descricao = dados.GetProperty("descricao").ToString();
                editora.Telefone = dados.GetProperty("telefone").ToString();
                if (editora.Nome.Length > 0 && editora.Descricao.Length > 0 && editora.Telefone.Length > 0)
                    msg = editoradal.gravar(editora);
                else
                    msg = "Preencha Todos os Campos";
            }
            else
            {
                editora.Nome = dados.GetProperty("nome").ToString();
                editora.Descricao = dados.GetProperty("descricao").ToString();
                editora.Telefone = dados.GetProperty("telefone").ToString();
                if (editora.Nome.Length > 0 && editora.Descricao.Length > 0 && editora.Telefone.Length > 0)
                    msg = editoradal.alterar(editora);
                else
                    msg = "Preencha Todos os Campos";
            }
            return msg;
        }

        public string Deletar(int id)
        {
            return new EditoraDAL().deletar(id);
        }

        public Editora obterEditoraPorID(int id)
        {
            return new DAL.EditoraDAL().BuscaEditoraPorId(id);
        }

        public List<Editora> obterTodasEditoras()
        {
            return new DAL.EditoraDAL().selecionarTodos();
        }

        public List<Editora> BuscarEditoraPorNome(string nome)
        {
            return new DAL.EditoraDAL().BuscaEditoraPorNome(nome);
        }
    }
}
