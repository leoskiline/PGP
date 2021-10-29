using Engenharia2.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.DAL
{
    public class LivroDAL
    {
        MySQLPersistence _bd = new MySQLPersistence();

        public string gravar(Livro livro)
        {
            string msg = "Falha ao Gravar Livro";
            string sql = "INSERT INTO livro (Nome,idEditora,idAdministrador,qtd) VALUES (@nome,@editoraId,@administradorId,@qtd);Select @@IDENTITY;";
            
            _bd.LimparParametros();
            _bd.AdicionarParametro("@nome", livro.Nome);
            _bd.AdicionarParametro("@editoraId", new EditoraDAL().BuscaEditoraPorId(livro.Editora.Id).Id.ToString());
            _bd.AdicionarParametro("@administradorId", new AdministradorDAL().obterIdPorNome("Leonardo Custodio dos Santos").ToString());
            _bd.AdicionarParametro("@qtd", livro.Qtd.ToString());
            _bd.AbrirConexao();
            livro.Id = _bd.ExecutarNonQueryAndGetID(sql);
            string sql2 = "INSERT INTO livro_has_autor (idLivro,idAutor) VALUES (@livroId,@autorId)";
            int rows2 = 0;
            for(int i = 0; i < livro.Autor.Count; i++)
            {
                _bd.LimparParametros();
                _bd.AdicionarParametro("@livroId", livro.Id.ToString());
                _bd.AdicionarParametro("@autorId", livro.Autor[i].Id.ToString());
                rows2 += _bd.ExecutarNonQuery(sql2);
            }
            _bd.FecharConexao();
            if(livro.Id != 0 && rows2 > 0)
            {
                msg = "Livro " + livro.Nome + " Gravado com Sucesso!";
            }
            return msg;
        }

        public string deletar(int id)
        {
            string msg = "Falha ao deletar Livro!";
            string sql = "DELETE FROM livro_has_autor WHERE Livro_idLivro=@idLivro";
            string sql2 = "DELETE FROM exemplar WHERE idLivro=@idLivro";
            string sql3 = "DELETE FROM livro WHERE idLivro=@idLivro";
            _bd.LimparParametros();
            _bd.AdicionarParametro("@idLivro", id.ToString());
            int rows, rows2,rows3;
            _bd.AbrirConexao();
            rows = _bd.ExecutarNonQuery(sql);
            rows2 = _bd.ExecutarNonQuery(sql2);
            rows3 = _bd.ExecutarNonQuery(sql3);
            _bd.FecharConexao();
            if (rows > 0 || rows2 > 0 || rows3 > 0)
            {
                msg = "Livro Deletado com Sucesso!";
            }
            
            return msg;
        }

        public void menosUm(Livro l)
        {
            string sql = "UPDATE livro SET qtd= @qtd WHERE idLivro='" + l.Id + "';";

            _bd.LimparParametros();
            _bd.AdicionarParametro("@qtd", Convert.ToString(l.Qtd-1));
            _bd.AbrirConexao();
            int rows = _bd.ExecutarNonQuery(sql);
            _bd.FecharConexao();
        }

        public void maisUm(Livro l)
        {
            string sql = "UPDATE livro SET qtd= @qtd WHERE idLivro='" + l.Id + "';";

            _bd.LimparParametros();
            _bd.AdicionarParametro("@qtd", Convert.ToString(l.Qtd + 1));
            _bd.AbrirConexao();
            int rows = _bd.ExecutarNonQuery(sql);
            _bd.FecharConexao();
        }

        public string alterar(Livro livro)
        {
            return "Falha ao Alterar";
        }

        //Seleção simples de Livros
        public List<Livro> selecionarTodos()
        {
            List<Livro> livros = new List<Livro>();
            string sql = "SELECT * FROM livro";
            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            EditoraDAL edal = new EditoraDAL();
            foreach (DataRow row in dt.Rows)
            {
                var livro = new Livro()
                {
                    Id = Convert.ToInt32(row["idLivro"]),
                    Nome = row["nome"].ToString(),
                    Editora = edal.BuscaEditoraPorId(Convert.ToInt32(row["idEditora"])),

                };
                livros.Add(livro);
            }
            return livros;
        }

        public List<Livro> obterLivrosPorListID(List<int> id)
        {
            List<Livro> livros= new List<Livro>();
            int cont = id.Count;
            DataTable[] dt = new DataTable[cont];
            _bd.AbrirConexao();
            string sql;
            Livro livro = null;
            for (int i = 0; i < cont; i++)
            {
                sql = "SELECT livro.idLivro,livro.Nome,livro.qtd,livro.idEditora,livro.idAdministrador,autor.idAutor FROM livro " +
                      "INNER JOIN editora ON editora.idEditora = livro.idEditora " +
                      "INNER JOIN livro_has_autor ON livro_has_autor.idLivro = livro.idLivro " +
                      "INNER JOIN autor ON livro_has_autor.idAutor = autor.idAutor " +
                      "where livro.idLivro =" + id[i];
                _bd.LimparParametros();
                _bd.AdicionarParametro("@idLivro", id[i].ToString());
                dt[i] = _bd.ExecutarSelect(sql);
                if (dt[i].Rows.Count > 0)
                {
                    foreach (DataRow row in dt[i].Rows)
                    {
                        if (livros.Exists(l => l.Id == Convert.ToInt32(row[0])))
                        {
                            foreach (Livro l in livros)
                            {
                                if (l.Id == Convert.ToInt32(row[0]))
                                {
                                    l.Autor.Add(new AutorDAL().BuscaAutorPorId(Convert.ToInt32(row[5])));
                                }
                            }
                        }
                        else
                        {
                            List<int> autoresId = new List<int>();
                            livro = new Livro();
                            livro.Id = Convert.ToInt32(row[0]);
                            livro.Nome = row[1].ToString();
                            livro.Qtd = Convert.ToInt32(row[2]);
                            livro.Editora = new EditoraDAL().BuscaEditoraPorId(Convert.ToInt32(row[3]));
                            livro.Administrador = new AdministradorDAL().obter("Leonardo Custodio dos Santos");
                            autoresId.Add(Convert.ToInt32(row[5]));
                            livro.Autor = new AutorDAL().obterAutoresPorListID(autoresId);
                            livros.Add(livro);
                        }
                    }
                }
            }
            _bd.FecharConexao();
            return livros;
        }

        public List<Livro> pesquisarLivros(string termo,string tipo)
        {
            string sql = "";
            if (tipo == "nome")
            {
                sql = "SELECT livro.idLivro,livro.Nome,livro.qtd,livro.idEditora,livro.idAdministrador,autor.idAutor FROM livro " +
                      "INNER JOIN editora ON editora.idEditora = livro.idEditora " +
                      "INNER JOIN livro_has_autor ON livro_has_autor.idLivro = livro.idLivro " +
                      "INNER JOIN autor ON livro_has_autor.idAutor = autor.idAutor " +
                      "where livro.Nome LIKE '%"+termo+"%'";
            }
            else if(tipo == "editora")
            {
                sql = "SELECT livro.idLivro,livro.Nome,livro.qtd,livro.idEditora,livro.idAdministrador,autor.idAutor FROM livro " +
                      "INNER JOIN editora ON editora.idEditora = livro.idEditora " +
                      "INNER JOIN livro_has_autor ON livro_has_autor.idLivro = livro.idLivro " +
                      "INNER JOIN autor ON livro_has_autor.idAutor = autor.idAutor " +
                      "where editora.Nome LIKE '%"+termo+"%'";
            }
            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            List<Livro> livros = new List<Livro>();
            if(dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if(livros.Exists(l => l.Id == Convert.ToInt32(row[0])))
                    {
                        foreach (Livro l in livros)
                        {
                            if (l.Id == Convert.ToInt32(row[0]))
                            {
                                l.Autor.Add(new AutorDAL().BuscaAutorPorId(Convert.ToInt32(row[5])));
                            }
                        }
                    }
                    else
                    {
                        List<int> autoresId = new List<int>();
                        Livro livro = new Livro();
                        livro.Id = Convert.ToInt32(row[0]);
                        livro.Nome = row[1].ToString();
                        livro.Qtd = Convert.ToInt32(row[2]);
                        livro.Editora = new EditoraDAL().BuscaEditoraPorId(Convert.ToInt32(row[3]));
                        livro.Administrador = new AdministradorDAL().obter("Leonardo Custodio dos Santos");
                        autoresId.Add(Convert.ToInt32(row[5]));
                        livro.Autor = new AutorDAL().obterAutoresPorListID(autoresId);
                        livros.Add(livro);
                    }
                }
            }
            return livros;
        }

        

        public Livro seleciona(int id){
            string sql = "SELECT livro.idLivro,livro.Nome,livro.qtd,livro.idEditora,livro.idAdministrador,autor.idAutor FROM livro " +
                       "INNER JOIN editora ON editora.idEditora = livro.idEditora " +
                       "INNER JOIN livro_has_autor ON livro_has_autor.idLivro = livro.idLivro " +
                       "INNER JOIN autor ON livro_has_autor.idAutor = autor.idAutor " +
                       "where livro.idLivro = @id";
            _bd.LimparParametros();
            _bd.AdicionarParametro("@id", id.ToString());
            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            List<Livro> livros = new List<Livro>();
            Livro livro = null;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (livros.Exists(l => l.Id == Convert.ToInt32(row[0])))
                    {
                        foreach (Livro l in livros)
                        {
                            if (l.Id == Convert.ToInt32(row[0]))
                            {
                                l.Autor.Add(new AutorDAL().BuscaAutorPorId(Convert.ToInt32(row[5])));
                            }
                        }
                    }
                    else
                    {
                        List<int> autoresId = new List<int>();
                        livro = new Livro();
                        livro.Id = Convert.ToInt32(row[0]);
                        livro.Nome = row[1].ToString();
                        livro.Qtd = Convert.ToInt32(row[2]);
                        livro.Editora = new EditoraDAL().BuscaEditoraPorId(Convert.ToInt32(row[3]));
                        livro.Administrador = new AdministradorDAL().obter("Leonardo Custodio dos Santos");
                        autoresId.Add(Convert.ToInt32(row[5]));
                        livro.Autor = new AutorDAL().obterAutoresPorListID(autoresId);
                        livros.Add(livro);
                    }
                }
            }
            return livro;
        }
    }
}
