using Engenharia2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.DAL
{
    public class LeitorDAL
    {
        MySQLPersistence _bd = new MySQLPersistence();

        public string gravar(Leitor leitor)
        {
            string msg = "Falha ao Gravar Leitor";
            string sql = "INSERT INTO leitor (nome,cpf,endereco,dataNasc,idAtendente) VALUES (@nome,@cpf,@endereco,@dataNasc,@atendenteId)";
            _bd.AdicionarParametro("@nome", leitor.Nome);
            _bd.AdicionarParametro("@cpf", leitor.Cpf);
            _bd.AdicionarParametro("@endereco", leitor.Endereco);
            _bd.AdicionarParametro("@dataNasc", leitor.DataNasc.ToString("yyyy-MM-dd HH:mm:ss"));
            AtendenteDAL atDal = new AtendenteDAL();
            _bd.AdicionarParametro("@atendenteId", atDal.obterIdPorNome("Maria Luiza").ToString());
            _bd.AbrirConexao();
            int rows = _bd.ExecutarNonQuery(sql);
            _bd.FecharConexao();
            if (rows > 0)
            {
                msg = "Leitor " + leitor.Nome + " Gravado com Sucesso!";
            }
            return msg;
        }

        public List<Leitor> selecionarTodos()
        {
            List<Leitor> leitores = new List<Leitor>();
            string sql = "SELECT * FROM leitor";
            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            foreach (DataRow row in dt.Rows)
            {
                var leitor = new Leitor()
                {
                    Id = Convert.ToInt32(row["idLeitor"]),
                    Nome = row["Nome"].ToString(),
                    Cpf = row["CPF"].ToString(),
                    Endereco = row["Endereco"].ToString(),
                    DataNasc = Convert.ToDateTime(row["DataNasc"].ToString())

                };
                leitores.Add(leitor);
            }
            return leitores;
        }

        public string deletar(int id)
        {
            string msg = "Falha ao Deletar Leitor";
            string sql = "DELETE FROM leitor WHERE idLeitor='" + id + "'";

            _bd.AbrirConexao();
            int rows = _bd.ExecutarNonQuery(sql);
            _bd.FecharConexao();
            if (rows > 0)
            {
                msg = "Leitor deletado com Sucesso!";
            }
            return msg;
        }

        public string alterar(Leitor leitor)
        {
            string msg = "Falha ao Alterar Leitor";
            string sql = "UPDATE leitor SET Nome = @nome, CPF=@cpf, Endereco=@endereco, DataNasc=@dataNasc WHERE idLeitor='" + leitor.Id + "'";

            _bd.LimparParametros();
            _bd.AdicionarParametro("@nome", leitor.Nome);
            _bd.AdicionarParametro("@cpf", leitor.Cpf);
            _bd.AdicionarParametro("@endereco", leitor.Endereco);
            _bd.AdicionarParametro("@dataNasc", leitor.DataNasc.ToString());

            _bd.AbrirConexao();
            int rows = _bd.ExecutarNonQuery(sql);
            _bd.FecharConexao();
            if (rows > 0)
            {
                msg = "Leitor alterado com Sucesso!";
            }
            return msg;
        }

        public Leitor BuscaLeitorPorId(int id)
        {
            string sql = "SELECT * FROM leitor WHERE idLeitor=@id";
            _bd.LimparParametros();
            _bd.AdicionarParametro("@id", id.ToString());
            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            Leitor leitor = null;

            if (dt.Rows.Count > 0)
            {
                leitor = new Leitor();
                leitor.Id = Convert.ToInt32(dt.Rows[0]["idLeitor"].ToString());
                leitor.Nome = dt.Rows[0]["Nome"].ToString();
                leitor.Cpf = dt.Rows[0]["CPF"].ToString();
                leitor.Endereco = dt.Rows[0]["Endereco"].ToString();
                leitor.DataNasc = Convert.ToDateTime(dt.Rows[0]["DataNasc"].ToString());
            }

            return leitor;


        }

        public Leitor BuscaLeitorPorCPF(string cpf)
        {
            string sql = "SELECT * FROM leitor WHERE CPF=@cpf";
            _bd.LimparParametros();
            _bd.AdicionarParametro("@cpf", cpf);
            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            Leitor leitor = null;

            if (dt.Rows.Count > 0)
            {
                leitor = new Leitor();
                leitor.Id = Convert.ToInt32(dt.Rows[0]["idLeitor"].ToString());
                leitor.Nome = dt.Rows[0]["Nome"].ToString();
                leitor.Cpf = dt.Rows[0]["CPF"].ToString();
                leitor.Endereco = dt.Rows[0]["Endereco"].ToString();
                leitor.DataNasc = Convert.ToDateTime(dt.Rows[0]["DataNasc"].ToString());
            }

            return leitor;


        }
    }
}
