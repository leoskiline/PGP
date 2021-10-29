using Engenharia2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.DAL
{
    public class PosicaoDAL
    {
        MySQLPersistence _bd = new MySQLPersistence();

        public Posicao gravar(string setor, string prateleira)
        {
            Posicao pos;
            string sql = "INSERT INTO posicao (Setor,Prateleira) VALUES (@setor,@prateleira);Select @@IDENTITY;";
            _bd.LimparParametros();
            _bd.AdicionarParametro("@setor", setor);
            _bd.AdicionarParametro("@prateleira", prateleira);
            if (Existe(setor, prateleira) != null)
            {
                pos = Existe(setor, prateleira);
            }
            else
            {
                _bd.AbrirConexao();
                int id = _bd.ExecutarNonQueryAndGetID(sql);
                pos = obterPorID(id);
            }
            _bd.FecharConexao();
            return pos;
        }

        public Posicao Existe(string setor,string prateleira)
        {
            Posicao pos = null;
            string sql = "SELECT * FROM posicao WHERE Setor = @setor AND Prateleira = @prateleira";
            _bd.LimparParametros();
            _bd.AdicionarParametro("@setor", setor);
            _bd.AdicionarParametro("@prateleira", prateleira);
            _bd.AbrirConexao();
            DataTable poss = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            if (poss.Rows.Count > 0)
            {
                pos = new Posicao()
                {
                    Id = Convert.ToInt32(poss.Rows[0]["idPosicao"]),
                    Setor = poss.Rows[0]["Setor"].ToString(),
                    Prateleira = poss.Rows[0]["Prateleira"].ToString()

                };
            }
            return pos;
        }
        public Posicao obterPorID(int id)
        {
            Posicao pos = null;
            string sql = "SELECT * FROM posicao WHERE idPosicao = @id";
            _bd.AdicionarParametro("@id", id.ToString());
            _bd.AbrirConexao();
            DataTable poss = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            if (poss.Rows.Count > 0)
            {
                pos = new Posicao()
                {
                   Id = Convert.ToInt32(poss.Rows[0]["idPosicao"]),
                   Setor = poss.Rows[0]["Setor"].ToString(),
                   Prateleira = poss.Rows[0]["Prateleira"].ToString()

                };
            }
            return pos;
        }
    }
}
