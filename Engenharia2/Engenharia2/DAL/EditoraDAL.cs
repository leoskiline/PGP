using Engenharia2.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.DAL
{
    public class EditoraDAL
    {
        MySQLPersistence _bd = new MySQLPersistence();

        public string gravar(Editora editora)
        {
            string msg = "Falha ao Gravar Editora";
            string sql = "INSERT INTO editora (Nome,Descricao,Telefone,idAdministrador) VALUES (@nome,@descricao,@telefone,@administradorId)";
            _bd.AdicionarParametro("@nome", editora.Nome);
            _bd.AdicionarParametro("@descricao", editora.Descricao);
            _bd.AdicionarParametro("@telefone", editora.Telefone);
            AdministradorDAL admDal = new AdministradorDAL();
            _bd.AdicionarParametro("@administradorId", admDal.obterIdPorNome("Leonardo Custodio dos Santos").ToString());
            _bd.AbrirConexao();
            int rows =  _bd.ExecutarNonQuery(sql);
            _bd.FecharConexao();
            if(rows > 0)
            {
                msg = "Editora " + editora.Nome + " Gravada com Sucesso!";
            }
            return msg;
        }

        public string deletar(int id)
        {
            string msg = "Falha ao Deletar Editora";
            string sql = "DELETE FROM editora WHERE idEditora='"+ id +"'";
            
            _bd.AbrirConexao();
            int rows = _bd.ExecutarNonQuery(sql);
            _bd.FecharConexao();
            if (rows > 0)
            {
                msg = "Editora deletada com Sucesso!";
            }
            return msg;
        }

        public string alterar(Editora editora)
        {
            string msg = "Falha ao Alterar Editora";
            string sql = "UPDATE editora SET nome = @nome, descricao=@descricao, telefone=@telefone WHERE idEditora='" + editora.Id + "'";

            _bd.LimparParametros();
            _bd.AdicionarParametro("@nome", editora.Nome);
            _bd.AdicionarParametro("@descricao", editora.Descricao);
            _bd.AdicionarParametro("@telefone", editora.Telefone);

            _bd.AbrirConexao();
            int rows = _bd.ExecutarNonQuery(sql);
            _bd.FecharConexao();
            if (rows > 0)
            {
                msg = "Editora alterada com Sucesso!";
            }
            return msg;
        }

        //Seleção simples de lista
        public List<Editora> selecionarTodos()
        {
            List<Editora> editoras = new List<Editora>();
            string sql = "SELECT * FROM editora";
            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            foreach (DataRow row in dt.Rows)
            {
                var editora = new Editora()
                {
                    Id = Convert.ToInt32(row["idEditora"]),
                    Nome = row["Nome"].ToString(),
                    Descricao = row["Descricao"].ToString(),
                    Telefone = row["Telefone"].ToString(),
                    Administrador = new AdministradorDAL().obter("Leonardo Custodio dos Santos")

                };
                editoras.Add(editora);
            }
            return editoras;
        }

        

        public Editora BuscaEditoraPorId(int id)
        {
            string sql = "SELECT * FROM editora WHERE idEditora=@id";
            _bd.LimparParametros();
            _bd.AdicionarParametro("@id", id.ToString());
            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            Editora editora = null;
            AdministradorDAL adm = new AdministradorDAL();

            if (dt.Rows.Count > 0)
            {
                editora = new Editora();

                
                editora.Id = Convert.ToInt32(dt.Rows[0]["idEditora"]);
                editora.Nome = dt.Rows[0]["Nome"].ToString();
                editora.Descricao = dt.Rows[0]["Descricao"].ToString();
                editora.Telefone = dt.Rows[0]["Telefone"].ToString();
                editora.Administrador = adm.obter("Leonardo Custodio dos Santos");

            }

            return editora;
        }

        public List<Editora> BuscaEditoraPorNome(string nome)
        {
            List<Editora> editoras = new List<Editora>();
            string sql = "SELECT * FROM editora WHERE Nome LIKE '%"+nome+"%'";
            //_bd.LimparParametros();
            //_bd.AdicionarParametro("@nome", nome);

            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();

            
            foreach(DataRow row in dt.Rows)
            {
                var editora = new Editora()
                {
                    Id = Convert.ToInt32(row["idEditora"].ToString()),
                    Nome = row["Nome"].ToString(),
                    Descricao = row["Descricao"].ToString(),
                    Telefone = row["Telefone"].ToString(),
                    Administrador = new AdministradorDAL().obter("Leonardo Custodio dos Santos")
                };
                editoras.Add(editora);
            }
            return editoras;
        }
    }
}
