using Engenharia2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.DAL
{
    public class AtendenteDAL
    {
        MySQLPersistence _bd = new MySQLPersistence();

        public int obterIdPorNome(string nome)
        {
            int id = 0;
            string sql = "SELECT idAtendente FROM atendente WHERE Nome = @nome";
            _bd.AdicionarParametro("@nome", nome);
            _bd.AbrirConexao();
            DataTable atd = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            if (atd.Rows.Count > 0)
            {
                id = Convert.ToInt32(atd.Rows[0]["idAtendente"]);
            }
            return id;
        }

        public Atendente obterAtendentePorNome(string nome)
        {
            string sql = "SELECT * FROM atendente WHERE Nome=@nome";
            _bd.LimparParametros();
            _bd.AdicionarParametro("@nome", nome);
            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            Atendente atendente = null;

            if (dt.Rows.Count > 0)
            {
                atendente = new Atendente();
                atendente.Id= Convert.ToInt32(dt.Rows[0]["idAtendente"].ToString());
                atendente.Nome = dt.Rows[0]["Nome"].ToString();
                atendente.Endereco = dt.Rows[0]["Endereco"].ToString();
                atendente.Telefone = dt.Rows[0]["Telefone"].ToString();
            }

            return atendente;

        }
        
        public Atendente Autenticar(string email,string senha)
        {
            string sql = "SELECT * FROM atendente WHERE email='" + email + "' AND senha='" + senha + "'";
            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            Atendente at = null;
            if(dt.Rows.Count>0)
            {
                at = new Atendente()
                {
                    Id = Convert.ToInt32(dt.Rows[0]["idAtendente"]),
                    Nome = dt.Rows[0]["Nome"].ToString(),
                    Cpf = dt.Rows[0]["CPF"].ToString(),
                    Endereco = dt.Rows[0]["Endereco"].ToString(),
                    Telefone = dt.Rows[0]["Telefone"].ToString(),
                    Status = dt.Rows[0]["Status"].ToString(),
                    Senha = dt.Rows[0]["Senha"].ToString(),
                    Email = dt.Rows[0]["Email"].ToString()
                };
            }
            return at;
        }
    }
}
