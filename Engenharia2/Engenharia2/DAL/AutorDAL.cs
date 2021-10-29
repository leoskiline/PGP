using Engenharia2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.DAL
{
    public class AutorDAL
    {
        MySQLPersistence _bd = new MySQLPersistence();

        public string gravar(Autor autor)
        {
            string msg = "Falha ao Gravar Autor";
            string sql = "INSERT INTO autor (Nome,idAdministrador) VALUES (@nome,@administradorId)";

            _bd.AdicionarParametro("@nome", autor.Nome);


            AdministradorDAL admDal = new AdministradorDAL();
            _bd.AdicionarParametro("@administradorId", admDal.obterIdPorNome("Leonardo Custodio dos Santos").ToString());

            _bd.AbrirConexao();
            int rows = _bd.ExecutarNonQuery(sql);
            _bd.FecharConexao();
            if (rows > 0)
            {
                msg = "Autor " + autor.Nome + " Gravado com Sucesso!";
            }
            return msg;
        }

        public string deletar(int id)
        {
            string msg = "Falha ao Deletar Autor(a)";
            string sql = "DELETE FROM autor WHERE idAutor='" + id + "'";

            string sqlAux = "SELECT * FROM livro_has_autor WHERE idAutor='" + id + "'";
            
            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sqlAux);
            
            if (dt.Rows.Count > 0)
            {
                msg = "Autor(a) possui vinculo com um Livro, porfavor deletar o Livro primeiramente!";
            }
            else
            {
                int rows = _bd.ExecutarNonQuery(sql);
                if(rows > 0)
                {
                    msg = "Autor(a) deletado(a) com Sucesso!";
                }
            }
            _bd.FecharConexao();
            return msg;
        }

        public string alterar(Autor autor)
        {
            string msg = "Falha ao Alterar Autor";
            string sql = "UPDATE autor SET nome = @nome WHERE idAutor='" + autor.Id + "';";

            _bd.LimparParametros();
            _bd.AdicionarParametro("@nome", autor.Nome);
            _bd.AbrirConexao();
            int rows = _bd.ExecutarNonQuery(sql);
            _bd.FecharConexao();

            if (rows > 0)
            {
                msg = "Autor(a) alterado(a) com Sucesso!";
            }
            return msg;
        }

        public Autor BuscaAutorPorId(int id)
        {
            string sql = "SELECT * FROM autor WHERE idAutor="+ id;
            _bd.LimparParametros();
            _bd.AdicionarParametro("@id", id.ToString());
            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            Autor autor = null;
            AdministradorDAL adm = new AdministradorDAL();
            if(dt.Rows.Count > 0)
            {
                autor = new Autor();
                autor.Id = Convert.ToInt32(dt.Rows[0]["idAutor"]);
                autor.Nome = dt.Rows[0]["Nome"].ToString();
                autor.Administrador = adm.obter("Leonardo Custodio dos Santos");
            }

            return autor;
        }

        public List<Autor> BuscaAutorPorNome(string nome)
        {
            List<Autor> autores = new List<Autor>();
            string sql = "SELECT * FROM autor WHERE Nome LIKE '%" + nome + "%'";
           // _bd.LimparParametros();
            //_bd.AdicionarParametro("@nome", nome);

            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();

            foreach (DataRow row in dt.Rows)
            {
                var autor = new Autor()
                {
                    Id = Convert.ToInt32(row["idAutor"]),
                    Nome = row["Nome"].ToString(),
                    Administrador = new AdministradorDAL().obter("Leonardo Custodio dos Santos")
                };
                autores.Add(autor);
            }

            return autores;
        }

        public List<Autor> selecionarTodos()
        {
            List<Autor> autores = new List<Autor>();
            string sql = "SELECT * FROM autor";
            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            foreach (DataRow row in dt.Rows)
            {
                var autor = new Autor()
                {
                    Id = Convert.ToInt32(row["idAutor"]),
                    Nome = row["Nome"].ToString(),
                    Administrador = new AdministradorDAL().obter("Leonardo Custodio dos Santos")

                };
                autores.Add(autor);
            }
            return autores;
        }

        public List<Autor> obterAutoresPorListID(List<int> id)
        {
            List<Autor> autores = new List<Autor>();
            int cont = id.Count;
            DataTable[] dt = new DataTable[cont];
            _bd.AbrirConexao();
            string sql;
            Autor autor = null;
            for (int i = 0;i< cont;i++)
            {
                sql = "SELECT * FROM autor WHERE idAutor = @idAutor";
                _bd.LimparParametros();
                _bd.AdicionarParametro("@idAutor", id[i].ToString());
                dt[i] = _bd.ExecutarSelect(sql);
                if (dt[i].Rows.Count > 0)
                {
                    autor = new Autor()
                    {
                        Id = Convert.ToInt32(dt[i].Rows[0]["idAutor"]),
                        Nome = dt[i].Rows[0]["Nome"].ToString(),
                        Administrador = new AdministradorDAL().obter("Leonardo Custodio dos Santos")
                    };
                    autores.Add(autor);
                }
            }
            _bd.FecharConexao();
            return autores;
        }
    }
}
