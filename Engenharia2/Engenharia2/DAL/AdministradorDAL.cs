using Engenharia2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.DAL
{
    public class AdministradorDAL
    {
        MySQLPersistence _bd = new MySQLPersistence();

        public int obterIdPorNome(string nome)
        {
            int id = 0;
            string sql = "SELECT idAdministrador FROM administrador WHERE Nome = @nome";
            _bd.AdicionarParametro("@nome", nome);
            _bd.AbrirConexao();
            DataTable admin = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            if(admin.Rows.Count > 0)
            {
                id = Convert.ToInt32(admin.Rows[0]["idAdministrador"]);                 
            }
            return id;
        }

        public Administrador Autenticar(string email,string senha)
        {
            string sql = "SELECT * FROM administrador WHERE email='" + email + "' AND senha='" + senha + "'";
            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            Administrador adm = null;
            if(dt.Rows.Count>0)
            {
                adm = new Administrador()
                {
                    Id = Convert.ToInt32(dt.Rows[0]["idAdministrador"]),
                    Nome = dt.Rows[0]["Nome"].ToString(),
                    Cpf = dt.Rows[0]["CPF"].ToString(),
                    Endereco = dt.Rows[0]["Endereco"].ToString(),
                    Telefone = dt.Rows[0]["Telefone"].ToString(),
                    Status = dt.Rows[0]["Status"].ToString(),
                    Senha = dt.Rows[0]["Senha"].ToString(),
                    Email = dt.Rows[0]["Email"].ToString()
                };
            }
            return adm;
        }

        public Administrador obter(string nome)
        {
            Administrador adm = null;            
            string sql = "SELECT * FROM administrador WHERE Nome = @nome";
            _bd.AdicionarParametro("@nome", nome);
            _bd.AbrirConexao();
            DataTable admin = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            if (admin.Rows.Count > 0)
            {
                adm = new Administrador()
                {
                    Id = Convert.ToInt32(admin.Rows[0]["idAdministrador"]),
                    Nome = admin.Rows[0]["Nome"].ToString(),
                    Cpf = admin.Rows[0]["CPF"].ToString(),
                    Endereco = admin.Rows[0]["Endereco"].ToString(),
                    Telefone = admin.Rows[0]["Telefone"].ToString()

                };
            }
            return adm;
        }


    }
}
